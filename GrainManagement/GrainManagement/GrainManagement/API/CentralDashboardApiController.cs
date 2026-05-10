using GrainManagement.Services.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.API;

[ApiController]
[Route("api/CentralDashboard")]
public sealed class CentralDashboardApiController : ControllerBase
{
    private readonly ICentralDashboardService _service;

    public CentralDashboardApiController(ICentralDashboardService service)
    {
        _service = service;
    }

    /// <summary>
    /// GET /api/CentralDashboard?from=YYYY-MM-DD&amp;to=YYYY-MM-DD
    /// Both parameters default to the server's local "today" when omitted.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken ct)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var fromDate = from ?? today;
        var toDate   = to   ?? today;

        var rows = await _service.GetAsync(fromDate, toDate, ct);

        return Ok(new
        {
            FromDate    = fromDate.ToString("yyyy-MM-dd"),
            ToDate      = toDate.ToString("yyyy-MM-dd"),
            GeneratedAt = DateTimeOffset.Now,
            Rows        = rows,
        });
    }
}
