using KioskPrintAgent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<PrintAgentOptions>(
    builder.Configuration.GetSection("PrintAgent"));

builder.Services.AddHttpClient("TicketApi", client =>
{
    var serverUrl = builder.Configuration["PrintAgent:ServerUrl"] ?? "http://localhost:5110";
    client.BaseAddress = new Uri(serverUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHostedService<PrintService>();

var host = builder.Build();
await host.RunAsync();
