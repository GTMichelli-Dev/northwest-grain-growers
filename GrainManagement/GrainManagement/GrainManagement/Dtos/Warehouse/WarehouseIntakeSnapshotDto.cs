namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// Single payload that can fill the Intake screen grids and detail pane.
/// (PascalCase property names are preserved by Program.cs JSON settings.)
/// </summary>
public sealed class WarehouseIntakeSnapshotDto
{
    public int LocationId { get; set; }

    public List<WarehouseTruckDto> TrucksInYard { get; set; } = new();
    public List<WarehouseCertDto> OpenCerts { get; set; } = new();
    public List<WarehouseCertDto> ClosedCerts { get; set; } = new();

    // Keyed by Lot
    public Dictionary<long, WarehouseWeightSheetDetailDto> DetailsByLot { get; set; } = new();

    // Keyed by Lot
    public Dictionary<long, List<WarehouseLoadDto>> LoadsByLot { get; set; } = new();
}
