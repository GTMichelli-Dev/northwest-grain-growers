using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

[UseAdminConnection]
[ApiController]
[Route("api/AccountItemFilters")]
public sealed class AccountItemFiltersApiController : ControllerBase
{
    private readonly dbContext _ctx;
    private readonly ILogger<AccountItemFiltersApiController> _logger;

    public AccountItemFiltersApiController(
        dbContext ctx,
        ILogger<AccountItemFiltersApiController> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    // GET /api/AccountItemFilters?productId=&itemId=&accountId=
    // Lists account item filters, optionally narrowed by product, item, and/or account.
    // Returns all records when no filters are supplied.
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int? productId,
        [FromQuery] long? itemId,
        [FromQuery] long? accountId,
        CancellationToken ct)
    {
        var query = _ctx.AccountItemFilters
            .AsNoTracking()
            .Where(f => f.Location.UseForWarehouse == true)
            .AsQueryable();

        if (productId.HasValue)
            query = query.Where(f => f.Item.ProductId == productId.Value);
        if (itemId.HasValue)
            query = query.Where(f => f.ItemId == itemId.Value);
        if (accountId.HasValue)
            query = query.Where(f => f.AccountId == accountId.Value);

        var data = await query
            .OrderBy(f => f.Account.LookupName)
            .ThenBy(f => f.Item.Description)
            .Select(f => new
            {
                Id                 = f.Id,
                AccountId          = f.AccountId,
                AccountName        = f.Account.LookupName + " (" + f.Account.As400AccountId + ")",
                ItemId             = f.ItemId,
                ItemDescription    = f.Item.Description,
                ProductId          = f.Item.ProductId,
                ProductDescription = f.Item.Product.Description,
                LocationId         = f.LocationId,
                LocationName       = f.Location.Name,
            })
            .ToListAsync(ct);

        return Ok(data);
    }

    // POST /api/AccountItemFilters
    // Creates a new AccountItemFilter row (grants an account access to a specific item).
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAccountItemFilterDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.AccountId <= 0)
            return BadRequest(new { message = "AccountId is required." });
        if (dto.ItemId <= 0)
            return BadRequest(new { message = "ItemId is required." });
        if (dto.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });

        // Prevent duplicate entries (unique on AccountId + ItemId + LocationId)
        var exists = await _ctx.AccountItemFilters
            .AnyAsync(f => f.AccountId == dto.AccountId && f.ItemId == dto.ItemId && f.LocationId == dto.LocationId, ct);
        if (exists)
            return Conflict(new { message = "This account/item/location combination already exists." });

        var filter = new AccountItemFilter
        {
            Id         = Guid.NewGuid(),
            AccountId  = dto.AccountId,
            ItemId     = dto.ItemId,
            LocationId = dto.LocationId,
        };

        _ctx.AccountItemFilters.Add(filter);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex,
                "Failed to create AccountItemFilter for AccountId={AccountId}, ItemId={ItemId}",
                dto.AccountId, dto.ItemId);
            return StatusCode(500, new { message = "Database error while creating filter." });
        }

        return Ok(new { filter.Id });
    }

    // POST /api/AccountItemFilters/bulk
    // Creates AccountItemFilter rows for ALL active warehouse locations for the given account + item.
    // Silently skips any location that already has a filter.
    [HttpPost("bulk")]
    public async Task<IActionResult> CreateBulk(
        [FromBody] CreateBulkAccountItemFilterDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.AccountId <= 0)
            return BadRequest(new { message = "AccountId is required." });
        if (dto.ItemId <= 0)
            return BadRequest(new { message = "ItemId is required." });

        // Get all active warehouse locations
        var allLocationIds = await _ctx.Locations
            .AsNoTracking()
            .Where(l => l.UseForWarehouse == true && l.IsActive == true)
            .Select(l => l.LocationId)
            .ToListAsync(ct);

        // Get existing filters for this account + item to skip duplicates
        var existingLocationIds = await _ctx.AccountItemFilters
            .AsNoTracking()
            .Where(f => f.AccountId == dto.AccountId && f.ItemId == dto.ItemId)
            .Select(f => f.LocationId)
            .ToListAsync(ct);

        var existingSet = new HashSet<int>(existingLocationIds);
        var newFilters = allLocationIds
            .Where(locId => !existingSet.Contains(locId))
            .Select(locId => new AccountItemFilter
            {
                Id         = Guid.NewGuid(),
                AccountId  = dto.AccountId,
                ItemId     = dto.ItemId,
                LocationId = locId,
            })
            .ToList();

        if (newFilters.Count == 0)
            return Ok(new { added = 0, message = "All locations already have filters for this account and item." });

        _ctx.AccountItemFilters.AddRange(newFilters);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex,
                "Failed to bulk-create AccountItemFilters for AccountId={AccountId}, ItemId={ItemId}",
                dto.AccountId, dto.ItemId);
            return StatusCode(500, new { message = "Database error while creating filters." });
        }

        return Ok(new { added = newFilters.Count });
    }

    // DELETE /api/AccountItemFilters/{id}
    // Removes an AccountItemFilter row (revokes account access to an item).
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var filter = await _ctx.AccountItemFilters.FindAsync(new object[] { id }, ct);
        if (filter is null)
            return NotFound(new { message = "Filter not found." });

        _ctx.AccountItemFilters.Remove(filter);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to delete AccountItemFilter {Id}", id);
            return StatusCode(500, new { message = "Database error while deleting filter." });
        }

        return Ok(new { id });
    }
}

public record CreateAccountItemFilterDto(long AccountId, long ItemId, int LocationId);
public record CreateBulkAccountItemFilterDto(long AccountId, long ItemId);
