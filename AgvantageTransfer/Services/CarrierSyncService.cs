using Agvantage_Transfer.Diagnostics;
using Agvantage_Transfer.Logging;
using Agvantage_Transfer.NwModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agvantage_Transfer.Sync;

public sealed class CarrierSyncService : ICarrierSyncService
{
    private readonly NW_DataContext _nw;
    private readonly ITransferLogger _log;

    public CarrierSyncService(NW_DataContext nw, ITransferLogger log)
    {
        _nw = nw;
        _log = log;
    }

    private static string Norm(string s) =>
        new string((s ?? string.Empty).Trim().ToUpperInvariant()
            .Where(ch => !char.IsWhiteSpace(ch)).ToArray());


    public async Task UpsertAsync(IEnumerable<DTOModels.AgvantageCarrierDTO> dtoList)
    {
        // Local normalizer: trim, uppercase, remove whitespace so " ARMSTRONG  " equals "ARMSTRONG"
        static string Norm(string s) =>
            new string((s ?? string.Empty).Trim().ToUpperInvariant().Where(ch => !char.IsWhiteSpace(ch)).ToArray());

        // Normalize & guard (skip blank descriptions)
        var dtos = dtoList
            .Where(d => !string.IsNullOrWhiteSpace(d.Description))
            .Select(d => new { d.Id, Desc = d.Description.Trim(), d.Active, Key = Norm(d.Description) })
            .ToList();

        // DTO lookups
        var dtoIds = new HashSet<int>(dtos.Select(d => d.Id));
        var dtoNames = new HashSet<string>(dtos.Select(d => d.Key));
        var dtoPairs = new HashSet<(int Id, string Key)>(dtos.Select(d => (d.Id, d.Key)));
        var dtoByName = dtos.GroupBy(d => d.Key).ToDictionary(g => g.Key, g => g.First()); // name → 1 dto
        var dtoByPair = dtos.GroupBy(d => (d.Id, d.Key)).ToDictionary(g => g.Key, g => g.First());

        using var tx = await _nw.Database.BeginTransactionAsync();

        // Load current carriers
        var current = await _nw.Carriers.AsTracking().ToListAsync();
        var byId = current.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.ToList());
        var byName = current.GroupBy(c => Norm(c.Description ?? string.Empty)).ToDictionary(g => g.Key, g => g.ToList());

        int updated = 0, renumbered = 0, deleted = 0, inserted = 0, deactivated = 0;

        // ----------------------------------------------------------------
        // A) UPDATE on exact (Id, Description) match
        // ----------------------------------------------------------------
        foreach (var c in current)
        {
            var key = Norm(c.Description ?? string.Empty);
            if (dtoByPair.TryGetValue((c.Id, key), out var dto))
            {
                // keep description tidy (50 guard if your DB requires; remove if not needed)
                var newDesc = dto.Desc.Length > 50 ? dto.Desc[..50] : dto.Desc;
                bool changed = false;

                if (!string.Equals(c.Description ?? string.Empty, newDesc, StringComparison.Ordinal))
                {
                    c.Description = newDesc;
                    changed = true;
                }
                if (c.Active != dto.Active)
                {
                    c.Active = dto.Active;
                    changed = true;
                }
                if (changed) updated++;
            }
        }
        if (updated > 0) await _log.InfoAsync($"Updated (exact pair): {updated}", "Nw_Data - Carriers");
        await _nw.SaveChangesAsync();

        // ----------------------------------------------------------------
        // B) RENAMING/REN UMBERING: if Description matches a DTO but Id differs,
        //    set c.Id = dto.Id. If some *other* row already has that Id but a different
        //    Description, delete that conflicting row first.
        // ----------------------------------------------------------------
        // Build fresh name index (may have changed from A)
        current = await _nw.Carriers.AsTracking().ToListAsync();
        byId = current.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.ToList());
        byName = current.GroupBy(c => Norm(c.Description ?? string.Empty)).ToDictionary(g => g.Key, g => g.ToList());

        var planDeletes = new HashSet<Guid>(); // Uid keys to delete
        var planRenumbers = new List<(Agvantage_Transfer.NwModels.Carrier c, int newId, string newDesc, bool newActive)>();

        foreach (var kvp in byName)
        {
            var nameKey = kvp.Key;
            if (!dtoByName.TryGetValue(nameKey, out var dto)) continue; // no DTO with this name

            var rowsWithThisName = kvp.Value;
            foreach (var c in rowsWithThisName)
            {
                // If pair already matches, skip (handled in A)
                if (c.Id == dto.Id) continue;

                // Conflict row: someone already has target Id but different name → delete that other row first
                if (byId.TryGetValue(dto.Id, out var rowsWithTargetId))
                {
                    foreach (var other in rowsWithTargetId)
                    {
                        var otherKey = Norm(other.Description ?? string.Empty);
                        if (other.Uid != c.Uid && otherKey != nameKey)
                            planDeletes.Add(other.Uid); // stale mismatched record
                    }
                }

                planRenumbers.Add((c, dto.Id, dto.Desc.Length > 50 ? dto.Desc[..50] : dto.Desc, dto.Active));
            }
        }

        if (planDeletes.Count > 0)
        {
            var del = current.Where(c => planDeletes.Contains(c.Uid)).ToList();
            _nw.Carriers.RemoveRange(del);
            deleted += del.Count;
            await _log.WarnAsync($"Deleted conflicting rows prior to Id renumbering: {del.Count}", "Nw_Data - Carriers");
            await _nw.SaveChangesAsync();
        }

        foreach (var p in planRenumbers)
        {
            // Re-fetch tracked entity (in case of context changes)
            var c = current.First(x => x.Uid == p.c.Uid);
            c.Id = p.newId;
            c.Description = p.newDesc;
            c.Active = p.newActive;
            renumbered++;
        }
        if (renumbered > 0) await _log.InfoAsync($"Renumbered (same name, different Id → set Id from DTO): {renumbered}", "Nw_Data - Carriers");
        await _nw.SaveChangesAsync();

        // ----------------------------------------------------------------
        // C) DELETE remaining mismatches where Id OR Name exists in DTOs but the pair does not
        //    (now safe—renumbering done)
        // ----------------------------------------------------------------
        current = await _nw.Carriers.AsTracking().ToListAsync();
        var toDelete = current.Where(c =>
        {
            var key = Norm(c.Description ?? string.Empty);
            var pairIn = dtoPairs.Contains((c.Id, key));
            var idIn = dtoIds.Contains(c.Id);
            var nameIn = dtoNames.Contains(key);
            return !pairIn && (idIn || nameIn);
        }).ToList();

        if (toDelete.Count > 0)
        {
            _nw.Carriers.RemoveRange(toDelete);
            deleted += toDelete.Count;
            await _log.WarnAsync($"Deleted mismatches (Id or Name exists, pair missing): {toDelete.Count}", "Nw_Data - Carriers");
            await _nw.SaveChangesAsync();
        }

        // ----------------------------------------------------------------
        // D) INSERT missing DTO pairs
        // ----------------------------------------------------------------
        current = await _nw.Carriers.AsTracking().ToListAsync();
        var existingPairs = new HashSet<(int Id, string Key)>(current.Select(c => (c.Id, Norm(c.Description ?? string.Empty))));
        foreach (var dto in dtos)
        {
            var key = (dto.Id, dto.Key);
            if (!existingPairs.Contains(key))
            {
                _nw.Carriers.Add(new Agvantage_Transfer.NwModels.Carrier
                {
                    Uid = Guid.NewGuid(),
                    Id = dto.Id,
                    Description = dto.Desc.Length > 50 ? dto.Desc[..50] : dto.Desc,
                    Active = dto.Active
                });
                existingPairs.Add(key);
                inserted++;
            }
        }
        if (inserted > 0) await _log.InfoAsync($"Inserted: {inserted}", "Nw_Data - Carriers");
        await _nw.SaveChangesAsync();

        // ----------------------------------------------------------------
        // E) DEACTIVATE rows absent by both Id and Name
        // ----------------------------------------------------------------
        current = await _nw.Carriers.AsTracking().ToListAsync();
        foreach (var c in current)
        {
            var key = Norm(c.Description ?? string.Empty);
            if (!dtoIds.Contains(c.Id) && !dtoNames.Contains(key) && c.Active)
            {
                c.Active = false;
                deactivated++;
            }
        }
        if (deactivated > 0) await _log.WarnAsync($"Deactivated (absent by Id & Name): {deactivated}", "Nw_Data - Carriers");
        await _nw.SaveChangesAsync();

        await tx.CommitAsync();
    }





}
