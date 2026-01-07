using Agvantage_Transfer.Logging;
using Agvantage_Transfer.NwModels;
using Microsoft.EntityFrameworkCore;
namespace Agvantage_Transfer.Sync;

internal class CropSyncService : ICropSyncService
{

        private readonly NW_DataContext _nw;
        private readonly ITransferLogger _log;

        public CropSyncService(NW_DataContext nw, ITransferLogger log)
        {

            _nw = nw;
            _log = log;
        }

        private static string Norm(string s) =>
            new string((s ?? string.Empty).Trim().ToUpperInvariant()
                .Where(ch => !char.IsWhiteSpace(ch)).ToArray());



        public async Task UpsertAsync(IEnumerable<DTOModels.AgvantageCropDTO> dtoList)
        {
            try
            {
                _nw.ChangeTracker.Clear();

                // Load current Crops
                var crops = await _nw.Crops.AsTracking().ToListAsync();

                // Index by Id and by normalized Description (name)
                var byId = crops.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.ToList());
                var byName = crops.GroupBy(c => Norm(c.Description ?? string.Empty))
                                  .ToDictionary(g => g.Key, g => g.ToList());

                // Normalize/guard incoming
                var incoming = (dtoList ?? Enumerable.Empty<DTOModels.AgvantageCropDTO>())
                    .Where(d => !string.IsNullOrWhiteSpace(d.Description))
                    .Select(d => new
                    {
                        Raw = d,
                        Id = d.Id,
                        Desc = d.Description.Trim(),
                        Key = Norm(d.Description),
                        d.UseAtElevator,
                        d.UseAtSeedMill,
                        d.UnitOfMeasure,
                        d.Active,
                        d.PoundPerBushel,
                        d.ColorIndex,
                        d.SecondaryColorIndex
                    })
                    .ToList();

                var seenIds = new HashSet<int>();
                var seenNames = new HashSet<string>();
                var accepted = new List<dynamic>();

                int dupId = 0, dupName = 0, idMissing = 0, nameIdClash = 0;

                foreach (var x in incoming)
                {
                    // Rule 1a: duplicate Id in DTOs => exclude + warn
                    if (!seenIds.Add(x.Id))
                    {
                        dupId++;
                        await _log.WarnAsync($"Crop DTO duplicate Id {x.Id} — excluding '{x.Desc}'.", "NW_Data - Crops");
                        continue;
                    }

                    // Rule 1b: duplicate Description in DTOs => exclude + warn
                    if (!seenNames.Add(x.Key))
                    {
                        dupName++;
                        await _log.WarnAsync($"Crop DTO duplicate Description '{x.Desc}' — excluding Id {x.Id}.", "NW_Data - Crops");
                        continue;
                    }

                    // Rule 2: DTO.Id not found in DB => mark inactive on DTO, exclude + warn
                    if (!byId.ContainsKey(x.Id))
                    {
                        idMissing++;
                        x.Raw.Active = false; // mark inactive as requested
                        await _log.WarnAsync($"Crop DTO Id {x.Id} not found in Crops — marking inactive and excluding '{x.Desc}'.", "NW_Data - Crops");
                        continue;
                    }

                    // Rule 3: Description used by a different Id in DB => exclude + warn
                    if (byName.TryGetValue(x.Key, out var cropsWithName) && cropsWithName.Any(c => c.Id != x.Id))
                    {
                        nameIdClash++;
                        var otherIds = string.Join(", ", cropsWithName.Select(c => c.Id).Distinct().OrderBy(v => v));
                        await _log.WarnAsync($"Description '{x.Desc}' already used by Crop Id(s) [{otherIds}] — excluding DTO Id {x.Id}.", "NW_Data - Crops");
                        continue;
                    }

                    accepted.Add(x);
                }

                int updated = 0;

                // Update only rows matched by Id
                foreach (var x in accepted)
                {
                    var rows = byId[x.Id];
                    if (rows.Count > 1)
                    {
                        await _log.WarnAsync($"Multiple Crop rows found for Id {x.Id} — updating the first; please dedupe the DB.", "NW_Data - Crops");
                    }

                    var c = rows[0];
                    bool changed = false;

                    if (!string.Equals(c.Description, x.Desc, StringComparison.Ordinal))
                    { c.Description = x.Desc; changed = true; }

                    if (c.UseAtElevator != x.UseAtElevator)
                    { c.UseAtElevator = x.UseAtElevator; changed = true; }

                    if (c.UseAtSeedMill != x.UseAtSeedMill)
                    { c.UseAtSeedMill = x.UseAtSeedMill; changed = true; }

                    if (!string.Equals(c.UnitOfMeasure, x.UnitOfMeasure, StringComparison.Ordinal))
                    { c.UnitOfMeasure = x.UnitOfMeasure; changed = true; }

                    if (c.Active != x.Active)
                    { c.Active = x.Active; changed = true; }

                    if (c.PoundPerBushel != x.PoundPerBushel)
                    { c.PoundPerBushel = x.PoundPerBushel; changed = true; }

                    if (c.ColorIndex != x.ColorIndex)
                    { c.ColorIndex = x.ColorIndex; changed = true; }

                    if (c.SecondaryColorIndex != x.SecondaryColorIndex)
                    { c.SecondaryColorIndex = x.SecondaryColorIndex; changed = true; }

                    if (changed) updated++;
                }

                    await _log.InfoAsync($"Crops Updated — skipped due to rules: duplicateId={dupId}, duplicateDesc={dupName}, idMissingInDB={idMissing}, descIdClash={nameIdClash}.", "NW_Data - Crops");

                await _nw.SaveChangesAsync();
                _nw.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                await _log.WarnAsync($"An error occurred in Crop UpsertAsync: {TransferLogger.UsefulMessage(ex)}", "NW_Data - Crops");
            }

            // Local helper mirrors Producer logic
            static string Norm(string s) => string.IsNullOrWhiteSpace(s)
                ? string.Empty
                : string.Join(' ', s.Trim().ToUpperInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }


    }

