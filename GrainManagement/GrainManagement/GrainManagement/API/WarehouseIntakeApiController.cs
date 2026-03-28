using GrainManagement.Auth;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using GrainManagement.Dtos.Warehouse;

namespace GrainManagement.API;

[ApiController]
[Route("api/warehouse/intake")]
[RequiresModule(nameof(ModuleOptions.WarehouseIntake))]
public sealed class WarehouseIntakeApiController : ControllerBase
{
    private readonly IWarehouseIntakeDataService _service;

    public WarehouseIntakeApiController(IWarehouseIntakeDataService service)
    {
        _service = service;
    }


    [HttpPost("new-truck")]
    public async Task<IActionResult> CreateNewTruck([FromBody] NewTruckRequestDto request, CancellationToken ct)
    {
        if (request == null)
            return BadRequest(new { error = "Invalid payload." });

        if (request.Weight < 1 || request.Weight > 100000)
            return BadRequest(new { error = "Weight must be between 1 and 100000." });

        // optional sanity checks
        if (string.Equals(request.Mode, "scale", StringComparison.OrdinalIgnoreCase) && (request.ScaleId ?? 0) < 1)
            return BadRequest(new { error = "ScaleId is required when mode = scale." });

        // TODO call service
        // var result = await _service.CreateNewTruckAsync(...);
        //return Ok(new { truckId = request.TruckId, lot = request.Lot });
        return Ok(new { message = "Truck created successfully." });
    }








    [HttpGet("snapshot")]
    public async Task<IActionResult> GetSnapshot([FromQuery] int locationId, CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest("locationId is required");

        var snapshot = await _service.GetSnapshotAsync(locationId, ct);
        return Ok(snapshot);
    }
}
