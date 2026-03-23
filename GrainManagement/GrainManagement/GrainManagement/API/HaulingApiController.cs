using GrainManagement.Dtos.Hauling;
using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

[ApiController]
[Route("api/Hauling")]
public sealed class HaulingApiController : ControllerBase
{
    private readonly dbContext _ctx;

    public HaulingApiController(dbContext ctx)
    {
        _ctx = ctx;
    }

    // ── Haulers ──────────────────────────────────────────────────────────

    [HttpGet("Haulers")]
    public async Task<IActionResult> GetHaulers(CancellationToken ct)
    {
        var data = await _ctx.Haulers
            .AsNoTracking()
            .OrderBy(h => h.Description)
            .Select(h => new
            {
                h.Id,
                h.Description,
                h.IsActive
            })
            .ToListAsync(ct);

        return Ok(data);
    }

    // ── Fuel Surcharge ───────────────────────────────────────────────────

    [HttpGet("Districts")]
    public async Task<IActionResult> GetDistricts(CancellationToken ct)
    {
        var data = await _ctx.LocationDistricts
            .AsNoTracking()
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .Select(d => new
            {
                d.DistrictId,
                d.Name
            })
            .ToListAsync(ct);

        return Ok(data);
    }

    [HttpGet("FuelSurcharges")]
    public async Task<IActionResult> GetFuelSurcharges(CancellationToken ct)
    {
        var data = await _ctx.Locations
            .AsNoTracking()
            .Where(l => l.IsActive)
            .OrderBy(l => l.Name)
            .Select(l => new
            {
                l.LocationId,
                l.Name,
                l.Code,
                l.DistrictId,
                DistrictName = l.District.Name,
                l.FuelSurcharge
            })
            .ToListAsync(ct);

        return Ok(data);
    }

    [UseAdminConnection]
    [HttpPatch("FuelSurcharge/{locationId:int}")]
    public async Task<IActionResult> UpdateFuelSurcharge(int locationId, [FromBody] UpdateFuelSurchargeDto dto, CancellationToken ct)
    {
        var loc = await _ctx.Locations.FindAsync(new object[] { locationId }, ct);
        if (loc == null) return NotFound();

        loc.FuelSurcharge = dto.FuelSurcharge;
        await _ctx.SaveChangesAsync(ct);
        return Ok();
    }

    [UseAdminConnection]
    [HttpPatch("FuelSurchargeByDistrict/{districtId:int}")]
    public async Task<IActionResult> UpdateFuelSurchargeByDistrict(int districtId, [FromBody] UpdateFuelSurchargeDto dto, CancellationToken ct)
    {
        var locations = await _ctx.Locations
            .Where(l => l.DistrictId == districtId && l.IsActive)
            .ToListAsync(ct);

        foreach (var loc in locations)
            loc.FuelSurcharge = dto.FuelSurcharge;

        await _ctx.SaveChangesAsync(ct);
        return Ok(new { Updated = locations.Count });
    }

    [UseAdminConnection]
    [HttpPatch("FuelSurchargeAll")]
    public async Task<IActionResult> UpdateFuelSurchargeAll([FromBody] UpdateFuelSurchargeDto dto, CancellationToken ct)
    {
        var locations = await _ctx.Locations
            .Where(l => l.IsActive)
            .ToListAsync(ct);

        foreach (var loc in locations)
            loc.FuelSurcharge = dto.FuelSurcharge;

        await _ctx.SaveChangesAsync(ct);
        return Ok(new { Updated = locations.Count });
    }

    // ── Hauler Rates ─────────────────────────────────────────────────────

    [HttpGet("HaulerRates")]
    public async Task<IActionResult> GetHaulerRates(CancellationToken ct)
    {
        var data = await _ctx.WeightSheetHaulerRates
            .AsNoTracking()
            .OrderBy(r => r.RateType)
            .ThenBy(r => r.MaxDistance)
            .Select(r => new
            {
                r.Id,
                r.RateType,
                r.MaxDistance,
                r.Rate
            })
            .ToListAsync(ct);

        return Ok(data);
    }

    [UseAdminConnection]
    [HttpPatch("HaulerRates/{id:int}")]
    public async Task<IActionResult> UpdateHaulerRate(int id, [FromBody] UpdateHaulerRateDto dto, CancellationToken ct)
    {
        if (dto.Rate < 0 || dto.Rate > 10)
            return BadRequest(new { message = "Rate must be between 0 and 10." });

        var rate = await _ctx.WeightSheetHaulerRates.FindAsync(new object[] { id }, ct);
        if (rate == null) return NotFound();

        rate.Rate = dto.Rate;
        await _ctx.SaveChangesAsync(ct);
        return Ok();
    }
}
