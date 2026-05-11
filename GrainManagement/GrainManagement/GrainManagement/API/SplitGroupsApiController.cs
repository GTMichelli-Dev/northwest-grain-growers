using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

// Reference-data endpoints for split groups. Intentionally not gated on a
// specific module (mirrors LookupsController) so both the GrowerDelivery
// WeightSheetLots popup and the /Admin/SplitGroups page can call them.
[ApiController]
[Route("api/SplitGroups")]
public sealed class SplitGroupsApiController : ControllerBase
{
    private readonly dbContext _ctx;

    public SplitGroupsApiController(dbContext ctx)
    {
        _ctx = ctx;
    }

    // GET /api/SplitGroups
    // Every active split group with its primary account info. Groups with no
    // PrimaryAccountId are returned with PrimaryAccountName = null so the UI
    // can render "Unassigned"/"Not Set" and highlight the row.
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var groups = await _ctx.SplitGroups
            .AsNoTracking()
            .Where(sg => sg.IsActive)
            .OrderBy(sg => sg.SplitGroupDescription)
            .Select(sg => new
            {
                sg.SplitGroupId,
                sg.SplitGroupDescription,
                sg.PrimaryAccountId,
                PrimaryAccountName = sg.PrimaryAccountId != null
                    ? _ctx.Accounts
                          .Where(a => a.AccountId == sg.PrimaryAccountId)
                          .Select(a => a.EntityName != null && a.EntityName != "" ? a.EntityName : a.LookupName)
                          .FirstOrDefault()
                    : null,
                PrimaryAccountAs400Id = sg.PrimaryAccountId != null
                    ? _ctx.Accounts
                          .Where(a => a.AccountId == sg.PrimaryAccountId)
                          .Select(a => (long?)a.As400AccountId)
                          .FirstOrDefault()
                    : (long?)null,
            })
            .ToListAsync(ct);

        return Ok(groups);
    }

    // GET /api/SplitGroups/{splitGroupId}/Percents
    // Per-grower split rows for one split group — master-detail view.
    [HttpGet("{splitGroupId:int}/Percents")]
    public async Task<IActionResult> GetPercents(int splitGroupId, CancellationToken ct)
    {
        if (splitGroupId <= 0)
            return BadRequest(new { message = "splitGroupId is required." });

        var rows = await _ctx.SplitGroupPercents
            .AsNoTracking()
            .Where(p => p.SplitGroupId == splitGroupId && p.IsActive)
            .Join(_ctx.Accounts,
                  p => p.AccountId,
                  a => a.AccountId,
                  (p, a) => new
                  {
                      p.AccountId,
                      As400AccountId = a.As400AccountId,
                      AccountName    = a.EntityName != null && a.EntityName != "" ? a.EntityName : a.LookupName,
                      p.SplitPercent,
                  })
            .OrderByDescending(r => r.SplitPercent)
            .ThenBy(r => r.AccountName)
            .ToListAsync(ct);

        return Ok(rows);
    }
}
