using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace GrainManagement.Hubs
{
    public class PrintHub : Hub
    {
        // deviceId -> connectionId
        private static readonly ConcurrentDictionary<string, string> Devices = new();

        public Task Register(string deviceId)
        {
            Devices[deviceId] = Context.ConnectionId;
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            foreach (var kv in Devices)
            {
                if (kv.Value == Context.ConnectionId)
                    Devices.TryRemove(kv.Key, out _);
            }
            return base.OnDisconnectedAsync(exception);
        }

        internal static bool TryGetConnection(string deviceId, out string connectionId)
            => Devices.TryGetValue(deviceId, out connectionId!);
    }
}
