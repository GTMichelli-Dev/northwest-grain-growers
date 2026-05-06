using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models; // <-- Add this using directive
using GrainManagement.Auth;
using GrainManagement.Hubs;
using Microsoft.AspNetCore.HttpOverrides;
using GrainManagement.Services.Warehouse;
using Microsoft.Extensions.FileProviders;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

	// Branding / theming (server-controlled)
	builder.Services.Configure<BrandingOptions>(builder.Configuration.GetSection("Branding"));

// Module system — controls which features are active for this deployment
builder.Services.Configure<ModuleOptions>(builder.Configuration.GetSection(ModuleOptions.SectionName));
builder.Services.AddSingleton<IModuleContext, ModuleContext>();

// App readiness flag — set after EF warm-up
builder.Services.AddSingleton<AppReadiness>();

// DevExpress Reporting — web document viewer
builder.Services.AddDevExpressControls();
builder.Services.AddScoped<DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension, ReportStorageService>();
builder.Services.ConfigureReportingServices(configurator =>
{
    configurator.ConfigureWebDocumentViewer(viewerConfigurator =>
    {
        viewerConfigurator.UseCachedReportSourceBuilder();
    });
});

// Authentication + Microsoft Entra ID (OIDC)
// Also enable token acquisition so you can inject ITokenAcquisition in controllers.
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi(new[] { "Group.Read.All" })
    .AddDistributedTokenCaches();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<ILocationContext, LocationContext>();
builder.Services.AddScoped<IServerInfoProvider, ServerInfoProvider>();
builder.Services.AddSingleton<IScaleRegistry, ScaleRegistry>();
builder.Services.AddSingleton<SystemInfoSnapshot>();
builder.Services.AddScoped<IWarehouseDashboardService, DummyWarehouseDashboardService>();
// SignalR (Warehouse realtime refresh)
//builder.Services.AddSignalR();

// Warehouse data abstraction (dummy now, real later)
builder.Services.AddScoped<IWarehouseIntakeDataService, DummyWarehouseIntakeDataService>();

// ── Scale Reader Service (now runs as a separate standalone service) ─────
// Scale configs and polling are managed by the external ScaleReaderService process.
// The web app receives scale updates via SignalR and serves the config UI.

// ── Camera Service (hosted background worker) ──────────────────────────
builder.Services.AddSingleton<GrainManagement.Services.Camera.RestartSignal>();
builder.Services.AddSingleton<GrainManagement.Services.Camera.AnnounceSignal>();
builder.Services.AddDbContext<GrainManagement.Services.Camera.Data.CameraDbContext>(opt =>
    opt.UseSqlite($"Data Source={Path.Combine(AppContext.BaseDirectory, "cameraservice.db")}"));
builder.Services.AddScoped<GrainManagement.Services.Camera.CameraCaptureService>();
builder.Services.AddHostedService<GrainManagement.Services.Camera.CameraWorker>();

// ── Print Service (hosted background worker) ────────────────────────────
builder.Services.AddSingleton<GrainManagement.Services.Print.CupsClient>();
builder.Services.AddSingleton<GrainManagement.Services.Print.RestartSignal>();
builder.Services.AddSingleton<GrainManagement.Services.Print.Data.PrintDbContext>();
builder.Services.AddHostedService<GrainManagement.Services.Print.PrintWorker>();
builder.Services.Configure<GrainManagement.Services.Print.PrintDispatchOptions>(
    builder.Configuration.GetSection(GrainManagement.Services.Print.PrintDispatchOptions.SectionName));
builder.Services.AddScoped<GrainManagement.Services.Print.IPrintDispatchService, GrainManagement.Services.Print.PrintDispatchService>();

// Pushes "weight sheet updated" SignalR notifications to subscribed pages.
builder.Services.AddScoped<GrainManagement.Services.IWeightSheetNotifier, GrainManagement.Services.WeightSheetNotifier>();



builder.Services.AddDistributedSqlServerCache(o =>
{
    o.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    o.SchemaName = "web";
    o.TableName = "TokenCache";
});

builder.Services.AddScoped<IJsonLog, JsonLog>();


builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = null;
        o.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });

// SignalR JSON protocol
builder.Services.AddSignalR()
    .AddJsonProtocol(o =>
    {
        o.PayloadSerializerOptions.PropertyNamingPolicy = null;
        o.PayloadSerializerOptions.DictionaryKeyPolicy = null;
    });

builder.Services.AddAuthorization(options =>
{
    var grp = builder.Configuration.GetSection("GrainSecurity");
    string adminId = grp["AdminGroupId"]!;
    string managerId = grp["ManagerGroupId"]!;
    string userId = grp["UserGroupId"]!;

    // Use ClaimConstants.Groups ("groups") from Microsoft.Identity.Web
    options.AddPolicy("GrainAdmin", p => p.RequireClaim("groups", adminId));
    options.AddPolicy("GrainManager", p => p.RequireClaim("groups", managerId));
    options.AddPolicy("GrainUser", p => p.RequireClaim("groups", userId));

});

// Small DI service to expose role booleans per request
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IDeviceContext, DeviceContext>();

builder.Services.AddScoped<ILocationService, sqlLocationService>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GrainManagement API", Version = "v1" });
});



// Force Authorization Code + PKCE and control logout redirect
builder.Services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    options.ResponseType = "code";
    options.UsePkce = true;
    options.SaveTokens = true;

    // After remote sign-out completes, land on Home/Index (instead of the built-in SignedOut page)
    options.SignedOutCallbackPath = "/signout-callback-oidc"; // optional (default is fine)
    options.SignedOutRedirectUri = "/Home/Index";

    options.Events ??= new OpenIdConnectEvents();
    options.Events.OnSignedOutCallbackRedirect = context =>
    {
        context.Response.Redirect("/Home/Index");
        context.HandleResponse();
        return Task.CompletedTask;
    };

    // ── Fetch Azure AD group memberships on sign-in ──────────────────────
    // Groups are added to the ClaimsPrincipal BEFORE the auth cookie is
    // created, so they persist across all subsequent requests.  This avoids
    // the race / cache-miss issue where IClaimsTransformation's Graph call
    // fails silently and the user appears to have no roles.
    var previousOnTokenValidated = options.Events.OnTokenValidated;
    options.Events.OnTokenValidated = async context =>
    {
        // Run existing (MSAL) handler first — it stores the tokens in cache
        if (previousOnTokenValidated != null)
            await previousOnTokenValidated(context);

        // If groups already came in the ID token, nothing to do
        if (context.Principal?.HasClaim(c => c.Type == "groups") == true)
            return;

        try
        {
            var tokenAcq = context.HttpContext.RequestServices
                .GetRequiredService<ITokenAcquisition>();
            var httpFactory = context.HttpContext.RequestServices
                .GetRequiredService<IHttpClientFactory>();
            var logger = context.HttpContext.RequestServices
                .GetService<ILogger<GroupClaimsTransformation>>();

            var token = await tokenAcq.GetAccessTokenForUserAsync(
                new[] { "Group.Read.All" },
                user: context.Principal);

            var client = httpFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var ids = new List<string>();
#nullable enable
            string? next = "https://graph.microsoft.com/v1.0/me/transitiveMemberOf/microsoft.graph.group?$select=id";
#nullable restore

            while (!string.IsNullOrEmpty(next))
            {
                var json = await client.GetStringAsync(next);
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("value", out var value))
                    foreach (var item in value.EnumerateArray())
                        if (item.TryGetProperty("id", out var idProp))
                            ids.Add(idProp.GetString()!);

                next = doc.RootElement.TryGetProperty("@odata.nextLink", out var nl)
                    ? nl.GetString()
                    : null;
            }

            if (context.Principal?.Identity is ClaimsIdentity id && ids.Count > 0)
            {
                foreach (var gid in ids.Distinct(StringComparer.OrdinalIgnoreCase))
                    id.AddClaim(new Claim("groups", gid));
            }

            logger?.LogInformation(
                "OnTokenValidated: added {Count} group claims for {User}",
                ids.Count,
                context.Principal?.FindFirst("preferred_username")?.Value);
        }
        catch (Exception ex)
        {
            var logger = context.HttpContext.RequestServices
                .GetService<ILogger<GroupClaimsTransformation>>();
            logger?.LogWarning(ex,
                "OnTokenValidated: failed to fetch groups for {User} — " +
                "GroupClaimsTransformation will retry on each request",
                context.Principal?.FindFirst("preferred_username")?.Value);
        }
    };
});


builder.Services.AddDbContext<dbContext>((serviceProvider, options) =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var httpContext = httpContextAccessor.HttpContext;

    var connectionString = configuration.GetConnectionString("DefaultConnection");

    var endpoint = httpContext?.GetEndpoint();
    var useAdminConn = endpoint?.Metadata.GetMetadata<UseAdminConnectionAttribute>() != null;
    if (useAdminConn)
    {
        connectionString = configuration.GetConnectionString("AdminConnection");
    }

    options.UseSqlServer(connectionString, sql =>
    {
        sql.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    });
});


//builder.Services.AddDbContext<dbContext>((serviceProvider, options) =>
//{
//    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
//    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
//    var httpContext = httpContextAccessor.HttpContext;

//    var connectionString = configuration.GetConnectionString("DefaultConnection");

//    var endpoint = httpContext?.GetEndpoint();
//    var useAdminConn = endpoint?.Metadata.GetMetadata<UseAdminConnectionAttribute>() != null;
//    if (useAdminConn)
//    {
//        connectionString = configuration.GetConnectionString("AdminConnection");
//    }

//    options.UseSqlServer(connectionString);
//});

//builder.Services.AddDbContext<dbContext>(
//      options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC + Microsoft Identity UI (for login/logout endpoints)
builder.Services.AddRazorPages().AddMicrosoftIdentityUI();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);

// HttpClient for downstream API/Graph calls
builder.Services.AddHttpClient();
builder.Services.AddTransient<IClaimsTransformation, GroupClaimsTransformation>();
// Load appdevelopmentsettings.json only in Development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appdevelopmentsettings.json", optional: true, reloadOnChange: true);
}

var app = builder.Build();

app.Use(async (context, next) =>
{
    var deviceId = context.Request.Headers["X-Device-Id"].FirstOrDefault()
                   ?? context.Connection.RemoteIpAddress?.ToString();

    if (!string.IsNullOrEmpty(deviceId))
    {
        context.Items["DeviceId"] = deviceId;
    }

    await next();
});


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
       
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GrainManagement API V1");
        c.RoutePrefix = "swagger"; // Swagger UI at /swagger
    });
}

var fh = new ForwardedHeadersOptions
{
    ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost
};

fh.KnownNetworks.Clear();
fh.KnownProxies.Clear();

app.UseForwardedHeaders(fh);

//app.UseHttpsRedirection();
app.UseDevExpressControls();
app.UseStaticFiles();

  
     
var ticketPath = builder.Configuration["TicketImages:PhysicalPath"];
var requestPath = builder.Configuration["TicketImages:RequestPath"];

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(ticketPath),
    RequestPath = requestPath
});



app.UseRouting();

// Resolve active theme for each request (users cannot override)
app.UseMiddleware<ThemeMiddleware>();




// TEMP: authentication disabled for local testing — re-enable before deploy!
//app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// SignalR hubs
app.MapHub<ScaleHub>("/hubs/scale");

app.MapHub<PrintHub>("/hubs/print");

app.MapHub<WarehouseHub>(WarehouseHub.HubRoute);

app.MapRazorPages();

// ── Warm-up: force EF Core model compilation + first DB connection at startup ──
// CanConnect() only tests the raw ADO.NET connection; a real LINQ query forces
// EF to compile the full model (entity mappings, relationships, etc.) which is
// the actual source of the cold-start lag.
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<dbContext>();
    // Warm up core tables and their query plans
    _ = ctx.Locations.AsNoTracking().Select(l => l.LocationId).FirstOrDefault();
    _ = ctx.Lots.AsNoTracking().Include(l => l.SplitGroup).Select(l => l.LotId).FirstOrDefault();
    _ = ctx.WeightSheets.AsNoTracking().Select(w => w.WeightSheetId).FirstOrDefault();
    _ = ctx.Items.AsNoTracking().Select(i => i.ItemId).FirstOrDefault();
}

// Mark app as ready (EF warm-up complete)
app.Services.GetRequiredService<AppReadiness>().MarkReady();

// Build system info string once at startup (assembly info + server friendly name)
var systemInfo = app.Services.GetRequiredService<SystemInfoSnapshot>();
await systemInfo.EnsureServerInfoLoadedAsync();

await app.RunAsync();
