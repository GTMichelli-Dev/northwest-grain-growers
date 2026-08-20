using BasicWeigh.Web.Data;
using BasicWeigh.Web.Hubs;
using BasicWeigh.Web.Services;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Suppress noisy EF Core SQL logging
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Query", LogLevel.Error);

// Database provider switching
var dbProvider = builder.Configuration["DatabaseProvider"] ?? "SQLite";
var connectionString = builder.Configuration.GetConnectionString(dbProvider)
    ?? "Data Source=BasicWeigh.db";

builder.Services.AddDbContext<ScaleDbContext>(options =>
{
    if (dbProvider == "MariaDB")
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});

// Scale service (simulated for demo mode)
builder.Services.AddSingleton<SimulatedScaleService>();
builder.Services.AddSingleton<IScaleService>(sp => sp.GetRequiredService<SimulatedScaleService>());
// Multi-scale weight store (tracks all scales with timeout detection)
builder.Services.AddSingleton<ScaleWeightStore>();

builder.Services.AddSingleton<PrintQueueService>();
builder.Services.AddSignalR(options =>
{
    // Increase max message size for camera image transfer (base64 images can be 200KB+)
    options.MaximumReceiveMessageSize = 1024 * 1024; // 1MB
});
builder.Services.AddHostedService<ScaleBroadcastService>();

// Cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.SlidingExpiration = true;
    });

builder.Services.AddControllersWithViews();

// AppSetup cache — single DB read, invalidated on save
builder.Services.AddSingleton<BasicWeigh.Web.Services.AppSetupCache>();

// Swagger / OpenAPI — always enabled, protected by ApiDefinitionPin
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Basic Weigh API Definitions",
        Version = "v1",
        Description = "API documentation for Basic Weigh truck scale management system."
    });
    // Exclude DevExpress and non-API controllers from Swagger
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"] ?? "";
        // Only include our own API endpoints (routes starting with api/)
        var relativePath = apiDesc.RelativePath ?? "";
        return relativePath.StartsWith("api/", StringComparison.OrdinalIgnoreCase);
    });
});

// DevExpress Reporting
builder.Services.AddDevExpressControls();
builder.Services.ConfigureReportingServices(configurator =>
{
    if (builder.Environment.IsDevelopment())
    {
        configurator.UseDevelopmentMode();
    }
    configurator.ConfigureReportDesigner(designerConfigurator =>
    {
        designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
    });
    configurator.ConfigureWebDocumentViewer(viewerConfigurator =>
    {
        viewerConfigurator.UseCachedReportSourceBuilder();
    });
});
DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new ReportStorageService());

var app = builder.Build();

// Initialize database and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
    context.Database.Migrate();
    DbInitializer.Seed(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseDevExpressControls();
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseStaticFiles();

// Swagger PIN protection middleware — must come before UseSwagger
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";
    if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
    {
        var db = context.RequestServices.GetRequiredService<ScaleDbContext>();
        var setup = db.AppSetup.First();
        var pinFromQuery = context.Request.Query["pin"].FirstOrDefault();
        var pin = pinFromQuery ?? context.Request.Cookies["SwaggerPin"];
        if (pin != setup.ApiDefinitionPin)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<html><body style='font-family:sans-serif;text-align:center;padding:60px;'><h2>Not Authorized</h2><p>A valid API Definition PIN is required to access the Swagger documentation.</p><p><a href='/'>Return to Basic Weigh</a></p></body></html>");
            return;
        }
        // Set cookie so subsequent requests don't need ?pin=
        context.Response.Cookies.Append("SwaggerPin", pin!, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddHours(24)
        });
        // If pin came via query string on the base /swagger path, redirect to
        // /swagger/index.html so the cookie is set before the page loads.
        // We must redirect (not rewrite) to preserve correct relative asset paths.
        if (pinFromQuery != null && (path.Equals("/swagger", StringComparison.OrdinalIgnoreCase)
            || path.Equals("/swagger/", StringComparison.OrdinalIgnoreCase)))
        {
            context.Response.Redirect("/swagger/index.html");
            return;
        }
    }
    await next();
});

// Swagger — always enabled (not just development)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basic Weigh API v1");
    c.DocumentTitle = "Basic Weigh API Definitions";
    c.InjectStylesheet("/css/swagger-custom.css");
    c.HeadContent = @"<link rel=""icon"" type=""image/x-icon"" href=""/api/setup/icon"" />";
});

app.UseRouting();
app.UseAuthentication();

// Middleware: if UseLogin is off, skip auth. If on, enforce it.
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? "";

    // Always allow: login page, static files, kiosk, scale API, SignalR, Swagger
    if (path.StartsWith("/Account/") ||
        path.StartsWith("/css/") || path.StartsWith("/js/") || path.StartsWith("/images/") ||
        path.StartsWith("/_content/") || path.StartsWith("/favicon") ||
        path.StartsWith("/api/scale/") || path.StartsWith("/scaleHub") ||
        path.StartsWith("/api/setup/icon") ||
        path.StartsWith("/swagger"))
    {
        await next();
        return;
    }

    // Kiosk access: check PIN if UseLogin is on
    if (path.StartsWith("/Kiosk") || path.StartsWith("/api/kiosk/"))
    {
        var db = context.RequestServices.GetRequiredService<ScaleDbContext>();
        var setup = db.AppSetup.First();
        if (setup.UseLogin)
        {
            var pin = context.Request.Query["pin"].FirstOrDefault()
                      ?? context.Request.Cookies["KioskPin"];
            if (pin != setup.KioskCode)
            {
                context.Response.Redirect("/Account/Login");
                return;
            }
            // Set cookie so subsequent requests don't need ?pin=
            if (!context.Request.Cookies.ContainsKey("KioskPin"))
            {
                context.Response.Cookies.Append("KioskPin", pin, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(24)
                });
            }
        }
        await next();
        return;
    }

    // Check if login is required
    var dbCheck = context.RequestServices.GetRequiredService<ScaleDbContext>();
    var appSetup = dbCheck.AppSetup.First();
    if (appSetup.UseLogin && !context.User.Identity!.IsAuthenticated)
    {
        context.Response.Redirect($"/Account/Login?returnUrl={Uri.EscapeDataString(path)}");
        return;
    }

    // Role-based access
    if (appSetup.UseLogin && context.User.Identity!.IsAuthenticated)
    {
        var role = context.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "User";

        // Setup page: Admin only
        if (path.StartsWith("/Setup") && role != "Admin")
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access denied. Admin role required.");
            return;
        }

        // User management: Admin only
        if (path.StartsWith("/Account/Users") || path.StartsWith("/Account/CreateUser") ||
            path.StartsWith("/Account/EditUser") || path.StartsWith("/Account/ResetPassword") ||
            path.StartsWith("/Account/DeleteUser"))
        {
            if (role != "Admin")
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Access denied. Admin role required.");
                return;
            }
        }

        // Edit Tables: Manager or Admin only
        if (path.StartsWith("/MasterData") && role == "User")
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access denied. Manager or Admin role required.");
            return;
        }
    }

    await next();
});

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ScaleHub>("/scaleHub");

app.Run();
