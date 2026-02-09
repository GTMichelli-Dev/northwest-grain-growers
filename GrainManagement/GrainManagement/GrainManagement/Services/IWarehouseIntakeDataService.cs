using GrainManagement.Dtos.Warehouse;

namespace GrainManagement.Services;

/// <summary>
/// Abstraction for the Intake screen data source.
/// Today: dummy in-memory data.
/// Later: SQL/AS400/PLC/SOAP-backed implementation.
/// </summary>
public interface IWarehouseIntakeDataService
{
    Task<WarehouseIntakeSnapshotDto> GetSnapshotAsync(int locationId, CancellationToken ct = default);
}
