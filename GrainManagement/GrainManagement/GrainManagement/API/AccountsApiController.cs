#nullable enable
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using GrainManagement.Models;
using GrainManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

[UseAdminConnection]
[ApiController]
[Route("api/accounts")]
public class AccountsApiController : ControllerBase
{
    private readonly dbContext _ctx;

    public AccountsApiController(dbContext ctx)
    {
        _ctx = ctx;
    }

    public class AccountRow
    {
        public long AccountId { get; set; }
        public long As400AccountId { get; set; }
        public bool IsActive { get; set; }
        public string? LookupName { get; set; }
        public bool IsProducer { get; set; }
        public bool IsSeedProducer { get; set; }
        public bool IsWholesale { get; set; }
        public string? Phone1 { get; set; }
        public string? Email { get; set; }
        public int SplitGroupCount { get; set; }
    }

    /// <summary>GET /api/accounts — DevExtreme DataSource load</summary>
    /// <param name="splitFilter">Optional: "none" = no split groups, "has" = has split groups</param>
    [HttpGet]
    public object Get(DataSourceLoadOptions loadOptions, [FromQuery] string? splitFilter = null, [FromQuery] string? search = null, [FromQuery] string? producerFilter = null)
    {
        DevExtremeUtils.NormalizeLoadOptions(loadOptions);

        var baseQuery = _ctx.Accounts
            .AsNoTracking();

        if (producerFilter == "producer")
            baseQuery = baseQuery.Where(a => a.IsProducer);
        else if (producerFilter == "non-producer")
            baseQuery = baseQuery.Where(a => !a.IsProducer);

        if (!string.IsNullOrWhiteSpace(search))
            baseQuery = baseQuery.Where(a => a.LookupName.Contains(search));

        if (splitFilter == "none")
            baseQuery = baseQuery.Where(a => a.IsProducer && !_ctx.SplitGroups.Any(sg => sg.PrimaryAccountId == a.AccountId));
        else if (splitFilter == "has")
            baseQuery = baseQuery.Where(a => _ctx.SplitGroups.Any(sg => sg.PrimaryAccountId == a.AccountId));

        var query = baseQuery.Select(a => new AccountRow
            {
                AccountId = a.AccountId,
                As400AccountId = a.As400AccountId,
                IsActive = a.IsActive,
                LookupName = a.LookupName,
                IsProducer = a.IsProducer,
                IsSeedProducer = a.IsSeedProducer,
                IsWholesale = a.IsWholesale,
                Phone1 = a.Phone1,
                Email = a.Email,
                SplitGroupCount = _ctx.SplitGroups.Count(sg => sg.PrimaryAccountId == a.AccountId)
            });

        return DataSourceLoader.Load(query, loadOptions);
    }

    /// <summary>GET /api/accounts/{accountId}/SplitGroups</summary>
    [HttpGet("{accountId}/SplitGroups")]
    public async Task<IActionResult> GetSplitGroups(long accountId, CancellationToken ct)
    {
        var groups = await _ctx.SplitGroups
            .AsNoTracking()
            .Where(sg => sg.PrimaryAccountId == accountId)
            .OrderBy(sg => sg.SplitGroupDescription)
            .Select(sg => new
            {
                sg.SplitGroupId,
                sg.SplitGroupDescription,
                sg.IsActive,
                sg.UseForSales,
                sg.UseForReceive,
                Percents = sg.SplitGroupPercents
                    .OrderByDescending(p => p.SplitPercent)
                    .Select(p => new
                    {
                        p.Id,
                        p.Account.As400AccountId,
                        AccountName = p.Account.LookupName,
                        p.SplitPercent
                    }).ToList()
            })
            .ToListAsync(ct);

        return Ok(groups);
    }
}
