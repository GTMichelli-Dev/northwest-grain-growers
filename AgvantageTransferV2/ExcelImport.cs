using Agvantage_Transfer.DTOModels;
using Agvantage_Transfer.Logging;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Agvantage_Transfer.SeedModels;


namespace Agvantage_Transfer;

public sealed class ExcelImport( ITransferLogger log)
{
    private readonly ITransferLogger _log = log;
    public async Task<List<AgvantageCarrierDTO>> LoadCarriersAsync(string xlsxPath)
    {
        if (string.IsNullOrWhiteSpace(xlsxPath))
            await _log.ErrorAsync("xlsxPath is empty.", "System");


        if (!File.Exists(xlsxPath))
            await _log.ErrorAsync("Carriers.xlsx not found.", "System");
           

        return await Task.Run(async () =>
        {
            try
            {
                using var wb = new XLWorkbook(xlsxPath);
                var ws = wb.Worksheets.FirstOrDefault(w =>
                             string.Equals(w.Name, "Carriers", StringComparison.OrdinalIgnoreCase))
                         ?? wb.Worksheet(1);

                var used = ws.RangeUsed() ?? throw new InvalidOperationException("No data found in Carriers.xlsx.");
                var headerRow = used.FirstRowUsed();

                var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                foreach (var cell in headerRow.CellsUsed())
                {
                    var key = NormalizeHeader(cell.GetString());
                    if (!string.IsNullOrEmpty(key) && !headerMap.ContainsKey(key))
                        headerMap[key] = cell.Address.ColumnNumber;
                }

                int colId = FindColumn(headerMap, "VMVNO");
                int colDescription = FindColumn(headerMap, "VMVNAM");
                int colActive = FindColumn(headerMap, "VMCRTIN");

                if (colDescription <= 0)
                    throw new InvalidOperationException("Could not find a Description column in Carriers.xlsx.");

                var results = new List<AgvantageCarrierDTO>();
                foreach (var row in used.RowsUsed().Skip(1))
                {
                    var id = colId > 0 ? ParseInt(row.Cell(colId).GetValue<string>()) : 0;
                    var desc = row.Cell(colDescription).GetValue<string>()?.Trim() ?? string.Empty;
                    var act = colActive > 0 ? ParseBool(row.Cell(colActive).GetValue<string>()) : true;

                    if (string.IsNullOrWhiteSpace(desc)) continue;

                    results.Add(new AgvantageCarrierDTO { Id = id, Description = desc, Active = act });
                }
                await _log.InfoAsync($"Loaded {results.Count} carriers from {xlsxPath}", "Nw_Data - Carriers");
               
                return results;
            }
            catch (Exception ex)
            {
                await _log.ErrorAsync($"Failed to load carriers from Excel:{ex.Message}","Carriers");
                
                return new List<AgvantageCarrierDTO>();
            }
        });
    }

    // helpers
    private static int FindColumn(IDictionary<string, int> map, params string[] names)
    { foreach (var n in names) if (map.TryGetValue(NormalizeHeader(n), out var c)) return c; return -1; }

    private static string NormalizeHeader(string? s)
        => string.IsNullOrWhiteSpace(s) ? "" : new string(s.Trim().ToLowerInvariant().Where(ch => !char.IsWhiteSpace(ch) && !char.IsPunctuation(ch)).ToArray());

    private static int ParseInt(string? s) { _ = int.TryParse((s ?? "").Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var v); return v; }
    private static bool ParseBool(string? s)
    {
        var t = (s ?? "").Trim().ToLowerInvariant();
        if (bool.TryParse(t, out var b)) return b;
        return t is "y" or "yes" or "1";
    }



    public async Task<List<AgvantageProducerDTO>> LoadProducersAsync(string excelPath)
    {
        var results = new List<AgvantageProducerDTO>();
        if (string.IsNullOrWhiteSpace(excelPath) || !File.Exists(excelPath))
            return results;

        await Task.Yield(); // keep signature truly async-friendly

        using var wb = new XLWorkbook(excelPath);
        var ws = wb.Worksheet(1);

        foreach (var row in ws.RowsUsed())
        {
            // mimic your guard: only rows with numeric ID in col 1
            var idStr = row.Cell(1).GetValue<string>().Trim();
            if (!idStr.All(char.IsDigit)) continue;

            int id = int.Parse(idStr);
            string desc = row.Cell(2).GetValue<string>()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(desc)) continue;

            // Active: your code intended "I" or "D" in col 3 => inactive
            var activeToken = row.Cell(3).GetValue<string>().Trim().ToUpperInvariant();
            bool active = !(activeToken == "I" || activeToken == "D");

            string email = row.Cell(4).GetValue<string>()?.Trim() ?? string.Empty;

            var dto = new AgvantageProducerDTO
            {
                Id = id,
                Description = desc,
                Active = active,
                EmailAddress = string.IsNullOrWhiteSpace(email) ? null : email,
                EmailWs = IsValidEmail(email),
                PrintWs = true, // matches your TransferCustomer default
                CompanyName = row.Cell(5).GetValue<string>()?.Trim(),
                CustomerName1 = row.Cell(6).GetValue<string>()?.Trim(),
                CustomerName2 = row.Cell(7).GetValue<string>()?.Trim(),
                Address1 = row.Cell(8).GetValue<string>()?.Trim(),
                Address2 = row.Cell(9).GetValue<string>()?.Trim(),
                City = row.Cell(10).GetValue<string>()?.Trim(),
                State = row.Cell(11).GetValue<string>()?.Trim(),
                Zip1 = row.Cell(12).GetValue<string>()?.Trim(),
                Zip2 = row.Cell(13).GetValue<string>()?.Trim(),
                HomePhone = row.Cell(14).GetValue<string>()?.Trim(),
                WorkPhone = row.Cell(15).GetValue<string>()?.Trim(),
                MobilePhone = row.Cell(15).GetValue<string>()?.Trim(),

                Member = row.Cell(16).GetValue<string>()?.Trim(),
                Phone = row.Cell(17).GetValue<string>()?.Trim(),
                // NOTE: col 18 was written to Member again in your code; skipping here
            };

            results.Add(dto);
        }

        return results;
    }






    private static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;
        try { _ = new System.Net.Mail.MailAddress(email); return true; }
        catch { return false; }
    }


    public async Task<List<AgvantageCropDTO>> LoadCropsAsync(string excelPath)
    {
        var results = new List<AgvantageCropDTO>();
        if (string.IsNullOrWhiteSpace(excelPath) || !File.Exists(excelPath))
            return results;

        await Task.Yield(); // keep signature async-friendly

        using var wb = new XLWorkbook(excelPath);
        var ws = wb.Worksheet(1);

        foreach (var row in ws.RowsUsed())
        {
            // Only rows with numeric Id in col 1
            var idStr = row.Cell(1).GetValue<string>().Trim();
            if (!idStr.All(char.IsDigit)) continue;

            int id = int.Parse(idStr);
            string desc = row.Cell(2).GetValue<string>()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(desc)) continue;

            // Pounds-per-unit (#/bu, #/ton, etc.) from col 3
            var ppuStr = row.Cell(3).GetValue<string>().Trim();
            float poundPerBushel = 0f;
            if (decimal.TryParse(ppuStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var ppuDec))
                poundPerBushel = (float)ppuDec;

            // UOM mapping from col 4 -> B/H/T
            var uomRaw = row.Cell(4).GetValue<string>()?.Trim() ?? string.Empty;
            var uom = ConvertAgvantageUomToNWDataUom(uomRaw);

            // Your legacy rule: Id > 79 → Seed crop
            bool seed = id > 79;

            results.Add(new AgvantageCropDTO
            {
                Id = id,
                Description = desc,
                UnitOfMeasure = uom,
                PoundPerBushel = poundPerBushel,
                UseAtElevator = !seed,
                UseAtSeedMill = seed,
                Active = true,
                ColorIndex = null,
                SecondaryColorIndex = null
            });
        }

        await _log.InfoAsync($"Loaded {results.Count} crops from {excelPath}","Crops");
        return results;
    }

    // Keep the mapping logic close-by (exactly as you described)
    private static string ConvertAgvantageUomToNWDataUom(string UOM)
    {
        var t = (UOM ?? string.Empty).ToUpperInvariant();
        if (t.Contains("CW")) return "H";  // hundredweight
        if (t.Contains("TO")) return "T";  // ton
        return "B";                        // bushel (default)
    }


    public async Task<List<AgvantageItemDTO>> LoadItemsAsync(string excelPath)
    {
        var results = new List<AgvantageItemDTO>();

        if (string.IsNullOrWhiteSpace(excelPath) || !File.Exists(excelPath))
        {
            await _log.ErrorAsync("Seed Master Excel not found or path empty.","Seed - Items");
            return results;
        }

        await Task.Yield(); // keep signature truly async-friendly

        using var wb = new XLWorkbook(excelPath);
        var ws = wb.Worksheet(1);
        var rows = ws.RowsUsed();

        foreach (var row in rows)
        {
            // Only process rows with numeric Id in the first column
            var idStr = row.Cell(1).GetValue<string>().Trim();
            if (!idStr.All(char.IsDigit)) continue;

            int id = int.Parse(idStr, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);

            string description = row.Cell(2).GetValue<string>()?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(description)) continue;

            // FLC (col 3)
            var flcTxt = row.Cell(3).GetValue<string>().Trim();
            int flc = int.TryParse(flcTxt, out var flcVal) ? flcVal : 0;

            // Dept (col 4)
            var deptTxt = row.Cell(4).GetValue<string>().Trim();
            int? dept = int.TryParse(deptTxt, out var deptVal) ? deptVal : null;

            // Inactive flag (col 5): "I" means inactive, anything else = active
            var inactiveToken = row.Cell(5).GetValue<string>().Trim().ToUpperInvariant();
            bool inactive = inactiveToken == "I";

            results.Add(new AgvantageItemDTO
            {
                // Uid intentionally left default (do not generate random GUIDs here)
                Id = id,
                Description = description,
                Flc = flc,
                Dept = dept,
                Inactive = inactive,
                NotInUse = false,
                ItemType = SeedItemTypeFromFlc(flc), // Seed / Chemical / Other
                Uomcode = SeedItemUomFromFlc(flc),  // "L" or "O"
                StoreLocation = null,
                Comment = null
            });
        }

        await _log.InfoAsync($"Loaded {results.Count} seed items from {excelPath}","Seed Master");
        return results;
    }

    // --- helpers (match your legacy mapping rules) ---
    private static string SeedItemTypeFromFlc(int flc)
    {
        // 1,2,3,4,310,311 => "Seed"; 280 => "Chemical"; else "Other"
        if (flc == 1 || flc == 2 || flc == 3 || flc == 4 || flc == 310 || flc == 311) return "Seed";
        if (flc == 280) return "Chemical";
        return "Other";
    }

    private static string SeedItemUomFromFlc(int flc)
    {
        // Legacy default "L"; 280 => "O"
        if (flc == 280) return "O";
        return "L";
    }




    public async Task<List<AgvantageItemLocationDTO>> LoadItemLocationsAsync(string excelPath)
    {
        var results = new List<AgvantageItemLocationDTO>();

        if (string.IsNullOrWhiteSpace(excelPath) || !File.Exists(excelPath))
        {
            await _log.ErrorAsync("Item Location Excel not found or path empty.", "Seed - ItemLocation");
            return results;
        }

        await Task.Yield(); // keep the async signature cooperative

        using var wb = new XLWorkbook(excelPath);
        var ws = wb.Worksheet(1);

        var firstRowUsedObj = ws.FirstRowUsed();
        if (firstRowUsedObj == null)
        {
            await _log.ErrorAsync("No rows found in worksheet.", "Seed - ItemLocation");
            return results;
        }
        var firstRow = firstRowUsedObj.RowNumber();
        var lastRow = ws.LastRowUsed()?.RowNumber() ?? firstRow;

        // Try header-based mapping; fallback to fixed positions if headers not found.
        var headerRow = ws.Row(firstRow);
        var headers = headerRow.CellsUsed()
            .ToDictionary(c => c.GetString().Trim().ToLowerInvariant(), c => c.Address.ColumnNumber);

        bool hasHeader = headers.ContainsKey("id") && headers.ContainsKey("locationid");

        int idxId = GetIndex("id", 1);
        int idxLocationId = GetIndex("locationid", 2);
        int idxPrice = GetIndex("price", 6);
        int idxLot = GetIndex("lot", 4);
        int idxComment = GetIndex("comment", 5);
        int idxInactive = GetIndex("inactive", 6);
        int idxDefaultValue = GetIndex("defaultvalue", 7);

        int startRow = hasHeader ? firstRow + 1 : firstRow;

        for (int r = startRow; r <= lastRow; r++)
        {
            var row = ws.Row(r);

            // required keys
            var idText = row.Cell(idxId).GetValue<string>().Trim();
            var locText = row.Cell(idxLocationId).GetValue<string>().Trim();
            if (!int.TryParse(idText, out int id) || !int.TryParse(locText, out int locationId))
                continue;

            // optional / typed fields
            decimal price = ParseDecimal(row.Cell(idxPrice).GetValue<string>());
            string lot = "";// row.Cell(idxLot).GetValue<string>()?.Trim() ?? string.Empty;
            string comment = "";// row.Cell(idxComment).GetValue<string>()?.Trim() ?? string.Empty;
            bool inactive = false;// ParseInactive(row.Cell(idxInactive).GetValue<string>());
            decimal? defValue = null;// ParseNullableDecimal(row.Cell(idxDefaultValue).GetValue<string>());

            results.Add(new AgvantageItemLocationDTO
            {
                Id = id,
                LocationId = locationId,
                Price = price,
                Inactive = inactive,
                NotInUse = false,
                Lot = string.IsNullOrWhiteSpace(lot) ? string.Empty : lot,
                Comment = string.IsNullOrWhiteSpace(comment) ? string.Empty : comment,
                DefaultValue = defValue
            });
        }

        await _log.InfoAsync($"Loaded {results.Count} item-location rows from {excelPath}", "Seed - ItemLocation");
        return results;

        // ---------- local helpers ----------
        int GetIndex(string name, int fallback) =>
            headers.TryGetValue(name, out var col) ? col : fallback;

        //static bool ParseInactive(string v)
        //{
        //    if (string.IsNullOrWhiteSpace(v)) return false;
        //    var t = v.Trim().ToUpperInvariant();
        //    return t == "I" || t == "Y" || t == "1" || t == "TRUE" || t == "T";
        //}

        static decimal ParseDecimal(string v) =>
            decimal.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out var d) ? d : 0m;

        //static decimal? ParseNullableDecimal(string v) =>
        //    decimal.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out var d) ? d : (decimal?)null;
    }

    public async Task<List<AgvantageSeedDepartmentDTO>> LoadSeedDepartmentsAsync(string excelPath)
    {
        var results = new List<AgvantageSeedDepartmentDTO>();

        if (string.IsNullOrWhiteSpace(excelPath) || !File.Exists(excelPath))
        {
            await _log.ErrorAsync("Seed Department Excel not found or path empty.", "Seed Departments");
            return results;
        }

        await Task.Yield(); // keep the async signature cooperative

        using var wb = new XLWorkbook(excelPath);
        var ws = wb.Worksheet(1);

        var firstRowUsedObj = ws.FirstRowUsed();
        if (firstRowUsedObj == null)
        {
            await _log.ErrorAsync("No rows found in worksheet.", "Seed Departments");
            return results;
        }
        var firstRow = firstRowUsedObj.RowNumber();
        var lastRow = ws.LastRowUsed()?.RowNumber() ?? firstRow;

        // Header mapping (fallbacks to fixed positions if headers are missing)
        var headerRow = ws.Row(firstRow);
        var headers = headerRow.CellsUsed()
            .ToDictionary(c => c.GetString().Trim().ToLowerInvariant(), c => c.Address.ColumnNumber);

        bool hasHeader = headers.ContainsKey("id") && headers.ContainsKey("description");

        int idxId = GetIndex(new[] { "id", "deptid", "departmentid" }, 1);
        int idxDescription = GetIndex(new[] { "description", "name", "dept" }, 2);
        int idxSpringWheat = GetIndex(new[] { "springwheat", "spring wheat", "spring_wheat" }, 3);
        int idxNotUsed = GetIndex(new[] { "notused", "not used", "inactive" }, 4);

        int startRow = hasHeader ? firstRow + 1 : firstRow;

        for (int r = startRow; r <= lastRow; r++)
        {
            var row = ws.Row(r);

            var idText = row.Cell(idxId).GetValue<string>().Trim();
            var desc = row.Cell(idxDescription).GetValue<string>()?.Trim();

            if (!int.TryParse(idText, NumberStyles.Integer, CultureInfo.InvariantCulture, out int id))
                continue;
            if (string.IsNullOrWhiteSpace(desc))
                continue;

            bool springWheat = ParseBool(row.Cell(idxSpringWheat).GetValue<string>());
            bool notUsed = ParseBool(row.Cell(idxNotUsed).GetValue<string>());

            results.Add(new AgvantageSeedDepartmentDTO
            {
                // Uid left default (do not generate here)
                Id = id,
                Description = desc,
                SpringWheat = springWheat,
                NotUsed = notUsed
            });
        }

        await _log.InfoAsync($"Loaded {results.Count} seed departments from {excelPath}", "Seed Departments");
        return results;

        // ---------- local helpers ----------
        int GetIndex(string[] names, int fallback)
        {
            foreach (var n in names)
                if (headers.TryGetValue(n, out var col)) return col;
            return fallback;
        }

        static bool ParseBool(string v)
        {
            if (string.IsNullOrWhiteSpace(v)) return false;
            var t = v.Trim();
            if (bool.TryParse(t, out var b)) return b;
            t = t.ToUpperInvariant();
            return t is "Y" or "YES" or "1" or "T" or "TRUE";
        }
    }

}
