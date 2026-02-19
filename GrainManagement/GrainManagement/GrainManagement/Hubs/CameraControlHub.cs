using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace GrainManagement.Hubs;

public sealed class CameraControlHub : Hub
{
    // deviceId -> connectionId (Pi agent connections)
    private static readonly ConcurrentDictionary<string, string> DeviceConnections = new(StringComparer.OrdinalIgnoreCase);

    // cameraId -> viewer count
    private static readonly ConcurrentDictionary<string, int> ViewerCounts = new(StringComparer.OrdinalIgnoreCase);

    // cameraId -> stop timer cts
    private static readonly ConcurrentDictionary<string, CancellationTokenSource> StopTimers = new(StringComparer.OrdinalIgnoreCase);

    // IMPORTANT: replace this with your real mapping (cameraId -> deviceId).
    // For now, if all cameras are on one Pi, return that one deviceId.
    private static string MapCameraToDevice(string cameraId) => "pi-01";

    // ----- Pi agent registers itself -----
    public Task RegisterDevice(string deviceId)
    {
        DeviceConnections[deviceId] = Context.ConnectionId;
        return Task.CompletedTask;
    }

    // ----- Viewer presence -----
    public async Task JoinCamera(string cameraId)
    {
        // cancel pending stop
        if (StopTimers.TryRemove(cameraId, out var oldCts))
        {
            oldCts.Cancel();
            oldCts.Dispose();
        }

        var count = ViewerCounts.AddOrUpdate(cameraId, 1, (_, old) => old + 1);

        // first viewer => start publish on Pi
        if (count == 1)
        {
            var deviceId = MapCameraToDevice(cameraId);
            if (DeviceConnections.TryGetValue(deviceId, out var connId))
                await Clients.Client(connId).SendAsync("StartPublish", cameraId);
        }
    }

    public Task LeaveCamera(string cameraId)
    {
        var count = ViewerCounts.AddOrUpdate(cameraId, 0, (_, old) => Math.Max(0, old - 1));

        if (count == 0)
        {
            // delay stop to avoid tab flapping / reconnect jitter
            var cts = new CancellationTokenSource();
            StopTimers[cameraId] = cts;

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(45), cts.Token);
                    var deviceId = MapCameraToDevice(cameraId);

                    if (DeviceConnections.TryGetValue(deviceId, out var connId))
                        await Clients.Client(connId).SendAsync("StopPublish", cameraId);
                }
                catch (TaskCanceledException) { }
            });
        }

        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // If a Pi disconnects, remove it
        foreach (var kvp in DeviceConnections)
        {
            if (kvp.Value == Context.ConnectionId)
            {
                DeviceConnections.TryRemove(kvp.Key, out _);
                break;
            }
        }
        return base.OnDisconnectedAsync(exception);
    }
}
