using GrainManagement.Dtos.Warehouse;

namespace GrainManagement.Services;

/// <summary>
/// Dummy data source used until the real Intake pipeline is wired up.
/// </summary>
public sealed class DummyWarehouseIntakeDataService : IWarehouseIntakeDataService
{
    public Task<WarehouseIntakeSnapshotDto> GetSnapshotAsync(int locationId, CancellationToken ct = default)
    {
        // These values mirror the previous JS dummy arrays so the UI stays identical.
        var snapshot = new WarehouseIntakeSnapshotDto
        {
            LocationId = locationId,
            TrucksInYard = new List<WarehouseTruckDto>
            {
                new() { Id = 101, Bol = 98740, Customer = "MADISON RANCHES", Bin = "01 - White Tank", Moist = 10.2m, Protein = 9.7m, Carrier = "MCGREGORS", Crop = "SWW" },
                new() { Id = 102, Bol = 98495, Customer = "ARCHER FARMS",   Bin = "01 - White Tank", Moist = 10.0m, Protein = 10.0m, Carrier = "MCGREGORS", Crop = "SWW" },
                new() { Id = 103, Bol = 98552, Customer = "LINDELL LANCE",  Bin = "01 - White Tank", Moist = 10.0m, Protein = 10.0m, Carrier = "MCGREGORS", Crop = "SWW" },
            },
            OpenCerts = new List<WarehouseCertDto>
            {
                new() { Lot = 1252740044, Customer = "MADISON RANCHES",           Landlord = "",      Carrier = "MCGREGORS",    Crop = "SWW" },
                new() { Lot = 1252740045, Customer = "ARCHER FARMS",             Landlord = "JONES", Carrier = "ABC TRUCKING", Crop = "DNS" },
                new() { Lot = 1252740046, Customer = "SOUTH VALLEY FARMS, LLC",  Landlord = "SMITH", Carrier = "FAST HAUL",    Crop = "HRS" },
            },
            ClosedCerts = new List<WarehouseCertDto>
            {
                new() { Lot = 1252740001, Customer = "ARCHER FARMS",            Landlord = "",      Carrier = "MCGREGORS", Crop = "SWW" },
                new() { Lot = 1252739999, Customer = "SOUTH VALLEY FARMS, LLC", Landlord = "SMITH", Carrier = "FAST HAUL",  Crop = "HRS" },
            },
            DetailsByLot = new Dictionary<long, WarehouseWeightSheetDetailDto>
            {
                [1252740044] = new() { WeightCertificateId = 1170, Lot = 1252740044, Net = 205200, Grower = "MADISON RANCHES",           Account = "221", Landlord = "",      Field = "",          Commodity = "SWW", Carrier = "MCGREGORS",   Comments = "" },
                [1252740045] = new() { WeightCertificateId = 1171, Lot = 1252740045, Net = 184560, Grower = "ARCHER FARMS",             Account = "314", Landlord = "JONES", Field = "NORTH 40",  Commodity = "DNS", Carrier = "ABC TRUCKING", Comments = "Sample comment…" },
                [1252740046] = new() { WeightCertificateId = 1172, Lot = 1252740046, Net =  99020, Grower = "SOUTH VALLEY FARMS, LLC",  Account = "118", Landlord = "SMITH", Field = "RIVER FLATS",Commodity = "HRS", Carrier = "FAST HAUL",    Comments = "" },
            },
            LoadsByLot = new Dictionary<long, List<WarehouseLoadDto>>
            {
                [1252740044] = new()
                {
                    new() { Bol = 98740, Bin = "01 - East GP", Moist = 10.2m, Protein =  9.7m, Gross = 104620, Tare = 35480, Net = 69140 },
                    new() { Bol = 98495, Bin = "01 - East GP", Moist = 10.0m, Protein = 10.0m, Gross = 103760, Tare = 35300, Net = 68460 },
                    new() { Bol = 98552, Bin = "01 - East GP", Moist = 10.0m, Protein = 10.0m, Gross = 102740, Tare = 35140, Net = 67600 },
                },
                [1252740045] = new()
                {
                    new() { Bol = 99001, Bin = "02 - South Bin", Moist = 9.8m, Protein = 11.2m, Gross = 100200, Tare = 35200, Net = 65000 },
                    new() { Bol = 99002, Bin = "02 - South Bin", Moist = 9.9m, Protein = 11.1m, Gross = 101140, Tare = 35580, Net = 65560 },
                },
                [1252740046] = new()
                {
                    new() { Bol = 99110, Bin = "Pile A", Moist = 11.0m, Protein = 12.5m, Gross = 70200, Tare = 35180, Net = 35020 },
                    new() { Bol = 99111, Bin = "Pile A", Moist = 11.2m, Protein = 12.4m, Gross = 69900, Tare = 35800, Net = 34100 },
                },
            }
        };

        return Task.FromResult(snapshot);
    }
}
