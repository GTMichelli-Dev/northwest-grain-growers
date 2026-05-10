using GrainManagement.As400Sync;
using Microsoft.OpenApi.Models;
using System.Reflection;

// Resolve the build version once for both the startup banner and /api/sync/info.
static string ResolveAssemblyVersion()
{
    var asm = Assembly.GetExecutingAssembly();
    var info = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    if (!string.IsNullOrWhiteSpace(info))
    {
        // Trim build metadata + commit hash if present (e.g. "1.0.1+abcdef0").
        var plus = info.IndexOf('+');
        return plus > 0 ? info[..plus] : info;
    }
    return asm.GetName().Version?.ToString() ?? "0.0.0";
}

var serviceVersion = ResolveAssemblyVersion();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<As400SyncOptions>(builder.Configuration.GetSection("As400Sync"));

// Auto-run worker. Defaults in As400SyncOptions ship with all Sync* flags
// false, so this is a no-op until an admin opts in via appsettings.
builder.Services.AddHostedService<As400SyncWorker>();

// Outbound SignalR client. Connects to the website's /hubs/as400sync hub,
// registers as a service, listens for run commands, and streams progress.
builder.Services.AddHostedService<As400SyncHubClient>();

builder.Services.AddSingleton<As400Reader>();
builder.Services.AddSingleton<AccountsUpserter>();
builder.Services.AddSingleton<ProductItemsUpserter>();
builder.Services.AddSingleton<SplitGroupsUpserter>();
builder.Services.AddSingleton<SyncCoordinator>();
builder.Services.AddSingleton<As400SyncRunner>();
builder.Services.AddSingleton<WarehouseTransferUploader>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sync From Agvantage",
        Version = "v1",
        Description = "Internal endpoints to trigger Agvantage sync jobs"
    });

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
        Description = "Internal API key required for /api/sync endpoints"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ── Startup banner ──────────────────────────────────────────────────────────
{
    var envLabelStartup = app.Environment.EnvironmentName;

    var configuredUrls = builder.Configuration
        .GetSection("Kestrel:Endpoints").GetChildren()
        .Select(e => e["Url"])
        .Where(u => !string.IsNullOrWhiteSpace(u))
        .ToArray();

    if (configuredUrls.Length == 0)
    {
        var fallback = builder.Configuration["Urls"];
        if (!string.IsNullOrWhiteSpace(fallback))
            configuredUrls = new[] { fallback };
    }

    var listeningDisplay = configuredUrls.Length > 0
        ? string.Join(", ", configuredUrls!)
        : "(default)";

    // Build a browseable Swagger link. 0.0.0.0 / + / * aren't browseable,
    // so present them as localhost.
    static string ToBrowseUrl(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return raw;
        return raw.Replace("0.0.0.0", "localhost")
                  .Replace("://+",   "://localhost")
                  .Replace("://*",   "://localhost");
    }

    var swaggerBase = configuredUrls.FirstOrDefault();
    var swaggerUrl = string.IsNullOrWhiteSpace(swaggerBase)
        ? "(start the service to view)"
        : ToBrowseUrl(swaggerBase!.TrimEnd('/')) + "/swagger";

    var bar = new string('─', 70);
    Console.WriteLine();
    Console.WriteLine(bar);
    Console.WriteLine($" GrainManagement.As400Sync   v{serviceVersion}");
    Console.WriteLine($" Environment : {envLabelStartup}");
    Console.WriteLine($" Listening   : {listeningDisplay}");
    Console.WriteLine($" Swagger UI  : Sync From Agvantage ({envLabelStartup.ToUpperInvariant()})");
    Console.WriteLine($"               {swaggerUrl}");
    Console.WriteLine(bar);
    Console.WriteLine();
}

// API key enforcement for /api/sync/* except status
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/sync")
        && !context.Request.Path.StartsWithSegments("/api/sync/status")
        && !context.Request.Path.StartsWithSegments("/api/sync/info"))
    {
        var expected = builder.Configuration["Security:ApiKey"];
        var provided = context.Request.Headers["X-Api-Key"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(expected) || provided != expected)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid or missing X-Api-Key");
            return;
        }
    }

    await next();
});
app.UseStaticFiles();
app.UseSwagger();

var envLabel = app.Environment.EnvironmentName.ToUpperInvariant();
var envColor = app.Environment.IsDevelopment() ? "#2563eb" : "#b91c1c";

app.UseSwaggerUI(c =>
{
    c.DocumentTitle = $"GrainManagement  Sync From Agvantage({envLabel})";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AS400 Sync v1");

    c.InjectStylesheet("/swagger/custom.css?v=6");

});



// endpoints...
app.MapPost("/api/sync/accounts", async (SyncCoordinator coord, As400SyncRunner runner, CancellationToken ct) =>
{
    await using var lease = await coord.TryAcquireAsync(SyncJob.Accounts, ct);
    if (lease is null) return Results.Conflict(new { ok = false, job = "Accounts", message = "A sync is already running." });

    await runner.RunAsync(SyncJob.Accounts, ct);
    return Results.Ok(new { ok = true, job = "Accounts", message = "Accounts sync completed." });
});

app.MapPost("/api/sync/products", async (SyncCoordinator coord, As400SyncRunner runner, CancellationToken ct) =>
{
    await using var lease = await coord.TryAcquireAsync(SyncJob.Products, ct);
    if (lease is null) return Results.Conflict(new { ok = false, job = "Products", message = "A sync is already running." });

    await runner.RunAsync(SyncJob.Products, ct);
    return Results.Ok(new { ok = true, job = "Products", message = "Products sync completed." });
});

app.MapPost("/api/sync/splitgroups", async (SyncCoordinator coord, As400SyncRunner runner, CancellationToken ct) =>
{
    await using var lease = await coord.TryAcquireAsync(SyncJob.SplitGroups, ct);
    if (lease is null) return Results.Conflict(new { ok = false, job = "SplitGroups", message = "A sync is already running." });

    await runner.RunAsync(SyncJob.SplitGroups, ct);
    return Results.Ok(new { ok = true, job = "SplitGroups", message = "SplitGroups sync completed." });
});

app.MapGet("/api/sync/status", (SyncCoordinator coord) =>
{
    return Results.Ok(new
    {
        accounts = new { running = coord.IsRunning(SyncJob.Accounts, out var a), startedAtUtc = a },
        products = new { running = coord.IsRunning(SyncJob.Products, out var p), startedAtUtc = p },
        splitGroups = new { running = coord.IsRunning(SyncJob.SplitGroups, out var s), startedAtUtc = s }
    });
});

// Combined version + status. Anonymous (no API key) so the website can poll
// it cheaply for the admin "service info" panel.
app.MapGet("/api/sync/info", (SyncCoordinator coord, IConfiguration cfg) =>
{
    var accBusy   = coord.IsRunning(SyncJob.Accounts,    out var accStarted);
    var prodBusy  = coord.IsRunning(SyncJob.Products,    out var prodStarted);
    var splitBusy = coord.IsRunning(SyncJob.SplitGroups, out var splitStarted);

    return Results.Ok(new
    {
        service     = "GrainManagement.As400Sync",
        version     = serviceVersion,
        environment = app.Environment.EnvironmentName,
        serviceId   = cfg["As400Sync:ServiceId"] ?? "as400sync",
        hubUrl      = cfg["As400Sync:HubUrl"]    ?? "",
        busy        = accBusy || prodBusy || splitBusy,
        accounts    = new { running = accBusy,   startedAtUtc = accStarted   },
        products    = new { running = prodBusy,  startedAtUtc = prodStarted  },
        splitGroups = new { running = splitBusy, startedAtUtc = splitStarted }
    });
});

app.Run();