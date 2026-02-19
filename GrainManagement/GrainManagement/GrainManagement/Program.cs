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


var builder = WebApplication.CreateBuilder(args);

	// Branding / theming (server-controlled)
	builder.Services.Configure<BrandingOptions>(builder.Configuration.GetSection("Branding"));

// Authentication + Microsoft Entra ID (OIDC)
// Also enable token acquisition so you can inject ITokenAcquisition in controllers.
builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddDistributedTokenCaches();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IServerInfoProvider, ServerInfoProvider>();
builder.Services.AddSingleton<IScaleRegistry, ScaleRegistry>();

// SignalR (Warehouse realtime refresh)
//builder.Services.AddSignalR();

// Warehouse data abstraction (dummy now, real later)
builder.Services.AddScoped<IWarehouseIntakeDataService, DummyWarehouseIntakeDataService>();



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

if (builder.Configuration.GetValue<bool>("UseDemoData"))
{
    builder.Services.AddScoped<ILocationService, DemoLocationService>();
}
else
{
    builder.Services.AddScoped<ILocationService, sqlLocationService>();
}



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
app.UseStaticFiles();

app.UseRouting();

// Resolve active theme for each request (users cannot override)
app.UseMiddleware<ThemeMiddleware>();




app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// SignalR hubs
app.MapHub<ScaleHub>("/hubs/scale");
app.MapHub<CameraControlHub>("/hubs/camera");

app.MapHub<PrintHub>("/hubs/print");

app.MapHub<WarehouseHub>(WarehouseHub.HubRoute);

app.MapRazorPages();

app.Run();
