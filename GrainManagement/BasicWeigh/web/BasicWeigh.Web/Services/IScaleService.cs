using System.Collections.Concurrent;

namespace BasicWeigh.Web.Services;

public interface IScaleService
{
    int GetCurrentWeight();
    bool IsInMotion();
    bool IsConnected();
    bool HasError();
}

public class SimulatedScaleService : IScaleService
{
    private int _simulatedWeight = 0;
    private bool _motion = false;
    private bool _error = false;
    private bool _comError = false;

    /// <summary>
    /// Timestamp of the last weight update from an external scale (non-demo mode).
    /// Null until the first POST api/scale/weight call.
    /// </summary>
    public DateTime? LastUpdate { get; private set; }

    public int GetCurrentWeight() => _simulatedWeight;
    public bool IsInMotion() => _motion;
    public bool IsConnected() => !_error;
    public bool HasError() => _error;

    public bool HasComError() => _comError;

    public void SetWeight(int weight) => _simulatedWeight = weight;
    public void SetMotion(bool motion) => _motion = motion;
    public void SetError(bool error) => _error = error;
    public void SetComError(bool comError) => _comError = comError;

    /// <summary>
    /// Mark that the scale just sent a fresh reading (resets the 5-second timeout).
    /// </summary>
    public void Touch() => LastUpdate = DateTime.UtcNow;
}

/// <summary>
/// Thread-safe multi-scale weight store. Tracks per-scale readings with timeout detection.
/// Scales that haven't updated in 5 seconds are flagged as com error.
/// </summary>
public class ScaleWeightStore
{
    private static readonly TimeSpan ComTimeout = TimeSpan.FromSeconds(5);

    private readonly ConcurrentDictionary<string, ScaleReading> _readings = new();

    /// <summary>
    /// Update or add a scale reading. Called by POST api/scale/weight or SignalR ScaleWeight.
    /// </summary>
    public void Update(string scaleId, string serviceId, int weight, bool motion, bool ok, string? status = null)
    {
        var key = $"{serviceId}:{scaleId}";
        _readings[key] = new ScaleReading
        {
            ScaleId = scaleId,
            ServiceId = serviceId,
            Weight = weight,
            Motion = motion,
            Ok = ok,
            Status = status ?? (ok ? (motion ? "Motion" : "Ok") : "Error"),
            LastUpdate = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Get the reading for a specific scale. Returns com error if stale.
    /// </summary>
    public ScaleReading? Get(string fullId)
    {
        if (_readings.TryGetValue(fullId, out var reading))
        {
            return CheckTimeout(reading);
        }
        // Try matching by scaleId only
        foreach (var kvp in _readings)
        {
            if (kvp.Key.EndsWith(":" + fullId))
                return CheckTimeout(kvp.Value);
        }
        return null;
    }

    /// <summary>
    /// Get all scale readings, with timeout checks applied.
    /// </summary>
    public IReadOnlyList<ScaleReading> GetAll()
    {
        return _readings.Values.Select(CheckTimeout).ToList();
    }

    private static ScaleReading CheckTimeout(ScaleReading reading)
    {
        if (DateTime.UtcNow - reading.LastUpdate > ComTimeout)
        {
            return new ScaleReading
            {
                ScaleId = reading.ScaleId,
                ServiceId = reading.ServiceId,
                Weight = 0,
                Motion = false,
                Ok = false,
                ComError = true,
                Status = "COM Error",
                LastUpdate = reading.LastUpdate
            };
        }
        return reading;
    }

    public class ScaleReading
    {
        public string ScaleId { get; set; } = "";
        public string ServiceId { get; set; } = "";
        public int Weight { get; set; }
        public bool Motion { get; set; }
        public bool Ok { get; set; }
        public bool ComError { get; set; }
        public string Status { get; set; } = "Unknown";
        public DateTime LastUpdate { get; set; }
    }
}
