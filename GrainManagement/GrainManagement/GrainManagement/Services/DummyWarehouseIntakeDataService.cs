using GrainManagement.Dtos.Warehouse;

namespace GrainManagement.Services;

/// <summary>
/// Dummy data source used until the real Intake pipeline is wired up.
/// </summary>
public sealed class DummyWarehouseIntakeDataService : IWarehouseIntakeDataService
{
    public Task<WarehouseIntakeSnapshotDto> GetSnapshotAsync(int locationId, CancellationToken ct = default)
    {
        // Deterministic "random" values so the UI is stable across refreshes.
        // Changing locationId will change the generated numbers.
        var lotsOpen = new[]
        {
            (Lot: 1252740044L, Grower: "MADISON RANCHES",          Carrier: "MCGREGORS",    Crop: "SWW"),
            (Lot: 1252740045L, Grower: "ARCHER FARMS",            Carrier: "ABC TRUCKING", Crop: "DNS"),
            (Lot: 1252740046L, Grower: "SOUTH VALLEY FARMS, LLC", Carrier: "FAST HAUL",    Crop: "HRS"),
        };

        var lotsClosed = new[]
        {
            (Lot: 1252740001L, Grower: "ARCHER FARMS",            Carrier: "MCGREGORS", Crop: "SWW"),
            (Lot: 1252739999L, Grower: "SOUTH VALLEY FARMS, LLC", Carrier: "FAST HAUL",  Crop: "HRS"),
        };

        // Build LoadsByLot + DetailsByLot Net from the loads (auto-calculated).
        var loadsByLot = new Dictionary<long, List<WarehouseLoadDto>>();
        var detailsByLot = new Dictionary<long, WarehouseWeightSheetDetailDto>();

        // Use a base timestamp per snapshot so the times look realistic.
        var baseUtc = DateTimeOffset.UtcNow.Date.AddHours(15); // ~7am Pacific (depending on DST)

        foreach (var item in lotsOpen.Concat(lotsClosed))
        {
            var rng = new Random(unchecked((int)(item.Lot ^ (locationId * 7919))));
            var loadCount = rng.Next(3, 6); // 3–5 loads per lot

            var loads = new List<WarehouseLoadDto>(capacity: loadCount + 1);

            decimal runningNet = 0m;

            // Make BOLs distinct per lot
            var bolBase = rng.Next(98000, 99900);

            for (int i = 0; i < loadCount; i++)
            {
                // Moist/Protein ranges by crop-ish (still just dummy realism)
                var moist = NextDecimal(rng, 9.6m, item.Crop == "HRS" ? 12.2m : 11.2m, 1);
                var protein = NextDecimal(rng, item.Crop == "DNS" ? 10.5m : 9.2m, item.Crop == "HRS" ? 12.8m : 11.6m, 1);

                // Gross/Tare ranges
                var gross = NextDecimal(rng, 98000m, 112000m, 0);
                var tare = NextDecimal(rng, 34000m, 38000m, 0);

                var net = gross - tare;
                runningNet += net;

                var weighedAt = baseUtc.AddMinutes(rng.Next(8, 16) * i).AddSeconds(rng.Next(0, 59));

                loads.Add(new WarehouseLoadDto
                {
                    Bol = bolBase + i,
                    Bin = rng.Next(0, 3) switch
                    {
                        0 => "01 - East GP",
                        1 => "02 - South Bin",
                        _ => "Pile A"
                    },
                    Moist = moist,
                    Protein = protein,
                    Gross = gross,
                    Tare = tare,
                    Net = net,
                    WeighedAtUtc = weighedAt,
                    RunningNet = runningNet,
                    IsSummary = false
                });
            }

            // Add summary row at the end
            loads.Add(new WarehouseLoadDto
            {
                Bol = 0,
                Bin = "TOTAL",
                Moist = 0m,
                Protein = 0m,
                Gross = 0m,
                Tare = 0m,
                Net = runningNet,
                WeighedAtUtc = loads.Count > 0 ? loads[^1].WeighedAtUtc : baseUtc,
                RunningNet = runningNet,
                IsSummary = true
            });

            loadsByLot[item.Lot] = loads;

            // Detail net should match the sum of loads.
            detailsByLot[item.Lot] = new WarehouseWeightSheetDetailDto
            {
                WeightCertificateId = rng.Next(1170, 1190),
                Lot = item.Lot,
                Net = runningNet,
                Grower = item.Grower,
                Account = rng.Next(100, 399).ToString(),
                Landlord = "",
                Field = "",
                Commodity = item.Crop,
                Carrier = item.Carrier,
                Comments = ""
            };
        }

        // Trucks in yard are loosely tied to open lots (but are still dummy)
        var snapshot = new WarehouseIntakeSnapshotDto
        {
            LocationId = locationId,
            TrucksInYard = new List<WarehouseTruckDto>
            {
                new() { Id = 101, Bol = 98740, Customer = "MADISON RANCHES", Bin = "01 - White Tank", Moist = 10.2m, Protein =  9.7m, Carrier = "MCGREGORS", Crop = "SWW" },
                new() { Id = 102, Bol = 98495, Customer = "ARCHER FARMS",   Bin = "01 - White Tank", Moist = 10.0m, Protein = 10.0m, Carrier = "MCGREGORS", Crop = "SWW" },
                new() { Id = 103, Bol = 98552, Customer = "LINDELL LANCE",  Bin = "01 - White Tank", Moist = 10.0m, Protein = 10.0m, Carrier = "MCGREGORS", Crop = "SWW" },
            },
            OpenCerts = lotsOpen.Select(x => new WarehouseCertDto { Lot = x.Lot, Customer = x.Grower, Landlord = "", Carrier = x.Carrier, Crop = x.Crop }).ToList(),
            ClosedCerts = lotsClosed.Select(x => new WarehouseCertDto { Lot = x.Lot, Customer = x.Grower, Landlord = "", Carrier = x.Carrier, Crop = x.Crop }).ToList(),
            DetailsByLot = detailsByLot,
            LoadsByLot = loadsByLot
        };

        return Task.FromResult(snapshot);
    }

    private static decimal NextDecimal(Random rng, decimal min, decimal max, int decimals)
    {
        var span = (double)(max - min);
        var val = (double)min + (rng.NextDouble() * span);
        var m = (decimal)val;

        if (decimals <= 0) return decimal.Round(m, 0, MidpointRounding.AwayFromZero);
        return decimal.Round(m, decimals, MidpointRounding.AwayFromZero);
    }
}
