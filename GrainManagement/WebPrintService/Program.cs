using WebPrintService;
using WebPrintService.Services;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Web Print Service", Version = "v1" });
});
builder.Services.AddControllers();

// Configuration — bind from appsettings.json "Print" section
builder.Services.Configure<PrintServiceOptions>(builder.Configuration.GetSection(PrintServiceOptions.SectionName));

// Services — register the right print client based on OS
if (OperatingSystem.IsWindows())
{
    builder.Services.AddSingleton<IPrintClient, WindowsPrintClient>();
}
else
{
    builder.Services.AddSingleton<IPrintClient, CupsClient>();
}
builder.Services.AddSingleton<RestartSignal>();
builder.Services.AddHostedService<PrintWorker>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Banner
var port = builder.Configuration["Print:Port"] ?? "5230";
app.Urls.Add($"http://*:{port}");

var version = typeof(Program).Assembly.GetName().Version?.ToString(3) ?? "0.0.0";
var startLog = app.Services.GetRequiredService<ILogger<Program>>();
var printSystem = OperatingSystem.IsWindows() ? "Windows Print" : "CUPS (Linux/macOS)";
var serverUrls = builder.Configuration.GetSection("Print:ServerUrls").Get<string[]>() ?? new[] { "http://localhost:5000" };

Console.WriteLine();
Console.WriteLine("  =============================================");
Console.WriteLine($"   Web Print Service  v{version}");
Console.WriteLine($"   Print System:  {printSystem}");
Console.WriteLine($"   Listening on:  http://localhost:{port}");
Console.WriteLine($"   Swagger:       http://localhost:{port}/swagger");
Console.WriteLine($"   Servers:       {string.Join(", ", serverUrls)}");
Console.WriteLine("  =============================================");
Console.WriteLine();

startLog.LogInformation("Web Print Service v{Version} started. PrintSystem={PrintSystem} Port={Port}",
    version, printSystem, port);

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
