using System.Collections.Concurrent;

namespace ScaleReaderService.Services;

/// <summary>
/// Thread-safe in-memory store of the latest weight reading per scale.
/// Written by ScaleWorker poll loop, read by the REST API.
/// </summary>
public class ScaleWeightStore
{
    private readonly ConcurrentDictionary<string, ScaleReading> _readings = new();

    public void Update(string scaleId, ScaleReading reading)
    {
        _readings[scaleId] = reading;
    }

    public ScaleReading? Get(string scaleId)
    {
        return _readings.TryGetValue(scaleId, out var r) ? r : null;
    }

    public IReadOnlyDictionary<string, ScaleReading> GetAll()
    {
        return _readings;
    }
}

public class ScaleReading
{
    public string ScaleId { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public int Weight { get; set; }
    public bool Motion { get; set; }
    public bool Ok { get; set; }
    public string Status { get; set; } = "Unknown";
    public string RawResponse { get; set; } = "";
    public string RawHex { get; set; } = "";
    public DateTime LastUpdate { get; set; }
}
