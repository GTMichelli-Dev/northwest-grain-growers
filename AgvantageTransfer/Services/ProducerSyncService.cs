
using Agvantage_Transfer.Logging;
using Agvantage_Transfer.NwModels;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace Agvantage_Transfer.Sync;

public sealed class ProducerSyncService : IProducerSyncService
{
    private readonly NW_DataContext _nw;
    private readonly ITransferLogger _log;
   
    public ProducerSyncService(NW_DataContext nw, ITransferLogger log)
    {
        
        _nw = nw;
        _log = log;
    }

    private static string Norm(string s) =>
        new string((s ?? string.Empty).Trim().ToUpperInvariant()
            .Where(ch => !char.IsWhiteSpace(ch)).ToArray());

    public async Task UpsertAsync(IEnumerable<DTOModels.AgvantageProducerDTO> dtoList)
    {
        try
        {
            _nw.ChangeTracker.Clear();

            // Load current Producers
            var producers = await _nw.Producers.AsTracking().ToListAsync();
            var byId = producers.GroupBy(p => p.Id).ToDictionary(g => g.Key, g => g.ToList());
            var byName = producers.GroupBy(p => Norm(p.Description ?? string.Empty))
                                  .ToDictionary(g => g.Key, g => g.ToList());

            // Normalize incoming + guard
            var incoming = (dtoList ?? Enumerable.Empty<DTOModels.AgvantageProducerDTO>())
                .Where(d => !string.IsNullOrWhiteSpace(d.Description))
                .Select(d => new
                {
                    Raw = d,
                    Id = d.Id,
                    Desc = d.Description.Trim(),
                    Key = Norm(d.Description),
                    d.Active,
                    d.EmailAddress,
                    d.EmailWs,
                    d.PrintWs,
                    d.CompanyName,
                    d.CustomerName1,
                    d.CustomerName2,
                    d.Address1,
                    d.Address2,
                    d.City,
                    d.State,
                    d.Zip1,
                    d.Zip2,
                    d.HomePhone,
                    d.WorkPhone,
                    d.MobilePhone,
                    d.Phone,
                    d.Member
                })
                .ToList();

            var seenIds = new HashSet<int>();
            var seenNames = new HashSet<string>();
            var accepted = new List<dynamic>();

            int dupId = 0, dupName = 0, idMissing = 0, nameIdClash = 0;

            foreach (var x in incoming)
            {
                // Rule 1: duplicate ID in DTO -> exclude + warn
                if (!seenIds.Add(x.Id))
                {
                    dupId++;
                    await _log.WarnAsync($"Producer DTO duplicate Id {x.Id} — excluding '{x.Desc}'.", "NW_Data - Producers");
                    continue;
                }

                // Rule 1: duplicate Description in DTO (normalized) -> exclude + warn
                if (!seenNames.Add(x.Key))
                {
                    dupName++;
                    await _log.WarnAsync($"Producer DTO duplicate Description '{x.Desc}' — excluding Id {x.Id}.", "NW_Data - Producers");
                    continue;
                }

                // Rule 2: DTO.Id does not exist in Producers -> mark inactive + exclude + warn
                if (!byId.ContainsKey(x.Id))
                {
                    idMissing++;
                    // mark inactive on the DTO instance (local)
                    x.Raw.Active = false;
                    await _log.WarnAsync($"Producer DTO Id {x.Id} not found in Producers — marking inactive and excluding '{x.Desc}'.", "NW_Data - Producers");
                    continue;
                }

                // Rule 3: same Description exists in Producers but bound to a different Id -> exclude + warn
                if (byName.TryGetValue(x.Key, out var prodsWithName) && prodsWithName.Any(p => p.Id != x.Id))
                {
                    nameIdClash++;
                    var otherIds = string.Join(", ", prodsWithName.Select(p => p.Id).Distinct().OrderBy(v => v));
                    await _log.WarnAsync($"Description '{x.Desc}' already used by Producer Id(s) [{otherIds}] — excluding DTO Id {x.Id}.", "NW_Data - Producers");
                    continue;
                }

                // Accept this DTO for update-by-Id
                accepted.Add(x);
            }

            int updated = 0;

            // Update only existing Producer rows matched by Id
            foreach (var x in accepted)
            {
                // Multiple DB rows with same Id is possible; update the first and warn about extras
                var rows = byId[x.Id];
                if (rows.Count > 1)
                {
                    await _log.WarnAsync($"Multiple Producer rows found for Id {x.Id} — updating the first, please dedupe DB.", "NW_Data - Producers");
                }

                var p = rows[0];
                bool changed = false;

                // Keep Description aligned to DTO unless it would violate Rule 3 (guarded above)
                if (!string.Equals(p.Description, x.Desc, StringComparison.Ordinal))
                { p.Description = x.Desc; changed = true; }

                if (p.Active != x.Active) { p.Active = x.Active; changed = true; }

                if (!string.Equals(p.EmailAddress, x.EmailAddress, StringComparison.OrdinalIgnoreCase))
                { p.EmailAddress = x.EmailAddress; changed = true; }

                if (p.EmailWs != x.EmailWs) { p.EmailWs = x.EmailWs; changed = true; }
                if (p.PrintWs != x.PrintWs) { p.PrintWs = x.PrintWs; changed = true; }

                if (!string.Equals(p.CompanyName, x.CompanyName, StringComparison.Ordinal))
                { p.CompanyName = x.CompanyName; changed = true; }

                if (!string.Equals(p.CustomerName1, x.CustomerName1, StringComparison.Ordinal))
                { p.CustomerName1 = x.CustomerName1; changed = true; }

                if (!string.Equals(p.CustomerName2, x.CustomerName2, StringComparison.Ordinal))
                { p.CustomerName2 = x.CustomerName2; changed = true; }

                if (!string.Equals(p.Address1, x.Address1, StringComparison.Ordinal))
                { p.Address1 = x.Address1; changed = true; }

                if (!string.Equals(p.Address2, x.Address2, StringComparison.Ordinal))
                { p.Address2 = x.Address2; changed = true; }

                if (!string.Equals(p.City, x.City, StringComparison.Ordinal))
                { p.City = x.City; changed = true; }

                if (!string.Equals(p.State, x.State, StringComparison.Ordinal))
                { p.State = x.State; changed = true; }

                if (!string.Equals(p.Zip1, x.Zip1, StringComparison.Ordinal))
                { p.Zip1 = x.Zip1; changed = true; }

                if (!string.Equals(p.Zip2, x.Zip2, StringComparison.Ordinal))
                { p.Zip2 = x.Zip2; changed = true; }

                if (!string.Equals(p.HomePhone, x.HomePhone, StringComparison.Ordinal))
                { p.HomePhone = x.HomePhone; changed = true; }

                if (!string.Equals(p.WorkPhone, x.WorkPhone, StringComparison.Ordinal))
                { p.WorkPhone = x.WorkPhone; changed = true; }

                if (!string.Equals(p.MobilePhone, x.MobilePhone, StringComparison.Ordinal))
                { p.MobilePhone = x.MobilePhone; changed = true; }

                if (!string.Equals(p.Phone, x.Phone, StringComparison.Ordinal))
                { p.Phone = x.Phone; changed = true; }

                if (!string.Equals(p.Member, x.Member, StringComparison.Ordinal))
                { p.Member = x.Member; changed = true; }

                if (changed) updated++;
            }

           
                await _log.InfoAsync(
                    $"Producers Update — skipped due to rules: duplicateId={dupId}, duplicateDesc={dupName}, idMissingInDB={idMissing}, descIdClash={nameIdClash}.", "NW_Data - Producers");
           
            await _nw.SaveChangesAsync();
            _nw.ChangeTracker.Clear();
        }
        catch (Exception ex)
        {
            await _log.WarnAsync($"An error occurred in UpsertAsync: {TransferLogger.UsefulMessage(ex)}", "NW_Data - Producers");
        }
    }

}
