using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API;

[UseAdminConnection]
[ApiController]
[Route("api/GrowerDelivery")]
public sealed class GrowerDeliveryApiController : ControllerBase
{
    private readonly dbContext _ctx;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<GrowerDeliveryApiController> _logger;

    public GrowerDeliveryApiController(
        dbContext ctx,
        ICurrentUser currentUser,
        ILogger<GrowerDeliveryApiController> logger)
    {
        _ctx = ctx;
        _currentUser = currentUser;
        _logger = logger;
    }

    // POST /api/GrowerDelivery
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] GrowerDeliveryDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });

        // ── Required field validation ────────────────────────────────────────
        if (dto.LotId <= 0)
            return BadRequest(new { message = "LotId is required." });
        if (dto.ProductId <= 0)
            return BadRequest(new { message = "ProductId is required." });
        if (dto.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });

        // ── Resolve lbs UOM — always lbs for grower deliveries ───────────────
        var uomId = await _ctx.UnitOfMeasures
            .AsNoTracking()
            .Where(u => u.IsActive == true && u.Code == "LBS")
            .Select(u => u.UomId)
            .FirstOrDefaultAsync(ct);

        if (uomId == 0)
            return StatusCode(500, new { message = "LBS unit of measure is not configured in the database." });

        // ── Quantity: must have scale pair OR direct, not both ───────────────
        bool hasScale  = dto.StartQty.HasValue && dto.EndQty.HasValue;
        bool hasDirect = dto.DirectQty.HasValue;

        if (!hasScale && !hasDirect)
            return BadRequest(new { message = "Either gross/tare weights or a direct quantity is required." });
        if (hasScale && hasDirect)
            return BadRequest(new { message = "Provide scale weights OR a direct quantity, not both." });

        // ── Build the transaction ────────────────────────────────────────────
        var txn = new InventoryTransaction
        {
            LotId        = dto.LotId,
            ProductId    = dto.ProductId,
            ItemId       = dto.ItemId,
            TxnType      = "RECEIVE",
            Direction    = 1,
            UomId        = uomId,
            LocationId   = dto.LocationId,
            AccountId    = dto.AccountId,
            SplitGroupId = dto.SplitGroupId,
            ToContainerId  = dto.ToContainerId,
            StartQty     = dto.StartQty,
            EndQty       = dto.EndQty,
            DirectQty    = dto.DirectQty,
            StartedAt    = dto.StartedAt,
            CompletedAt  = dto.CompletedAt,
            RefType      = dto.RefType,
            RefId        = dto.RefId,
            Notes        = dto.Notes,
            TxnAt        = DateTime.UtcNow,
            CreatedAt    = DateTime.UtcNow,
            IsVoided     = false,
            // TODO: resolve UserId from _currentUser.UPN via users.Users table
            CreatedByUserId = null,
        };

        // ── Build grain quality TransactionAttributes ────────────────────────
        var attributes = BuildAttributes(dto);

        if (attributes.Count > 0)
        {
            // Load all active attribute type codes in a single query
            var codes = attributes.Select(a => a.Code).ToHashSet();
            var typeMap = await _ctx.TransactionAttributeTypes
                .AsNoTracking()
                .Where(t => t.IsActive == true && codes.Contains(t.Code))
                .ToDictionaryAsync(t => t.Code, t => t.Id, ct);

            foreach (var attr in attributes)
            {
                if (!typeMap.TryGetValue(attr.Code, out var typeId))
                {
                    _logger.LogWarning("TransactionAttributeType not found for code {Code}", attr.Code);
                    continue;
                }

                txn.TransactionAttributes.Add(new TransactionAttribute
                {
                    AttributeTypeId = typeId,
                    DecimalValue    = attr.DecimalValue,
                    StringValue     = attr.StringValue,
                    IntValue        = null,
                    BoolValue       = null,
                    CreatedAt       = DateTime.UtcNow,
                });
            }
        }

        _ctx.InventoryTransactions.Add(txn);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to save GrowerDelivery for LotId={LotId}", dto.LotId);
            return StatusCode(500, new { message = "Database error while saving delivery." });
        }

        // Link to weight sheet if one was selected
        if (dto.WeightSheetUid.HasValue)
        {
            _ctx.WeightSheetLoads.Add(new WeightSheetLoad
            {
                Id                     = Guid.NewGuid(),
                InventoryTransactionId = txn.Id,
                WeightSheetUid         = dto.WeightSheetUid.Value,
            });

            try { await _ctx.SaveChangesAsync(ct); }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Delivery {TxnId} saved but WeightSheetLoad link failed", txn.Id);
                // Non-fatal — delivery already committed
            }
        }

        return Ok(new { id = txn.Id });
    }

    // GET /api/GrowerDelivery/OpenWeightSheets?locationId=
    [HttpGet("OpenWeightSheets")]
    public async Task<IActionResult> GetOpenWeightSheets([FromQuery] int locationId, CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        var sheets = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(ws => ws.LocationId == locationId && ws.ClosedAt == null && ws.Status != "VOID")
            .Include(ws => ws.Lot)
            .OrderByDescending(ws => ws.CreatedAt)
            .Select(ws => new
            {
                ws.WeightSheetUid,
                ws.WeightSheetId,
                ws.SheetType,
                CreationDate   = ws.CreationDate.ToString("MM/dd/yyyy"),
                ws.Status,
                LotDescription = ws.Lot != null ? ws.Lot.LotDescription : null,
                ws.LotId,
            })
            .ToListAsync(ct);

        return Ok(sheets);
    }

    // GET /api/GrowerDelivery/OpenLots?locationId=
    // Returns open lots at this location that carry the WEIGHTSHEET system trait (TraitTypeId=12).
    [HttpGet("OpenLots")]
    public async Task<IActionResult> GetOpenLots([FromQuery] int locationId, CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        var lots = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LocationId == locationId && l.IsOpen &&
                        l.LotTraits.Any(t => t.TraitTypeId == 12))
            .Include(l => l.SplitGroup)
            .OrderBy(l => l.LotDescription)
            .Select(l => new
            {
                l.Id,
                l.LotDescription,
                SplitGroupDescription = l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null,
                l.SplitGroupId,
            })
            .ToListAsync(ct);

        return Ok(lots);
    }

    // GET /api/GrowerDelivery/SplitGroupsByAccount?accountId=
    // Returns active split groups where PrimaryAccountId matches the given account.
    [HttpGet("SplitGroupsByAccount")]
    public async Task<IActionResult> GetSplitGroupsByAccount([FromQuery] long accountId, CancellationToken ct)
    {
        if (accountId <= 0)
            return BadRequest(new { message = "accountId is required." });

        var groups = await _ctx.SplitGroups
            .AsNoTracking()
            .Where(sg => sg.PrimaryAccountId == accountId && sg.IsActive)
            .OrderBy(sg => sg.SplitGroupDescription)
            .Select(sg => new { sg.SplitGroupId, sg.SplitGroupDescription })
            .ToListAsync(ct);

        return Ok(groups);
    }

    // GET /api/GrowerDelivery/WeightSheetLots?locationId=
    // Returns ALL weight sheet lots (open + closed) at the location — management view.
    [HttpGet("WeightSheetLots")]
    public async Task<IActionResult> GetWeightSheetLots([FromQuery] int locationId, CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        var lots = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LocationId == locationId &&
                        l.LotTraits.Any(t => t.TraitTypeId == 12))
            .Include(l => l.SplitGroup)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new
            {
                l.Id,
                l.LotDescription,
                l.IsOpen,
                CreatedAt             = l.CreatedAt.ToString("MM/dd/yyyy"),
                l.SplitGroupId,
                SplitGroupDescription = l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null,
                AccountId             = l.SplitGroup != null ? l.SplitGroup.PrimaryAccountId : (long?)null,
                AccountName           = l.SplitGroup != null
                    ? _ctx.Accounts
                          .Where(a => a.AccountId == l.SplitGroup.PrimaryAccountId)
                          .Select(a => a.EntityName)
                          .FirstOrDefault()
                    : null,
                l.ItemId,
                ItemDescription       = l.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == l.ItemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                l.Notes,
                HasClosedWeightSheet  = l.WeightSheets.Any(ws => ws.ClosedAt != null),
                State  = l.LotTraits.Where(t => t.TraitTypeId == 15).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                County = l.LotTraits.Where(t => t.TraitTypeId == 16).Select(t => t.Trait.TraitCode).FirstOrDefault(),
            })
            .ToListAsync(ct);

        return Ok(lots);
    }

    // POST /api/GrowerDelivery/WeightSheetLots/{id}/close  — marks the lot as closed (IsOpen = false)
    [HttpPost("WeightSheetLots/{id:long}/close")]
    public async Task<IActionResult> CloseLot(long id, CancellationToken ct)
    {
        var lot = await _ctx.Lots.FindAsync(new object[] { id }, ct);
        if (lot is null) return NotFound(new { message = "Lot not found." });

        lot.IsOpen    = false;
        lot.UpdatedAt = DateTime.UtcNow;

        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to close lot {LotId}", id);
            return StatusCode(500, new { message = "Database error while closing lot." });
        }

        return Ok(new { lot.Id, lot.IsOpen });
    }

    // POST /api/GrowerDelivery/WeightSheetLots/{id}/open  — re-opens a closed lot
    [HttpPost("WeightSheetLots/{id:long}/open")]
    public async Task<IActionResult> OpenLot(long id, CancellationToken ct)
    {
        var lot = await _ctx.Lots.FindAsync(new object[] { id }, ct);
        if (lot is null) return NotFound(new { message = "Lot not found." });

        lot.IsOpen    = true;
        lot.UpdatedAt = DateTime.UtcNow;

        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to re-open lot {LotId}", id);
            return StatusCode(500, new { message = "Database error while re-opening lot." });
        }

        return Ok(new { lot.Id, lot.IsOpen });
    }

    // POST /api/GrowerDelivery/WeightSheetLots
    // Creates a new weight sheet lot: Lot + WAREHOUSE LotTrait + LotSplitGroup rows from the split group percents.
    [HttpPost("WeightSheetLots")]
    public async Task<IActionResult> CreateWeightSheetLot(
        [FromBody] CreateWeightSheetLotDto dto,
        CancellationToken ct)
    {
        if (dto.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });
        if (dto.SplitGroupId <= 0)
            return BadRequest(new { message = "SplitGroupId is required." });

        // Load split group + percents in a single query
        var sg = await _ctx.SplitGroups
            .AsNoTracking()
            .Include(s => s.SplitGroupPercents)
            .FirstOrDefaultAsync(s => s.SplitGroupId == dto.SplitGroupId, ct);

        if (sg is null)
            return NotFound(new { message = "Split group not found." });

        // Resolve ProductId from ItemId when provided
        int? productId = null;
        if (dto.ItemId.HasValue)
        {
            productId = await _ctx.Items
                .AsNoTracking()
                .Where(i => i.ItemId == dto.ItemId.Value)
                .Select(i => (int?)i.ProductId)
                .FirstOrDefaultAsync(ct);
        }

        // Resolve the WAREHOUSE trait (TraitTypeId=12 = SYSTEM_USAGE, TraitCode="WAREHOUSE")
        var wsTrait = await _ctx.Traits
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TraitTypeId == 12 && t.TraitCode == "WAREHOUSE", ct);

        if (wsTrait is null)
            return StatusCode(500, new { message = "WAREHOUSE trait (TraitTypeId=12) is not configured in the database." });

        // Get next distributed Id for the Lot
        var lotId = await _ctx.NextLotIdAsync();
        if (!lotId.HasValue)
            return StatusCode(500, new { message = "Failed to generate Lot Id." });

        // Create the lot; add the LotTrait via navigation so EF wires LotId automatically
        var lot = new Lot
        {
            Id             = lotId.Value,
            RowGuid        = Guid.NewGuid(),
            LocationId     = dto.LocationId,
            SplitGroupId   = dto.SplitGroupId,
            LotDescription = sg.SplitGroupDescription,
            ItemId         = dto.ItemId,
            ProductId      = productId,
            Notes          = dto.Notes?.Trim(),
            IsOpen         = true,
            CreatedAt      = DateTime.UtcNow,
        };

        // Insert Lot + LotTrait using interpolated SQL to avoid EF navigation-fixup issues
        // with the one-to-one Lot ↔ LotSplitGroup mapping (DB actually allows many rows).
        try
        {
            var now = DateTime.UtcNow;
            string lotDesc = sg.SplitGroupDescription;
            long? itemId = dto.ItemId;
            string notes = lot.Notes;

            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [Inventory].[Lots] (Id, RowGuid, LocationId, SplitGroupId, LotDescription, ItemId, ProductId, Notes, IsOpen, CreatedAt)
                VALUES ({lot.Id}, {lot.RowGuid}, {dto.LocationId}, {dto.SplitGroupId}, {lotDesc}, {itemId}, {productId}, {notes}, 1, {now})",
                ct);

            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [Inventory].[LotTraits] (LotId, TraitId, TraitTypeId, IsExclusive, CreatedAt)
                VALUES ({lot.Id}, {wsTrait.TraitId}, {12}, 1, {now})",
                ct);

            // Insert State trait (TraitTypeId=15) if provided
            if (!string.IsNullOrWhiteSpace(dto.State))
            {
                var stateTrait = await _ctx.Traits.AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TraitTypeId == 15 && t.TraitCode == dto.State.Trim(), ct);
                if (stateTrait != null)
                {
                    await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                        INSERT INTO [Inventory].[LotTraits] (LotId, TraitId, TraitTypeId, IsExclusive, CreatedAt)
                        VALUES ({lot.Id}, {stateTrait.TraitId}, {15}, 1, {now})", ct);
                }
            }

            // Insert County trait (TraitTypeId=16) if provided
            if (!string.IsNullOrWhiteSpace(dto.County))
            {
                var countyTrait = await _ctx.Traits.AsNoTracking()
                    .FirstOrDefaultAsync(t => t.TraitTypeId == 16 && t.TraitCode == dto.County.Trim(), ct);
                if (countyTrait != null)
                {
                    await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                        INSERT INTO [Inventory].[LotTraits] (LotId, TraitId, TraitTypeId, IsExclusive, CreatedAt)
                        VALUES ({lot.Id}, {countyTrait.TraitId}, {16}, 1, {now})", ct);
                }
            }

            foreach (var p in sg.SplitGroupPercents)
            {
                var rowGuid = Guid.NewGuid();
                var isPrimary = p.AccountId == sg.PrimaryAccountId;
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    INSERT INTO [Inventory].[LotSplitGroups] (RowGuid, LotId, AccountId, SplitPercent, PrimaryAccount)
                    VALUES ({rowGuid}, {lot.Id}, {p.AccountId}, {p.SplitPercent}, {isPrimary})",
                    ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create WeightSheetLot for LocationId={LocationId}", dto.LocationId);
            return StatusCode(500, new { message = "Database error while creating weight sheet lot." });
        }

        return Ok(new { lot.Id, lot.LotDescription });
    }

    // PATCH /api/GrowerDelivery/WeightSheetLots/{id}
    // Updates an existing lot. If the lot has a closed weight sheet, only Notes can be changed.
    // If the lot itself is closed (and no closed WS), only Notes can be changed.
    [HttpPatch("WeightSheetLots/{id:long}")]
    public async Task<IActionResult> UpdateWeightSheetLot(
        long id,
        [FromBody] UpdateWeightSheetLotDto dto,
        CancellationToken ct)
    {
        var lot = await _ctx.Lots.FindAsync(new object[] { id }, ct);
        if (lot is null)
            return NotFound(new { message = "Lot not found." });

        var hasClosedWs = await _ctx.WeightSheets
            .AsNoTracking()
            .AnyAsync(ws => ws.LotId == id && ws.ClosedAt != null, ct);

        bool notesOnly = !lot.IsOpen || hasClosedWs;

        var now = DateTime.UtcNow;

        if (notesOnly)
        {
            // Only update Notes
            string notes = dto.Notes?.Trim();
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE [Inventory].[Lots]
                SET Notes = {notes}, UpdatedAt = {now}
                WHERE Id = {id}", ct);
        }
        else
        {
            // Full update: LotDescription, ItemId, ProductId, Notes
            int? productId = null;
            if (dto.ItemId.HasValue)
            {
                productId = await _ctx.Items
                    .AsNoTracking()
                    .Where(i => i.ItemId == dto.ItemId.Value)
                    .Select(i => (int?)i.ProductId)
                    .FirstOrDefaultAsync(ct);
            }

            string lotDesc = dto.LotDescription?.Trim();
            long? itemId = dto.ItemId;
            string notes = dto.Notes?.Trim();

            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE [Inventory].[Lots]
                SET LotDescription = {lotDesc}, ItemId = {itemId}, ProductId = {productId},
                    Notes = {notes}, UpdatedAt = {now}
                WHERE Id = {id}", ct);

            // Upsert State trait (TraitTypeId=15)
            await UpsertLotTrait(id, 15, dto.State?.Trim(), now, ct);

            // Upsert County trait (TraitTypeId=16)
            await UpsertLotTrait(id, 16, dto.County?.Trim(), now, ct);
        }

        return Ok(new { id, notesOnly });
    }

    // POST /api/GrowerDelivery/WeightSheets
    [HttpPost("WeightSheets")]
    public async Task<IActionResult> CreateWeightSheet(
        [FromBody] CreateWeightSheetDto dto,
        CancellationToken ct)
    {
        if (dto.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });

        var ws = new WeightSheet
        {
            WeightSheetUid = Guid.NewGuid(),
            LocationId     = dto.LocationId,
            LotId          = dto.LotId,
            SheetType      = "INTAKE",
            CreationDate   = DateOnly.FromDateTime(DateTime.Now),
            CreatedAt      = DateTime.UtcNow,
            Status         = "OPEN",
        };

        _ctx.WeightSheets.Add(ws);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create WeightSheet for LocationId={LocationId}", dto.LocationId);
            return StatusCode(500, new { message = "Database error while creating weight sheet." });
        }

        return Ok(new { ws.WeightSheetUid, ws.WeightSheetId });
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private async Task UpsertLotTrait(long lotId, int traitTypeId, string traitCode, DateTime now, CancellationToken ct)
    {
        // Delete existing trait of this type for the lot
        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM [Inventory].[LotTraits]
            WHERE LotId = {lotId} AND TraitTypeId = {traitTypeId}", ct);

        if (string.IsNullOrWhiteSpace(traitCode)) return;

        var trait = await _ctx.Traits.AsNoTracking()
            .FirstOrDefaultAsync(t => t.TraitTypeId == traitTypeId && t.TraitCode == traitCode, ct);

        if (trait == null) return;

        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [Inventory].[LotTraits] (LotId, TraitId, TraitTypeId, IsExclusive, CreatedAt)
            VALUES ({lotId}, {trait.TraitId}, {traitTypeId}, 1, {now})", ct);
    }

    private record AttributeEntry(string Code, decimal? DecimalValue, string StringValue);

    private static List<AttributeEntry> BuildAttributes(GrowerDeliveryDto dto)
    {
        var list = new List<AttributeEntry>();

        void AddDecimal(string code, decimal? value)
        {
            if (value.HasValue)
                list.Add(new AttributeEntry(code, value, null));
        }

        void AddString(string code, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                list.Add(new AttributeEntry(code, null, value.Trim()));
        }

        AddDecimal("MOISTURE",      dto.Moisture);
        AddDecimal("PROTEIN",       dto.Protein);
        AddDecimal("OIL",           dto.Oil);
        AddDecimal("STARCH",        dto.Starch);
        AddDecimal("TEST_WEIGHT",   dto.TestWeight);
        AddDecimal("DOCKAGE",       dto.Dockage);
        AddDecimal("FOREIGN_MATTER",dto.ForeignMatter);
        AddDecimal("SPLITS",        dto.Splits);
        AddDecimal("DAMAGED",       dto.Damaged);
        AddString ("GRADE",         dto.Grade);

        return list;
    }
}
