using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using GrainManagement.Models;
using GrainManagement.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

[UseAdminConnection]
[ApiController]
[Route("api/items")]
public class ItemsApiController : ControllerBase
{
    private readonly dbContext _ctx;

    public ItemsApiController(dbContext ctx)
    {
        _ctx = ctx;
    }

    public class ItemRow
    {
        public long ItemId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public long ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string CategoryDescription { get; set; }
        public string SystemUsage { get; set; }
        public int TraitCount { get; set; }
        public int OtherTraitCount { get; set; }
    }

    /// <summary>GET /api/items — DevExtreme DataSource load</summary>
    [HttpGet]
    public object Get(DataSourceLoadOptions loadOptions, [FromQuery] string? search = null)
    {
        DevExtremeUtils.NormalizeLoadOptions(loadOptions);

        var baseQuery = _ctx.Items.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            baseQuery = baseQuery.Where(i => i.Description.Contains(search)
                || i.Product.Description.Contains(search));

        var query = baseQuery.Select(i => new ItemRow
        {
            ItemId = i.ItemId,
            Description = i.Description,
            IsActive = i.IsActive,
            ProductId = i.ProductId,
            ProductDescription = i.Product.Description,
            CategoryDescription = i.Product.Category.Description,
            SystemUsage = i.ItemTraits
                .Where(it => it.TraitTypeId == 12)
                .Select(it => it.Trait.Description)
                .FirstOrDefault(),
            TraitCount = i.ItemTraits.Count(),
            OtherTraitCount = i.ItemTraits.Count(it => it.TraitTypeId != 12)
        });

        return DataSourceLoader.Load(query, loadOptions);
    }

    /// <summary>GET /api/items/lookups — distinct values for filter dropdowns</summary>
    [HttpGet("lookups")]
    public async Task<IActionResult> GetLookups(CancellationToken ct)
    {
        var categories = await _ctx.Items.AsNoTracking()
            .Select(i => i.Product.Category.Description)
            .Where(d => d != null)
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync(ct);

        var products = await _ctx.Items.AsNoTracking()
            .Select(i => i.Product.Description)
            .Where(d => d != null)
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync(ct);

        var systemUsages = await _ctx.ItemTraits.AsNoTracking()
            .Where(it => it.TraitTypeId == 12)
            .Select(it => it.Trait.Description)
            .Where(d => d != null)
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync(ct);

        return Ok(new { Categories = categories, Products = products, SystemUsages = systemUsages });
    }

    /// <summary>GET /api/items/{itemId}/Traits</summary>
    [HttpGet("{itemId}/Traits")]
    public async Task<IActionResult> GetTraits(long itemId, CancellationToken ct)
    {
        var traits = await _ctx.ItemTraits
            .AsNoTracking()
            .Where(it => it.ItemId == itemId)
            .OrderBy(it => it.Trait.TraitType.Description)
            .ThenBy(it => it.Trait.Description)
            .Select(it => new
            {
                it.TraitId,
                TraitDescription = it.Trait.Description,
                TraitCode = it.Trait.TraitCode,
                TraitTypeDescription = it.Trait.TraitType.Description,
                it.Trait.TraitType.TraitTypeId
            })
            .ToListAsync(ct);

        return Ok(traits);
    }
}
