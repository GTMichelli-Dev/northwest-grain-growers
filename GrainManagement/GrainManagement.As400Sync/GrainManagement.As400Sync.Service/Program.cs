using GrainManagement.As400Sync;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<As400SyncOptions>(builder.Configuration.GetSection("As400Sync"));
builder.Services.AddHostedService<As400SyncWorker>();

builder.Services.AddSingleton<As400Reader>();
builder.Services.AddSingleton<AccountsUpserter>();
builder.Services.AddSingleton<ProductItemsUpserter>();
builder.Services.AddSingleton<SplitGroupsUpserter>();
builder.Services.AddSingleton<SyncCoordinator>();
builder.Services.AddSingleton<As400SyncRunner>();

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

// API key enforcement for /api/sync/* except status
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/sync")
        && !context.Request.Path.StartsWithSegments("/api/sync/status"))
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

app.Run();