using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using GrainManagement.Auth;
using GrainManagement.Hubs;
using Microsoft.AspNetCore.HttpOverrides;
using GrainManagement.Services.Warehouse;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

	// Branding / theming (server-controlled)
	builder.Services.Configure<BrandingOptions>(builder.Configuration.GetSection("Branding"));

// Module system — controls which features are active for this deployment
builder.Services.Configure<ModuleOptions>(builder.Configuration.GetSection(ModuleOptions.SectionName));
builder.Services.AddSingleton<IModuleContext, ModuleContext>();

// Outbound email (smtp2go credentials in appsettings:Email; TestMode diverts
// every send to a single configurable inbox so dev / staging never reaches
// producers).
builder.Services.Configure<GrainManagement.Services.Email.EmailOptions>(
    builder.Configuration.GetSection("Email"));
builder.Services.AddSingleton<GrainManagement.Services.Email.IEmailService,
    GrainManagement.Services.Email.EmailService>();

// End Of Day report data service — loads the three daily summary DTOs that
// back the new XtraReports.
builder.Services.AddScoped<GrainManagement.Services.Warehouse.IEndOfDayReportService,
    GrainManagement.Services.Warehouse.EndOfDayReportService>();

// Historical report data service — backs the on-demand Reports views
// (Daily Intake/Transfer picker, WS Series picker, Commodities table).
builder.Services.AddScoped<GrainManagement.Services.Warehouse.IReportBuilderService,
    GrainManagement.Services.Warehouse.ReportBuilderService>();

// Central deployment home-page dashboard — per-location intake / transfer
// totals for a date range.
builder.Services.AddScoped<GrainManagement.Services.Warehouse.ICentralDashboardService,
    GrainManagement.Services.Warehouse.CentralDashboardService>();

// Prior-day open-WS guard — used by the WS / load create endpoints to
// hard-block new work while yesterday's weight sheets are still open.
builder.Services.AddScoped<GrainManagement.Services.Warehouse.IPriorDayWeightSheetGuard,
    GrainManagement.Services.Warehouse.PriorDayWeightSheetGuard>();

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

// Authentication is handled per-action via PINs (see AuthController +
// RemoteAdminController + the Privileges constants). No identity-provider
// wiring at the request pipeline level.

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

// ── Camera registry ─────────────────────────────────────────────────────
// External CameraService instances connect via SignalR (CameraHub) and
// announce their hardware. The registry is the web-side view of "what
// cameras exist right now"; role/scale/BOL assignments live in
// system.CameraAssignments (managed via /api/cameras/assignments).
builder.Services.AddSingleton<GrainManagement.Services.Camera.ICameraRegistry,
                              GrainManagement.Services.Camera.CameraRegistry>();
builder.Services.AddScoped<GrainManagement.Services.Camera.ICameraCaptureTrigger,
                           GrainManagement.Services.Camera.CameraCaptureTrigger>();

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

// ── Temp Weight Tickets (kiosk button-press → captured weight) ──────────
// Motion-wait orchestrator, REST API, nightly purge of orphans.
builder.Services.AddSingleton<GrainManagement.Services.TempTickets.ITempTicketOrchestrator,
                              GrainManagement.Services.TempTickets.TempTicketOrchestrator>();
builder.Services.AddHostedService<GrainManagement.Services.TempTickets.TempTicketPurgeWorker>();

// ── Ticket-image compositor (multi-camera stitching + watermark) ────────
// Sites with multiple cameras per (scale, role) upload per-camera frames
// to /api/ticket/{load}/image?cameraId=X. The coalescer waits for the
// uploads to settle, then the compositor stitches a single canonical
// {load}_{dir}.jpg.
builder.Services.AddSingleton<GrainManagement.Services.Images.TicketImageCoalescer>();
builder.Services.AddScoped<GrainManagement.Services.Images.ITicketImageCompositor,
                           GrainManagement.Services.Images.TicketImageCompositor>();



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

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDeviceContext, DeviceContext>();

builder.Services.AddScoped<ILocationService, sqlLocationService>();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GrainManagement API", Version = "v1" });
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

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddHttpClient();

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




// Authorization pipeline kept for future per-action attribute use.
// No identity provider is registered; PIN checks happen inside actions.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// SignalR hubs
app.MapHub<ScaleHub>("/hubs/scale");

app.MapHub<PrintHub>("/hubs/print");

app.MapHub<WarehouseHub>(WarehouseHub.HubRoute);

app.MapHub<As400SyncHub>(As400SyncHub.HubRoute);

app.MapHub<CameraHub>(CameraHub.HubRoute);

app.MapHub<TempTicketKioskHub>(TempTicketKioskHub.HubRoute);

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
