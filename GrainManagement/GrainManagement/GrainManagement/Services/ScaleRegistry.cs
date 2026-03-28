#nullable enable
using System.Collections.Concurrent;
using GrainManagement.Dtos.Scales;

namespace GrainManagement.Services
{
    public interface IScaleRegistry
    {
        void Upsert(ScaleDto scale);
        IReadOnlyList<ScaleDto> GetSnapshotWithHealth(TimeSpan staleAfter);
    }

    public sealed class ScaleRegistry : IScaleRegistry
    {
        // Key by Id (your requirement)
        private readonly ConcurrentDictionary<int, ScaleDto> _scales = new();

        // Track descriptions to prevent duplicates (case-insensitive)
        private readonly ConcurrentDictionary<string, int> _descToId =
            new(StringComparer.OrdinalIgnoreCase);

        // Coordinate updates that touch both dictionaries (Id map + description map)
        private readonly object _gate = new();

        private static string Norm(string? s) => (s ?? "").Trim();

        public void Upsert(ScaleDto scale)
        {
            if (scale == null)
                throw new ArgumentException("Scale cannot be null.");

            if (scale.Id < 1)
                throw new ArgumentException("Scale Id must be >= 1.");

            var desc = Norm(scale.Description);
            if (string.IsNullOrWhiteSpace(desc))
                throw new ArgumentException("Scale Description is required.");

            var now = DateTime.UtcNow;

            lock (_gate)
            {
                // Enforce "no dup Descriptions"
                if (_descToId.TryGetValue(desc, out var existingId) && existingId != scale.Id)
                    throw new InvalidOperationException($"Duplicate Description: '{desc}' is already used by Id {existingId}.");

                // If this Id already exists and description changed, free the old description
                if (_scales.TryGetValue(scale.Id, out var existing))
                {
                    var oldDesc = Norm(existing.Description);

                    // If changing description, ensure the new description isn't taken by another Id
                    if (!string.Equals(oldDesc, desc, StringComparison.OrdinalIgnoreCase))
                    {
                        if (_descToId.TryGetValue(desc, out var otherId) && otherId != scale.Id)
                            throw new InvalidOperationException($"Duplicate Description: '{desc}' is already used by Id {otherId}.");

                        if (!string.IsNullOrWhiteSpace(oldDesc))
                            _descToId.TryRemove(oldDesc, out _);
                    }

                    // Update existing in-place
                    existing.Description = desc;
                    existing.Motion = scale.Motion;
                    existing.Ok = scale.Ok;
                    existing.Weight = scale.Weight;
                    existing.Status = scale.Status;
                    existing.LastUpdate = now;

                    // Ensure mapping points to this Id
                    _descToId[desc] = scale.Id;
                    return;
                }

                // New scale for this Id
                scale.Description = desc;
                scale.LastUpdate = now;

                _scales[scale.Id] = scale;
                _descToId[desc] = scale.Id;
            }
        }

        public IReadOnlyList<ScaleDto> GetSnapshotWithHealth(TimeSpan staleAfter)
        {
            var now = DateTime.UtcNow;

            var list = _scales.Values
        .Select(s =>
        {
            var dto = new ScaleDto
            {
                Id = s.Id,
                Description = s.Description,
                Weight = s.Weight,
                Ok = s.Ok,
                Motion = s.Motion,
                Status = s.Status,
                LastUpdate = s.LastUpdate
            };

            if (now - dto.LastUpdate > staleAfter)
            {
                dto.Ok = false;
                dto.Status = "No connection";
                dto.Weight = 0;
                dto.Motion = false;
            }

            return dto;
        })
        .OrderBy(s => s.Id)
        .ToList();


            return list;
        }
    }
}
