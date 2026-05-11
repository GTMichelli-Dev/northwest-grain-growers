using System.Diagnostics;
using GrainManagement.Dtos.TempTickets;
using GrainManagement.Models;
using GrainManagement.Services.Camera;
using GrainManagement.Services.Print;

namespace GrainManagement.Services.TempTickets;

/// <summary>
/// Background-task implementation of <see cref="ITempTicketOrchestrator"/>.
/// Lives as a singleton so the press handler can fire-and-forget without
/// keeping the kiosk's HTTP request open while we wait for the scale to
/// stop moving.
/// </summary>
public sealed class TempTicketOrchestrator : ITempTicketOrchestrator
{
    // Polling cadence + ceiling for the no-motion wait. 100 ms is fast
    // enough to feel instant on the kiosk display; 8 s is generous —
    // a producer who can't stop moving after that long isn't going to.
    private static readonly TimeSpan PollInterval   = TimeSpan.FromMilliseconds(100);
    private static readonly TimeSpan StableWindow   = TimeSpan.FromMilliseconds(400);
    private static readonly TimeSpan MaxWait        = TimeSpan.FromSeconds(8);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IScaleRegistry _scales;
    private readonly ILogger<TempTicketOrchestrator> _log;

    public TempTicketOrchestrator(
        IServiceScopeFactory scopeFactory,
        IScaleRegistry scales,
        ILogger<TempTicketOrchestrator> log)
    {
        _scopeFactory = scopeFactory;
        _scales = scales;
        _log = log;
    }

    public TempTicketPressResponse Enqueue(TempTicketPressRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.KioskId) || request.ScaleId <= 0)
        {
            return new TempTicketPressResponse
            {
                Status = "rejected",
                Message = "KioskId and ScaleId are required."
            };
        }

        // Snapshot the request — the calling controller is scoped and may be
        // disposed by the time the background task runs.
        var captured = new TempTicketPressRequest
        {
            KioskId = request.KioskId,
            ScaleId = request.ScaleId,
            PrinterTarget = request.PrinterTarget,
        };

        _ = Task.Run(() => RunAsync(captured));

        return new TempTicketPressResponse { Status = "queued" };
    }

    private async Task RunAsync(TempTicketPressRequest req)
    {
        try
        {
            // 1) Wait for the scale to settle.
            var snapshot = await WaitForStableWeightAsync(req.ScaleId);
            if (snapshot is null)
            {
                _log.LogWarning("Temp ticket press from {Kiosk} on scale {ScaleId} timed out waiting for stable weight.",
                    req.KioskId, req.ScaleId);
                return;
            }

            // 2) Store the row + 3) fire side effects (print + camera).
            // The scope's lifetime is bounded by this background task,
            // not the original HTTP request.
            using var scope = _scopeFactory.CreateScope();
            var db          = scope.ServiceProvider.GetRequiredService<dbContext>();
            var serverInfo  = scope.ServiceProvider.GetRequiredService<IServerInfoProvider>();
            var print       = scope.ServiceProvider.GetRequiredService<IPrintDispatchService>();
            var cameraFire  = scope.ServiceProvider.GetRequiredService<ICameraCaptureTrigger>();

            var server = await serverInfo.GetAsync();
            if (server is null)
            {
                _log.LogWarning("Temp ticket press: no system.Servers row matches this host — skipping.");
                return;
            }

            var row = new TempWeightTicket
            {
                ServerId = server.ServerId,
                ScaleId  = req.ScaleId,
                KioskId  = req.KioskId,
                Gross    = snapshot.Value.Gross,
                Tare     = snapshot.Value.Tare,
                Net      = snapshot.Value.Net,
                Units    = snapshot.Value.Units ?? "lbs",
                CreatedAt = DateTime.UtcNow,
            };

            db.Set<TempWeightTicket>().Add(row);
            await db.SaveChangesAsync();

            _log.LogInformation(
                "Temp ticket {Id} stored from {Kiosk} on scale {ScaleId}: gross={Gross} net={Net}.",
                row.TempTicketId, req.KioskId, req.ScaleId, row.Gross, row.Net);

            // 4) Fire the print. Same target string as the rest of the print
            // dispatch system — "serviceId:printerId" or just "printerId".
            if (!string.IsNullOrWhiteSpace(req.PrinterTarget))
            {
                try
                {
                    await print.DispatchTicketAsync(
                        ticketId: row.TempTicketId.ToString(),
                        printerTarget: req.PrinterTarget,
                        type: "tempticket");
                }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "Temp ticket {Id}: print dispatch failed.", row.TempTicketId);
                }
            }

            // 5) Fire camera capture (Phase 2 wires the multi-camera composite).
            // The CameraCaptureTrigger maps direction → role; "tmp" → "TempTicket".
            try
            {
                await cameraFire.FireAsync(
                    loadNumber: row.TempTicketId.ToString(),
                    direction:  "tmp",
                    locationId: null,
                    scaleId:    req.ScaleId);
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "Temp ticket {Id}: camera capture trigger failed.", row.TempTicketId);
            }

            // Mark complete so the management view can distinguish in-flight
            // rows (rare) from finalized ones.
            row.CompletedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Temp ticket press from {Kiosk} on scale {ScaleId} failed.",
                req.KioskId, req.ScaleId);
        }
    }

    /// <summary>
    /// Polls <see cref="IScaleRegistry"/> until the named scale reports
    /// Motion = false for <see cref="StableWindow"/> consecutive samples.
    /// Returns null if the wait exceeds <see cref="MaxWait"/>.
    /// </summary>
    private async Task<WeightSnapshot?> WaitForStableWeightAsync(int scaleId)
    {
        var deadline = Stopwatch.StartNew();
        DateTime? motionClearedAt = null;
        WeightSnapshot? lastStable = null;

        while (deadline.Elapsed < MaxWait)
        {
            var scale = _scales.GetSnapshotWithHealth(TimeSpan.FromSeconds(5))
                                .FirstOrDefault(s => s.Id == scaleId);

            if (scale is null || !scale.Ok)
            {
                await Task.Delay(PollInterval);
                continue;
            }

            if (scale.Motion)
            {
                motionClearedAt = null;
                lastStable = null;
                await Task.Delay(PollInterval);
                continue;
            }

            // Motion bit is clear. Start (or continue) the stability window.
            var nowSnap = new WeightSnapshot
            {
                Gross = scale.Weight,
                Tare  = null,
                Net   = scale.Weight,
                Units = "lbs",
            };

            motionClearedAt ??= DateTime.UtcNow;
            lastStable = nowSnap;

            if (DateTime.UtcNow - motionClearedAt.Value >= StableWindow)
                return lastStable;

            await Task.Delay(PollInterval);
        }

        return null;
    }

    private readonly struct WeightSnapshot
    {
        public decimal Gross { get; init; }
        public decimal? Tare { get; init; }
        public decimal? Net { get; init; }
        public string? Units { get; init; }
    }
}
