#nullable enable
using System.Collections.Concurrent;
using GrainManagement.Dtos.Scales;

namespace GrainManagement.Services.ScaleReader
{
    /// <summary>
    /// Thread-safe in-memory store for the latest weight reading from each scale.
    /// The ScaleWorker writes here after each successful poll; consumers (SignalR push,
    /// REST endpoints, UI) read from here.
    /// </summary>
    public sealed class ScaleWeightStore
    {
        private readonly ConcurrentDictionary<int, ScaleDto> _store = new();

        /// <summary>
        /// Update (or insert) the latest reading for a scale.
        /// </summary>
        public void Update(ScaleDto dto)
        {
            if (dto == null) return;

            _store.AddOrUpdate(
                dto.Id,
                _ => dto,
                (_, __) => dto);
        }

        /// <summary>
        /// Get the latest reading for a specific scale, or null if never read.
        /// </summary>
        public ScaleDto? Get(int scaleId)
        {
            return _store.TryGetValue(scaleId, out var dto) ? dto : null;
        }

        /// <summary>
        /// Get a snapshot of all current readings.
        /// </summary>
        public IReadOnlyList<ScaleDto> GetAll()
        {
            return _store.Values.OrderBy(s => s.Id).ToList();
        }

        /// <summary>
        /// Remove a scale from the store (e.g. when a config is deleted).
        /// </summary>
        public bool Remove(int scaleId)
        {
            return _store.TryRemove(scaleId, out _);
        }

        /// <summary>
        /// Clear all stored readings.
        /// </summary>
        public void Clear()
        {
            _store.Clear();
        }
    }
}
