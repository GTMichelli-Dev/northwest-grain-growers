using GrainManagement.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace GrainManagement.Services.Print;

public sealed class PrintDispatchService : IPrintDispatchService
{
    private readonly IHubContext<PrintHub> _hub;
    private readonly ILogger<PrintDispatchService> _log;
    private readonly PrintDispatchOptions _options;

    public PrintDispatchService(
        IHubContext<PrintHub> hub,
        IOptions<PrintDispatchOptions> options,
        ILogger<PrintDispatchService> log)
    {
        _hub = hub;
        _log = log;
        _options = options.Value;
    }

    public async Task DispatchTicketAsync(string ticketId, string? printerTarget = null, string type = "weighout", CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ticketId))
            throw new ArgumentException("ticketId is required", nameof(ticketId));

        ParsePrinterTarget(printerTarget, out var serviceId, out var printerId);

        var group = !string.IsNullOrWhiteSpace(serviceId)
            ? $"Print_{serviceId}"
            : "PrintClients";

        _log.LogInformation("Dispatching print job Ticket={TicketId} Type={Type} Group={Group} PrinterId={PrinterId}",
            ticketId, type, group, printerId);

        await _hub.Clients.Group(group).SendAsync("PrintTicket",
            new { ticketId, type, printerId },
            cancellationToken);
    }

    private void ParsePrinterTarget(string? printerTarget, out string serviceId, out string printerId)
    {
        serviceId = _options.DefaultServiceId;
        printerId = "";

        if (string.IsNullOrWhiteSpace(printerTarget))
            return;

        var target = printerTarget.Trim();
        var parts = target.Split(':', 2, StringSplitOptions.TrimEntries);

        if (parts.Length == 2)
        {
            serviceId = string.IsNullOrWhiteSpace(parts[0]) ? _options.DefaultServiceId : parts[0];
            printerId = parts[1];
            return;
        }

        printerId = parts[0];
    }
}

