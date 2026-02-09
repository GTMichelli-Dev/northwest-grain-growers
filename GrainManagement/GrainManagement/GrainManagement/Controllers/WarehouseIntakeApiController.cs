using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.Controllers;

[ApiController]
[Route("api/warehouse/intake")]
public sealed class WarehouseIntakeApiController : ControllerBase
{
    private readonly IWarehouseIntakeDataService _service;

    public WarehouseIntakeApiController(IWarehouseIntakeDataService service)
    {
        _service = service;
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
