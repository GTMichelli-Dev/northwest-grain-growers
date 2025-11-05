using System.Collections.Generic;
using System.Threading.Tasks;
using Agvantage_Transfer.DTOModels;

using System.Linq;
using Microsoft.EntityFrameworkCore;

using Agvantage_Transfer.SeedModels;     // Item + Seed DbContext
using Agvantage_Transfer.Logging;        // ITransferLogger


namespace Agvantage_Transfer.Sync
{
    public class SeedSyncService : ISeedSyncService
    {
        private readonly Seed_DataContext _seed;
        private readonly ITransferLogger _log;

        public SeedSyncService(Seed_DataContext seed, ITransferLogger log)
        {
            _seed = seed;
            _log = log;
        }

        public async Task UpsertSeedItemsAsync(IEnumerable<AgvantageItemDTO> dtoList)
        {
            try
            {
                _seed.ChangeTracker.Clear();

                // Current Items in DB
                var items = await _seed.Items.AsTracking().ToListAsync();

                // Index by Id and by normalized Description
                static string Norm(string s) =>
                    new string((s ?? string.Empty).Trim().ToUpperInvariant()
                        .Where(ch => !char.IsWhiteSpace(ch)).ToArray());

                var byId = items.GroupBy(i => i.Id).ToDictionary(g => g.Key, g => g.ToList());
                var byName = items.GroupBy(i => Norm(i.Description))
                                  .ToDictionary(g => g.Key, g => g.ToList());

                // Normalize incoming DTOs (ignore null/blank descriptions)
                var incoming = (dtoList ?? Enumerable.Empty<AgvantageItemDTO>())
                    .Where(d => d is not null && !string.IsNullOrWhiteSpace(d.Description))
                    .Select(d => new
                    {
                        Raw = d,
                        Id = d.Id,
                        Desc = d.Description.Trim(),
                        Key = Norm(d.Description),

                        // carry forward fields for updates
                        d.Dept,
                        d.StoreLocation,
                        d.ItemType,
                        d.Flc,
                        d.Uomcode,
                        d.Inactive,
                        d.NotInUse,
                        d.Comment
                    })
                    .ToList();

                // Rule enforcement + acceptance filter
                var seenIds = new HashSet<int>();
                var seenNames = new HashSet<string>();
                var accepted = new List<dynamic>();

                int dupId = 0, dupDesc = 0, idMissing = 0, descIdClash = 0;
                var dtoItemIds = new HashSet<int>((dtoList ?? Enumerable.Empty<AgvantageItemDTO>()).Select(d => d.Id));

                foreach (var dbItem in items)
                {
                    if (!dtoItemIds.Contains(dbItem.Id))
                    {
                        if (!dbItem.Inactive)
                        {
                            dbItem.Inactive = true;
                            await _log.WarnAsync($"Item {dbItem.Id} not present in DTO feed — marking Inactive=true.","Seed - Items");
                        }
                        // Optionally: detach from any “active” lists in memory, etc.
                    }
                }

                foreach (var x in incoming)
                {
                    // (1) Duplicate ID inside DTOs ⇒ exclude + warn
                    if (!seenIds.Add(x.Id))
                    {
                        dupId++;
                        await _log.WarnAsync($"Item DTO duplicate Id {x.Id} — excluding '{x.Desc}'.","Seed Master");
                        continue;
                    }

                    // (1) Duplicate Description inside DTOs ⇒ exclude + warn
                    if (!seenNames.Add(x.Key))
                    {
                        dupDesc++;
                        await _log.WarnAsync($"Item DTO duplicate Description '{x.Desc}' — excluding Id {x.Id}.","Seed Master");
                        continue;
                    }



                    // (3) Same Description exists in Items but tied to a different Id ⇒ exclude + warn
                    if (byName.TryGetValue(x.Key, out var itemsWithName) && itemsWithName.Any(i => i.Id != x.Id))
                    {
                        descIdClash++;
                        var otherIds = string.Join(", ", itemsWithName.Select(i => i.Id).Distinct().OrderBy(v => v));
                        await _log.WarnAsync($"Description '{x.Desc}' already used by Item Id(s) [{otherIds}] — excluding DTO Id {x.Id}.","Seed - Items");
                        continue;
                    }

                    accepted.Add(x);
                }

                // Apply updates to matched Items by Id
                int updated = 0;

                foreach (var x in accepted)
                {
                    if (!byId.TryGetValue(x.Id, out List<Item> rows))
                    {
                        // Add new item to the database
                        var newItem = new Item
                        {
                            Id = x.Id,
                            Description = x.Desc,
                            Dept = x.Dept,
                            StoreLocation = x.StoreLocation,
                            ItemType = SeedItemType(x.Flc),
                            Flc = x.Flc,
                            Uomcode = x.Uomcode,
                           
                            Inactive = x.Inactive,
                            NotInUse = x.NotInUse,
                            Comment = x.Comment
                        };

                        _seed.Items.Add(newItem);
                        await _log.WarnAsync($"Item — inserted: , Item Id={x.Id}, Description={x.Desc}, Type={SeedItemType(x.Flc)}.", "Seed - ItemLocation");

                        
                        continue;
                    }

                    if (rows.Count > 1)
                        await _log.WarnAsync($"Multiple Item rows found for Id {x.Id} — updating the first; please dedupe the DB.","Seed - Items");

                    var e = rows[0];
                    bool changed = false;

                    if (!string.Equals(e.Description, x.Desc, StringComparison.Ordinal)) { e.Description = x.Desc; changed = true; }
                    if (e.Dept != x.Dept) { e.Dept = x.Dept; changed = true; }
                    if (e.StoreLocation != x.StoreLocation) { e.StoreLocation = x.StoreLocation; changed = true; }
                    if (!string.Equals(e.ItemType, x.ItemType, StringComparison.Ordinal)) { e.ItemType = x.ItemType; changed = true; }
                    if (e.Flc != x.Flc) { e.Flc = x.Flc; changed = true; }
                    if (!string.Equals(e.Uomcode, x.Uomcode, StringComparison.Ordinal)) { e.Uomcode = x.Uomcode; changed = true; }
                    if (e.Inactive != x.Inactive) { e.Inactive = x.Inactive; changed = true; }
                    if (e.NotInUse != x.NotInUse) { e.NotInUse = x.NotInUse; changed = true; }
                    if (!string.Equals(e.Comment, x.Comment, StringComparison.Ordinal)) { e.Comment = x.Comment; changed = true; }

                    if (changed) updated++;

                }

               // if (dupId > 0 || dupDesc > 0 || idMissing > 0 || descIdClash > 0)
                    await _log.InfoAsync($"Items Updated — skipped due to rules: duplicateId={dupId}, duplicateDesc={dupDesc}, idMissingInDB={idMissing}, descIdClash={descIdClash}.","Seed - Items");

               // if (updated > 0)
               //     await _log.InfoAsync($"Items updated: {updated}","Seed - Items");

                await _seed.SaveChangesAsync();
                _seed.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                await _log.WarnAsync($"An error occurred in Seed Items Upsert: {TransferLogger.UsefulMessage(ex)}","Seed - Items");
            }
        }




        public string SeedItemType(int FLC)
        {
            string Retval = "Other";
            if (FLC == 1 || FLC == 2 || FLC == 3 || FLC == 4 || FLC == 310 || FLC == 311)
            {
                Retval = "Seed";
            }
            else if (FLC == 280)
            {
                Retval = "Chemical";
            }
            return Retval;
        }

        public async Task UpsertSeedItemLocationAsync(IEnumerable<AgvantageItemLocationDTO> dtoList)
        {
            try
            {
                _seed.ChangeTracker.Clear();

                // Load DB state
                var items = await _seed.Items.AsNoTracking().ToListAsync();                 // Parent items
                var itemLocations = await _seed.ItemLocations.AsTracking().ToListAsync();   // Existing ItemLocation rows

                // Quick lookups
                var itemsById = items.ToDictionary(i => i.Id, i => i);
                static string Norm(string s) =>
                    new string((s ?? string.Empty).Trim().ToUpperInvariant()
                        .Where(ch => !char.IsWhiteSpace(ch)).ToArray());

                // For rule (3)-analogue: at each Location, which Item Descriptions are already present (and by which ItemIds)?
                var locDescToItemIds = new Dictionary<(int LocationId, string DescKey), HashSet<int>>();
                foreach (var il in itemLocations)
                {
                    if (!itemsById.TryGetValue(il.Id, out var parent)) continue; // orphan protection
                    var key = (il.LocationId, Norm(parent.Description));
                    if (!locDescToItemIds.TryGetValue(key, out var set))
                        locDescToItemIds[key] = set = new HashSet<int>();
                    set.Add(il.Id);
                }

                // Index existing ItemLocation by composite key for fast upsert
                var byKey = itemLocations.ToDictionary(x => (x.Id, x.LocationId));

                // Track duplicates & skips
                var seenKeys = new HashSet<(int Id, int LocationId)>();
                int dupKey = 0, idMissing = 0, descIdClash = 0, updated = 0, inserted = 0;
                var dtoItemIds = new HashSet<int>(dtoList.Select(d => d.Id));

                foreach (var dbItem in itemLocations)
                {
                    if (!dtoItemIds.Contains(dbItem.Id))
                    {
                        if (!dbItem.Inactive)
                        {
                            dbItem.Inactive = true;
                            await _log.WarnAsync($"Item {dbItem.Id} not present in DTO feed — marking Inactive=true.", "Seed - ItemLocation");
                        }
                        // Optionally: detach from any “active” lists in memory, etc.
                    }
                }


                foreach (var dto in dtoList ?? Enumerable.Empty<AgvantageItemLocationDTO>())
                {
                    // Normalize DTO
                   
                    var key = (dto.Id, dto.LocationId);

                    // (1) Duplicate in DTOs by composite key (Id, LocationId) ⇒ exclude + warn
                    if (!seenKeys.Add(key))
                    {
                        dupKey++;
                        await _log.WarnAsync($"ItemLocation DTO duplicate (Id={dto.Id}, LocationId={dto.LocationId}) — excluding.", "Seed - ItemLocation");
                        continue;
                    }

                    // (2) DTO.Id must exist in Items; if not, mark Inactive and exclude + warn
                    if (!itemsById.TryGetValue(dto.Id, out var parent))
                    {
                        await _log.WarnAsync($"DTO.Id {dto.Id} not found in Item Master File (Seed.Items) — skipping.", "Seed - ItemLocation");
                        continue;
                    }


                    // (3) Same Item *Description* already present at this Location but tied to a *different* ItemId ⇒ exclude + warn
                    //     (Analogue to your Items rule: "Description matches but Id differs")
                    var parentDescKey = Norm(itemsById[dto.Id].Description);
                    if (locDescToItemIds.TryGetValue((dto.LocationId, parentDescKey), out var idSet)
                        && (idSet.Count > 1 || (idSet.Count == 1 && !idSet.Contains(dto.Id))))
                    {
                        descIdClash++;
                        var others = string.Join(", ", idSet.Where(id => id != dto.Id).OrderBy(x => x));
                        await _log.WarnAsync($"At Location {dto.LocationId}, description '{itemsById[dto.Id].Description}' already maps to ItemId(s) [{others}] — excluding DTO (Id={dto.Id}).","Seed - ItemLocation");
                        continue;
                    }

                    // Upsert
                    if (byKey.TryGetValue(key, out var row))
                    {
                        bool changed = false;
                      

                        if (row.Price != dto.Price) { row.Price = dto.Price; changed = true; }
                        if (row.Inactive != dto.Inactive) { row.Inactive = dto.Inactive; changed = true; }
                        if (row.NotInUse != dto.NotInUse) { row.NotInUse = dto.NotInUse; changed = true; }
                        if (!string.Equals(row.Lot, dto.Lot, StringComparison.Ordinal)) { row.Lot = dto.Lot; changed = true; }
                        if (!string.Equals(row.Comment, dto.Comment, StringComparison.Ordinal)) { row.Comment = dto.Comment; changed = true; }
                       // if (row.DefaultValue != dto.DefaultValue) { row.DefaultValue = dto.DefaultValue; changed = true; }
                        if (changed) updated++;
                    }
                    else
                    {
                        // Insert new ItemLocation
                        var entity = new ItemLocation
                        {
                            Uid = Guid.NewGuid(),
                            Id = dto.Id,
                             
                            LocationId = dto.LocationId,
                            Price = dto.Price,
                            Inactive = dto.Inactive,
                            NotInUse = dto.NotInUse,
                            Lot = string.IsNullOrWhiteSpace(dto.Lot) ? null : dto.Lot,
                            Comment = string.IsNullOrWhiteSpace(dto.Comment) ? null : dto.Comment,
                            DefaultValue = dto.DefaultValue
                        };
                        _seed.ItemLocations.Add(entity);
                        byKey[key] = entity;
                        inserted++;
                        await _log.WarnAsync($"ItemLocation — inserted: , Item Id={dto.Id}, Location={dto.LocationId}.", "Seed - ItemLocation");


                        // Maintain the (Location, Desc) -> ItemIds index so later DTOs see this mapping
                        if (!locDescToItemIds.TryGetValue((dto.LocationId, parentDescKey), out var set))
                            locDescToItemIds[(dto.LocationId, parentDescKey)] = set = new HashSet<int>();
                        set.Add(dto.Id);
                    }
                }

                //if (dupKey > 0 || idMissing > 0 || descIdClash > 0)
                    await _log.InfoAsync($"ItemLocation Updated — skipped: duplicateKey={dupKey}, itemIdMissing={idMissing}, descIdClash={descIdClash}.","Seed - ItemLocation");

                
                    //await _log.InfoAsync($"ItemLocation updated: {updated}, inserted: {inserted}.","Seed - ItemLocation");

                await _seed.SaveChangesAsync();
                _seed.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                await _log.WarnAsync($"Error in ItemLocation Upsert: {TransferLogger.UsefulMessage(ex)}","Seed - ItemLocation");
            }
        }


        public async Task UpsertSeedDepartmentsAsync(IEnumerable<AgvantageSeedDepartmentDTO> dtoList)
        {
            try
            {
                _seed.ChangeTracker.Clear();

                // Current DB state
                var depts = await _seed.Set<SeedDepartment>().AsTracking().ToListAsync();

                // Indexes
                static string Norm(string s) =>
                    new string((s ?? string.Empty).Trim().ToUpperInvariant()
                        .Where(ch => !char.IsWhiteSpace(ch)).ToArray());

                var byId = depts.ToDictionary(d => d.Id);
                var byName = depts.GroupBy(d => Norm(d.Description))
                                  .ToDictionary(g => g.Key, g => g.ToList());

                // Normalize incoming (ignore blank descriptions)
                var incoming = (dtoList ?? Enumerable.Empty<AgvantageSeedDepartmentDTO>())
                    .Where(d => d is not null && !string.IsNullOrWhiteSpace(d.Description))
                    .Select(d => new
                    {
                        Raw = d,
                        Id = d.Id,
                        Desc = d.Description.Trim(),
                        Key = Norm(d.Description),
                        d.SpringWheat,
                        d.NotUsed
                    })
                    .ToList();

                // Rule enforcement
                var seenIds = new HashSet<int>();
                var seenNames = new HashSet<string>();
                int dupId = 0, dupDesc = 0, idMissing = 0, descIdClash = 0, updated = 0;

                foreach (var x in incoming)
                {
                    // (1) Duplicate ID in DTOs ⇒ exclude + warn
                    if (!seenIds.Add(x.Id))
                    {
                        dupId++;
                        await _log.WarnAsync($"SeedDepartments DTO duplicate Id {x.Id} — excluding '{x.Desc}'.", " Seed - SeedDeptartments");
                        continue;
                    }

                    // (1) Duplicate Description in DTOs ⇒ exclude + warn
                    if (!seenNames.Add(x.Key))
                    {
                        dupDesc++;
                        await _log.WarnAsync($"SeedDepartments DTO duplicate Description '{x.Desc}' — excluding Id {x.Id}.", " Seed - SeedDeptartments");
                        continue;
                    }

                    // (2) DTO.Id not found in DB ⇒ add the row to the database
                    if (!byId.ContainsKey(x.Id))
                    {
                        idMissing++;
                        var newDepartment = new SeedDepartment
                        {
                            Uid = Guid.NewGuid(),
                            Id = x.Id,
                            Description = x.Desc,
                            SpringWheat = x.SpringWheat,
                            NotUsed = x.NotUsed
                        };

                        _seed.SeedDepartments.Add(newDepartment);
                        await _log.WarnAsync($"SeedDepartments — inserted: , Item Id={x.Id}, department={x.Desc}.", "Seed - SeedDepartments");
                        continue;
                    }

                    // (3) Same Description exists in DB but tied to a different Id ⇒ exclude + warn
                    if (byName.TryGetValue(x.Key, out var rowsWithName) && rowsWithName.Any(d => d.Id != x.Id))
                    {
                        descIdClash++;
                        var others = string.Join(", ", rowsWithName.Select(d => d.Id).Distinct().OrderBy(v => v));
                        await _log.WarnAsync($"SeedDepartments description '{x.Desc}' already used by Id(s) [{others}] — excluding DTO Id {x.Id}.", " Seed - SeedDeptartments");
                        continue;
                    }

                    // Update the matching row by Id
                    var e = byId[x.Id];
                    bool changed = false;

                    if (!string.Equals(e.Description, x.Desc, StringComparison.Ordinal)) { e.Description = x.Desc; changed = true; }
                    if (e.SpringWheat != x.SpringWheat) { e.SpringWheat = x.SpringWheat; changed = true; }
                    if (e.NotUsed != x.NotUsed) { e.NotUsed = x.NotUsed; changed = true; }

                    if (changed) updated++;
                }

              //  if (dupId > 0 || dupDesc > 0 || idMissing > 0 || descIdClash > 0)
                    await _log.InfoAsync($"SeedDepartments Updated — skipped: duplicateId={dupId}, duplicateDesc={dupDesc}, idMissingInDB={idMissing}, descIdClash={descIdClash}.", " Seed - SeedDeptartments");

                
                 //   await _log.InfoAsync($"SeedDepartments updated: {updated}", " Seed - SeedDeptartments");

                await _seed.SaveChangesAsync();
                _seed.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                await _log.WarnAsync($"Error in SeedDepartments Upsert: {TransferLogger.UsefulMessage(ex)}", " Seed - SeedDeptartments");
            }
        }
    }
}
