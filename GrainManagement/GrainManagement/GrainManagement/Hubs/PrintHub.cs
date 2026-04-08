#nullable enable
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace GrainManagement.Hubs
{
    public class PrintHub : Hub
    {
        // deviceId -> connectionId
        private static readonly ConcurrentDictionary<string, string> Devices = new();

        // connectionId -> serviceId
        private static readonly ConcurrentDictionary<string, string> PrintConnections = new();

        public Task Register(string deviceId)
        {
            Devices[deviceId] = Context.ConnectionId;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Backward-compatible alias used by kiosk.js.
        /// </summary>
        public Task RegisterPrinter(string printerId) => Register(printerId);

        /// <summary>
        /// Called by web print agents to join PrintClients + a service-specific group.
        /// </summary>
        public async Task JoinPrintGroup(string serviceId = "default")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "PrintClients");
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Print_{serviceId}");
            PrintConnections[Context.ConnectionId] = serviceId;
            await Clients.All.SendAsync("PrintServiceStatusChanged", GetConnectedPrintServiceIds());
        }

        public Task<bool> CheckPrintServiceConnected()
            => Task.FromResult(!PrintConnections.IsEmpty);

        public Task<List<string>> GetConnectedPrintServices()
            => Task.FromResult(GetConnectedPrintServiceIds());

        public Task PrintServiceReady(object announcement)
            => Clients.All.SendAsync("PrintServiceReady", announcement);

        public Task PrinterListResponse(object printers)
            => Clients.All.SendAsync("PrinterListReceived", printers);

        public Task RequestPrinterList()
            => Clients.Group("PrintClients").SendAsync("GetPrinterList");

        public Task PrintResult(object result)
            => Clients.All.SendAsync("PrintResult", result);

        public Task TestPrintResult(object result)
            => Clients.All.SendAsync("TestPrintResult", result);

        public Task TestPrint(string serviceId, string printerId)
            => Clients.Group($"Print_{serviceId}").SendAsync("TestPrint", printerId);

        /// <summary>
        /// Routes print commands to a specific print service, or all print clients.
        /// </summary>
        public Task PrintTicket(string? serviceId, object ticketData)
        {
            if (!string.IsNullOrWhiteSpace(serviceId))
                return Clients.Group($"Print_{serviceId}").SendAsync("PrintTicket", ticketData);

            return Clients.Group("PrintClients").SendAsync("PrintTicket", ticketData);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            PrintConnections.TryRemove(Context.ConnectionId, out _);

            foreach (var kv in Devices)
            {
                if (kv.Value == Context.ConnectionId)
                    Devices.TryRemove(kv.Key, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        internal static bool TryGetConnection(string deviceId, out string connectionId)
            => Devices.TryGetValue(deviceId, out connectionId!);

        private static List<string> GetConnectedPrintServiceIds()
            => PrintConnections.Values
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
                .ToList();
    }
}
