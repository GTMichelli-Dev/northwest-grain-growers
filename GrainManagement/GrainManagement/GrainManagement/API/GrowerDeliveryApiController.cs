using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace GrainManagement.API;

[UseAdminConnection]
[ApiController]
[Route("api/GrowerDelivery")]
public sealed class GrowerDeliveryApiController : ControllerBase
{
    private readonly dbContext _ctx;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<GrowerDeliveryApiController> _logger;
    private readonly SystemInfoSnapshot _systemInfo;

    public GrowerDeliveryApiController(
        dbContext ctx,
        ICurrentUser currentUser,
        ILogger<GrowerDeliveryApiController> logger,
        SystemInfoSnapshot systemInfo)
    {
        _ctx = ctx;
        _currentUser = currentUser;
        _logger = logger;
        _systemInfo = systemInfo;
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

        // ── Build the transaction (header + detail) ────────────────────────────
        var txn = new InventoryTransaction
        {
            LocationId   = dto.LocationId,
            CreatedAt    = DateTime.UtcNow,
        };

        txn.InventoryTransactionDetail = new InventoryTransactionDetail
        {
            LotId        = dto.LotId,
            ProductId    = dto.ProductId,
            ItemId       = dto.ItemId,
            TxnType      = "RECEIVE",
            Direction    = 1,
            UomId        = uomId,
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
            IsVoided     = false,
            // TODO: resolve UserId from _currentUser.UPN via users.Users table
            CreatedByUserId = null,
        };

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

        // ── Build grain quality TransactionAttributes via raw SQL (keyless entity) ──
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

                var now = DateTime.UtcNow;
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    INSERT INTO [Inventory].[TransactionAttributes]
                        (TransactionId, AttributeTypeId, DecimalValue, StringValue, IntValue, BoolValue, CreatedAt)
                    VALUES ({txn.TransactionId}, {typeId}, {attr.DecimalValue}, {attr.StringValue}, {(int?)null}, {(bool?)null}, {now})",
                    ct);
            }
        }

        // TODO: WeightSheetLoad linking needs rework — WeightSheetLoad.InventoryTransactionId
        // is Guid but InventoryTransaction.TransactionId is now long. Schema mismatch after DB refactor.
        // if (dto.WeightSheetUid.HasValue) { ... }

        return Ok(new { id = txn.TransactionId });
    }

    // GET /api/GrowerDelivery/OpenWeightSheets?locationId=
    [HttpGet("OpenWeightSheets")]
    public async Task<IActionResult> GetOpenWeightSheets([FromQuery] int locationId, CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        var sheets = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(ws => ws.LocationId == locationId && ws.ServerId == _systemInfo.ServerId && ws.ClosedAt == null)
            .OrderByDescending(ws => ws.CreatedAt)
            .Select(ws => new
            {
                ws.WeightSheetId,
                ws.WeightSheetType,
                CreationDate   = ws.CreationDate.ToString("MM/dd/yyyy"),
                IsOpen         = ws.ClosedAt == null,
                LotDescription = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.LotDescription).FirstOrDefault()
                    : null,
                ws.LotId,
                SplitGroupDescription = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId)
                        .Select(l => l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null)
                        .FirstOrDefault()
                    : null,
                AccountName = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId && l.SplitGroup != null)
                        .Select(l => _ctx.Accounts
                            .Where(a => a.AccountId == l.SplitGroup.PrimaryAccountId)
                            .Select(a => a.LookupName).FirstOrDefault())
                        .FirstOrDefault()
                    : null,
                ItemDescription = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId && l.ItemId != null)
                        .Select(l => _ctx.Items
                            .Where(i => i.ItemId == l.ItemId)
                            .Select(i => i.Description).FirstOrDefault())
                        .FirstOrDefault()
                    : null,
                ws.RateType,
                ws.CustomRateDescription,
                HaulerName = ws.HaulerId != null
                    ? _ctx.Haulers.Where(h => h.Id == ws.HaulerId).Select(h => h.Description).FirstOrDefault()
                    : null,
                LoadCount = _ctx.WeightSheetLoads.Count(wsl => wsl.WeightSheetUid == ws.RowUid),
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
                        l.ServerId == _systemInfo.ServerId &&
                        l.LotTraits.Any(t => t.TraitTypeId == 12))
            .Include(l => l.SplitGroup)
            .OrderBy(l => l.LotDescription)
            .Select(l => new
            {
                l.LotId,
                l.ServerId,
                l.LocationId,
                l.LotDescription,
                l.SplitGroupId,
                SplitGroupDescription = l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null,
                AccountName           = l.SplitGroup != null
                    ? _ctx.Accounts
                          .Where(a => a.AccountId == l.SplitGroup.PrimaryAccountId)
                          .Select(a => a.LookupName)
                          .FirstOrDefault()
                    : null,
                l.ItemId,
                ItemDescription       = l.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == l.ItemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                CropName              = l.Product != null ? l.Product.Description : null,
                State    = l.LotTraits.Where(t => t.TraitTypeId == 15).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                County   = l.LotTraits.Where(t => t.TraitTypeId == 16).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                Landlord = l.LotTraits.Where(t => t.TraitTypeId == 18).Select(t => t.Trait.TraitCode).FirstOrDefault(),
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

    // GET /api/GrowerDelivery/SplitGroupLookup?splitGroupId=
    // Returns basic info for a single split group by ID (must be active).
    [HttpGet("SplitGroupLookup")]
    public async Task<IActionResult> SplitGroupLookup([FromQuery] int splitGroupId, CancellationToken ct)
    {
        if (splitGroupId <= 0)
            return BadRequest(new { message = "splitGroupId is required." });

        var sg = await _ctx.SplitGroups
            .AsNoTracking()
            .Where(s => s.SplitGroupId == splitGroupId && s.IsActive)
            .Select(s => new { s.SplitGroupId, s.SplitGroupDescription, s.PrimaryAccountId })
            .FirstOrDefaultAsync(ct);

        if (sg is null)
            return NotFound(new { message = "Split group not found or inactive." });

        return Ok(sg);
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
                        l.ServerId == _systemInfo.ServerId &&
                        l.LotTraits.Any(t => t.TraitTypeId == 12))
            .Include(l => l.SplitGroup)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new
            {
                l.LotId,
                l.As400Id,
                l.LotDescription,
                l.IsOpen,
                CreatedAt             = l.CreatedAt.ToString("MM/dd/yyyy"),
                l.SplitGroupId,
                SplitGroupDescription = l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null,
                AccountId             = l.SplitGroup != null ? l.SplitGroup.PrimaryAccountId : (long?)null,
                AccountAs400Id        = l.SplitGroup != null
                    ? _ctx.Accounts
                          .Where(a => a.AccountId == l.SplitGroup.PrimaryAccountId)
                          .Select(a => (long?)a.As400AccountId)
                          .FirstOrDefault()
                    : (long?)null,
                AccountName           = l.SplitGroup != null
                    ? _ctx.Accounts
                          .Where(a => a.AccountId == l.SplitGroup.PrimaryAccountId)
                          .Select(a => a.LookupName)
                          .FirstOrDefault()
                    : null,
                l.ItemId,
                ItemDescription       = l.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == l.ItemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                l.Notes,
                HasClosedWeightSheet  = _ctx.WeightSheets.Any(ws => ws.LotId == l.LotId && ws.ClosedAt != null),
                State    = l.LotTraits.Where(t => t.TraitTypeId == 15).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                County   = l.LotTraits.Where(t => t.TraitTypeId == 16).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                Landlord = l.LotTraits.Where(t => t.TraitTypeId == 18).Select(t => t.Trait.TraitCode).FirstOrDefault(),
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

        return Ok(new { lot.LotId, lot.IsOpen });
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

        return Ok(new { lot.LotId, lot.IsOpen });
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

        // Insert the lot via raw SQL — trigger trg_Lots_AutoGenerateIDs generates LotId, BaseId, ServerId, As400Id
        try
        {
            var now = DateTime.UtcNow;
            string lotDesc = sg.SplitGroupDescription;
            long? itemId = dto.ItemId;
            string notes = dto.Notes?.Trim();

            // INSTEAD OF INSERT trigger generates LotId — pass RowUid so we can find the row back
            var rowUid = Guid.NewGuid();
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [Inventory].[Lots] (LocationId, SplitGroupId, LotDescription, ItemId, ProductId, Notes, IsOpen, CreatedAt, RowUid)
                VALUES ({dto.LocationId}, {dto.SplitGroupId}, {lotDesc}, {itemId}, {productId}, {notes}, 1, {now}, {rowUid})",
                ct);

            // Retrieve the trigger-generated LotId via the known RowUid
            var conn = (SqlConnection)_ctx.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync(ct);

            long lotId;
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
                cmd.CommandText = "SELECT LotId FROM [Inventory].[Lots] WHERE RowUid = @uid";
                cmd.Parameters.AddWithValue("@uid", rowUid);
                var result = await cmd.ExecuteScalarAsync(ct)
                    ?? throw new InvalidOperationException("INSTEAD OF trigger did not produce a LotId row.");
                lotId = (long)result;
            }

            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [Inventory].[LotTraits] (LotId, TraitId, TraitTypeId, IsExclusive, CreatedAt)
                VALUES ({lotId}, {wsTrait.TraitId}, {12}, 1, {now})",
                ct);

            // Insert State trait (TraitTypeId=15) if provided
            if (!string.IsNullOrWhiteSpace(dto.State))
            {
                var stateTrait = await _ctx.Traits
                    .FirstOrDefaultAsync(t => t.TraitTypeId == 15 && t.TraitCode == dto.State.Trim(), ct);
                if (stateTrait == null)
                {
                    stateTrait = new Trait { TraitCode = dto.State.Trim(), TraitTypeId = 15, Description = dto.State.Trim(), IsActive = true, CreatedAt = now };
                    _ctx.Traits.Add(stateTrait);
                    await _ctx.SaveChangesAsync(ct);
                }
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    INSERT INTO [Inventory].[LotTraits] (LotId, TraitId, TraitTypeId, IsExclusive, CreatedAt)
                    VALUES ({lotId}, {stateTrait.TraitId}, {15}, 1, {now})", ct);
            }

            // Insert County trait (TraitTypeId=16) if provided
            if (!string.IsNullOrWhiteSpace(dto.County))
            {
                var countyTrait = await _ctx.Traits
                    .FirstOrDefaultAsync(t => t.TraitTypeId == 16 && t.TraitCode == dto.County.Trim(), ct);
                if (countyTrait == null)
                {
                    countyTrait = new Trait { TraitCode = dto.County.Trim(), TraitTypeId = 16, Description = dto.County.Trim(), IsActive = true, CreatedAt = now };
                    _ctx.Traits.Add(countyTrait);
                    await _ctx.SaveChangesAsync(ct);
                }
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    INSERT INTO [Inventory].[LotTraits] (LotId, TraitId, TraitTypeId, IsExclusive, CreatedAt)
                    VALUES ({lotId}, {countyTrait.TraitId}, {16}, 1, {now})", ct);
            }

            // Insert Landlord trait (TraitTypeId=18) if provided
            if (!string.IsNullOrWhiteSpace(dto.Landlord))
            {
                var landlordTrait = await _ctx.Traits
                    .FirstOrDefaultAsync(t => t.TraitTypeId == 18 && t.TraitCode == dto.Landlord.Trim(), ct);
                if (landlordTrait == null)
                {
                    landlordTrait = new Trait { TraitCode = dto.Landlord.Trim(), TraitTypeId = 18, Description = dto.Landlord.Trim(), IsActive = true, CreatedAt = now };
                    _ctx.Traits.Add(landlordTrait);
                    await _ctx.SaveChangesAsync(ct);
                }
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    INSERT INTO [Inventory].[LotTraits] (LotId, TraitId, TraitTypeId, IsExclusive, CreatedAt)
                    VALUES ({lotId}, {landlordTrait.TraitId}, {18}, 1, {now})", ct);
            }

            foreach (var p in sg.SplitGroupPercents)
            {
                var rowGuid = Guid.NewGuid();
                var isPrimary = p.AccountId == sg.PrimaryAccountId;
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    INSERT INTO [Inventory].[LotSplitGroups] (RowGuid, LotId, AccountId, SplitPercent, PrimaryAccount)
                    VALUES ({rowGuid}, {lotId}, {p.AccountId}, {p.SplitPercent}, {isPrimary})",
                    ct);
            }

            return Ok(new { LotId = lotId, LotDescription = lotDesc });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create WeightSheetLot for LocationId={LocationId}", dto.LocationId);
            return StatusCode(500, new { message = "Database error while creating weight sheet lot." });
        }
    }

    // PATCH /api/GrowerDelivery/WeightSheetLots/{id}
    // Updates an existing lot. Requires a valid user PIN for audit trail.
    // If the lot has a closed weight sheet, only Notes can be changed.
    // If the lot itself is closed (and no closed WS), only Notes can be changed.
    [HttpPatch("WeightSheetLots/{id:long}")]
    public async Task<IActionResult> UpdateWeightSheetLot(
        long id,
        [FromBody] UpdateWeightSheetLotDto dto,
        CancellationToken ct)
    {
        // ── PIN validation ──────────────────────────────────────────────────
        if (!dto.Pin.HasValue || dto.Pin.Value <= 0)
            return BadRequest(new { message = "PIN is required." });

        var pinUser = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin.Value && u.IsActive, ct);

        if (pinUser is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        // ── Lot lookup ──────────────────────────────────────────────────────
        var lot = await _ctx.Lots.FindAsync(new object[] { id }, ct);
        if (lot is null)
            return NotFound(new { message = "Lot not found." });

        var hasClosedWs = await _ctx.WeightSheets
            .AsNoTracking()
            .AnyAsync(ws => ws.LotId == id && ws.ClosedAt != null, ct);

        // Allow full edit if created today, even when closed
        bool createdToday = lot.CreatedAt.Date == DateTime.UtcNow.Date;
        bool notesOnly = (!lot.IsOpen || hasClosedWs) && !createdToday;

        // ── Snapshot old values for audit ────────────────────────────────────
        var oldValues = new
        {
            lot.LotDescription,
            lot.ItemId,
            lot.ProductId,
            lot.Notes,
            State    = await _ctx.LotTraits.Where(t => t.LotId == id && t.TraitTypeId == 15).Select(t => t.Trait.TraitCode).FirstOrDefaultAsync(ct),
            County   = await _ctx.LotTraits.Where(t => t.LotId == id && t.TraitTypeId == 16).Select(t => t.Trait.TraitCode).FirstOrDefaultAsync(ct),
            Landlord = await _ctx.LotTraits.Where(t => t.LotId == id && t.TraitTypeId == 18).Select(t => t.Trait.TraitCode).FirstOrDefaultAsync(ct),
        };
        string oldJson = JsonSerializer.Serialize(oldValues);

        var now = DateTime.UtcNow;

        if (notesOnly)
        {
            // Only update Notes
            string notes = dto.Notes?.Trim();
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE [Inventory].[Lots]
                SET Notes = {notes}, UpdatedAt = {now}
                WHERE LotId = {id}", ct);
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
                WHERE LotId = {id}", ct);

            // Upsert State trait (TraitTypeId=15)
            await UpsertLotTrait(id, 15, dto.State?.Trim(), now, ct);

            // Upsert County trait (TraitTypeId=16)
            await UpsertLotTrait(id, 16, dto.County?.Trim(), now, ct);

            // Upsert Landlord trait (TraitTypeId=18)
            await UpsertLotTrait(id, 18, dto.Landlord?.Trim(), now, ct);
        }

        // ── Audit trail ─────────────────────────────────────────────────────
        var newValues = new
        {
            LotDescription = notesOnly ? oldValues.LotDescription : dto.LotDescription?.Trim(),
            ItemId         = notesOnly ? oldValues.ItemId : dto.ItemId,
            ProductId      = notesOnly ? oldValues.ProductId : (await _ctx.Items.AsNoTracking().Where(i => i.ItemId == dto.ItemId).Select(i => (int?)i.ProductId).FirstOrDefaultAsync(ct)),
            Notes          = dto.Notes?.Trim(),
            State          = notesOnly ? oldValues.State : dto.State?.Trim(),
            County         = notesOnly ? oldValues.County : dto.County?.Trim(),
            Landlord       = notesOnly ? oldValues.Landlord : dto.Landlord?.Trim(),
        };
        string newJson = JsonSerializer.Serialize(newValues);
        string keyJson = JsonSerializer.Serialize(new { LotId = id });
        string tableName = "Inventory.Lots";
        string action = "EDIT";
        string userName = pinUser.UserName;
        int locationId = lot.LocationId;

        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
            VALUES ({locationId}, {userName}, {tableName}, {action}, {keyJson}, {oldJson}, {newJson})",
            ct);

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

        // WeightSheet is a keyless entity — must use raw SQL for inserts
        try
        {
            var now = DateTime.UtcNow;
            var creationDate = DateOnly.FromDateTime(DateTime.Now);
            string sheetType = "Delivery"; // CK_WeightSheetsType allows 'Delivery' or 'Transfer'

            // INSTEAD OF INSERT trigger generates WeightSheetId — pass RowUid so we can find the row back
            var wsRowUid = Guid.NewGuid();
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [warehouse].[WeightSheets]
                    (LocationId, LotId, WeightSheetType, RateType, HaulerId, Miles, CustomRateDescription, Rate, CreationDate, CreatedAt, RowUid)
                VALUES
                    ({dto.LocationId}, {dto.LotId}, {sheetType}, {dto.RateType}, {dto.HaulerId}, {dto.Miles}, {dto.CustomRateDescription}, {dto.Rate}, {creationDate}, {now}, {wsRowUid})",
                ct);

            // Retrieve the trigger-generated WeightSheetId via the known RowUid
            var conn = (SqlConnection)_ctx.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync(ct);

            long wsId;
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
                cmd.CommandText = "SELECT WeightSheetId FROM [warehouse].[WeightSheets] WHERE RowUid = @uid";
                cmd.Parameters.AddWithValue("@uid", wsRowUid);
                var result = await cmd.ExecuteScalarAsync(ct)
                    ?? throw new InvalidOperationException("INSTEAD OF trigger did not produce a WeightSheetId row.");
                wsId = (long)result;
            }

            return Ok(new { WeightSheetId = wsId });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create WeightSheet for LocationId={LocationId}", dto.LocationId);
            return StatusCode(500, new { message = "Database error while creating weight sheet." });
        }
    }

    // GET /api/GrowerDelivery/WeightSheetDeliveryLoads?locationId=
    [HttpGet("WeightSheetDeliveryLoads")]
    public async Task<IActionResult> GetWeightSheetDeliveryLoads([FromQuery] int locationId, [FromQuery] long wsId = 0, CancellationToken ct = default)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync(ct);

        var sql = @"
            SELECT
                ws.WeightSheetId,
                ws.WeightSheetType,
                ws.CreationDate,
                ws.ClosedAt,
                ws.ServerId,
                ws.LocationId,
                ws.Notes                AS WsNotes,
                ws.HaulerId,
                h.Description           AS HaulerName,
                wsl.Id                  AS LoadId,
                itd.TransactionId,
                itd.StartQty            AS InWeight,
                itd.EndQty              AS OutWeight,
                itd.NetQty              AS Net,
                itd.TxnAt,
                itd.Notes,
                c.Description           AS ContainerDescription,
                attr1.DecimalValue      AS Attr1Value,
                attr1.StringValue       AS Attr1String,
                at1.Description         AS Attr1Description,
                attr2.DecimalValue      AS Attr2Value,
                attr2.StringValue       AS Attr2String,
                at2.Description         AS Attr2Description,
                lot.LotDescription,
                lot.LotId
            FROM [warehouse].[WeightSheets] ws
            LEFT JOIN [account].[Haulers] h
                ON h.Id = ws.HaulerId
            INNER JOIN [warehouse].[WeightSheetLoads] wsl
                ON wsl.WeightSheetUid = ws.RowUid
            INNER JOIN [Inventory].[InventoryTransactionDetails] itd
                ON itd.RefId = wsl.Id
            LEFT JOIN [container].[Containers] c
                ON c.ContainerId = itd.ToContainerId
            LEFT JOIN [Inventory].[TransactionAttributes] attr1
                ON attr1.TransactionId = itd.TransactionId AND attr1.AttributeTypeId = 1
            LEFT JOIN [Inventory].[TransactionAttributeTypes] at1
                ON at1.Id = 1
            LEFT JOIN [Inventory].[TransactionAttributes] attr2
                ON attr2.TransactionId = itd.TransactionId AND attr2.AttributeTypeId = 2
            LEFT JOIN [Inventory].[TransactionAttributeTypes] at2
                ON at2.Id = 2
            LEFT JOIN [Inventory].[Lots] lot
                ON lot.LotId = itd.LotId
            WHERE ws.LocationId = @locationId
              AND ws.ServerId   = @serverId
              AND ws.WeightSheetType = 'Delivery'
              AND (@wsId = 0 OR ws.WeightSheetId = @wsId)
            ORDER BY ws.WeightSheetId DESC, itd.TxnAt DESC";

        using var cmd = conn.CreateCommand();
        cmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("@locationId", locationId);
        cmd.Parameters.AddWithValue("@serverId", _systemInfo.ServerId);
        cmd.Parameters.AddWithValue("@wsId", wsId);

        var results = new List<object>();
        using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct))
        {
            results.Add(new
            {
                WeightSheetId          = reader.GetInt64(reader.GetOrdinal("WeightSheetId")),
                WeightSheetType        = reader.IsDBNull(reader.GetOrdinal("WeightSheetType")) ? null : reader.GetString(reader.GetOrdinal("WeightSheetType")),
                CreationDate           = reader.GetDateTime(reader.GetOrdinal("CreationDate")).ToString("MM/dd/yyyy"),
                ClosedAt               = reader.IsDBNull(reader.GetOrdinal("ClosedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ClosedAt")),
                ServerId               = reader.GetInt32(reader.GetOrdinal("ServerId")),
                LocationId             = reader.GetInt32(reader.GetOrdinal("LocationId")),
                WsNotes                = reader.IsDBNull(reader.GetOrdinal("WsNotes")) ? null : reader.GetString(reader.GetOrdinal("WsNotes")),
                HaulerId               = reader.IsDBNull(reader.GetOrdinal("HaulerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("HaulerId")),
                HaulerName             = reader.IsDBNull(reader.GetOrdinal("HaulerName")) ? null : reader.GetString(reader.GetOrdinal("HaulerName")),
                LoadId                 = reader.GetGuid(reader.GetOrdinal("LoadId")),
                TransactionId          = reader.GetInt64(reader.GetOrdinal("TransactionId")),
                InWeight               = reader.IsDBNull(reader.GetOrdinal("InWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("InWeight")),
                OutWeight              = reader.IsDBNull(reader.GetOrdinal("OutWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("OutWeight")),
                Net                    = reader.IsDBNull(reader.GetOrdinal("Net")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Net")),
                TxnAt                  = reader.GetDateTime(reader.GetOrdinal("TxnAt")),
                Notes                  = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                ContainerDescription   = reader.IsDBNull(reader.GetOrdinal("ContainerDescription")) ? null : reader.GetString(reader.GetOrdinal("ContainerDescription")),
                Attr1Value             = reader.IsDBNull(reader.GetOrdinal("Attr1Value")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Attr1Value")),
                Attr1String            = reader.IsDBNull(reader.GetOrdinal("Attr1String")) ? null : reader.GetString(reader.GetOrdinal("Attr1String")),
                Attr1Description       = reader.IsDBNull(reader.GetOrdinal("Attr1Description")) ? null : reader.GetString(reader.GetOrdinal("Attr1Description")),
                Attr2Value             = reader.IsDBNull(reader.GetOrdinal("Attr2Value")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Attr2Value")),
                Attr2String            = reader.IsDBNull(reader.GetOrdinal("Attr2String")) ? null : reader.GetString(reader.GetOrdinal("Attr2String")),
                Attr2Description       = reader.IsDBNull(reader.GetOrdinal("Attr2Description")) ? null : reader.GetString(reader.GetOrdinal("Attr2Description")),
                LotDescription         = reader.IsDBNull(reader.GetOrdinal("LotDescription")) ? null : reader.GetString(reader.GetOrdinal("LotDescription")),
                LotId                  = reader.IsDBNull(reader.GetOrdinal("LotId")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("LotId")),
            });
        }

        return Ok(results);
    }

    // POST /api/GrowerDelivery/WeightSheetDeliveryLoads/UpdateAttribute
    [HttpPost("WeightSheetDeliveryLoads/UpdateAttribute")]
    public async Task<IActionResult> UpdateDeliveryLoadAttribute(
        [FromBody] UpdateTransactionAttributeDto dto,
        CancellationToken ct)
    {
        if (dto.TransactionId <= 0)
            return BadRequest(new { message = "TransactionId is required." });
        if (dto.AttributeTypeId <= 0)
            return BadRequest(new { message = "AttributeTypeId is required." });
        if (dto.DecimalValue.HasValue && dto.DecimalValue.Value <= 0)
            return BadRequest(new { message = "Decimal value must be greater than zero." });

        var now = DateTime.UtcNow;
        var hasDecimal = dto.DecimalValue.HasValue;
        var hasString  = !string.IsNullOrWhiteSpace(dto.StringValue);
        var hasValue   = hasDecimal || hasString;
        var strVal     = hasString ? dto.StringValue.Trim() : (string)null;

        // Check if attribute already exists
        var existing = await _ctx.TransactionAttributes
            .FirstOrDefaultAsync(a => a.TransactionId == dto.TransactionId && a.AttributeTypeId == dto.AttributeTypeId, ct);

        if (existing != null)
        {
            if (hasValue)
            {
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE [Inventory].[TransactionAttributes]
                    SET DecimalValue = {dto.DecimalValue}, StringValue = {strVal}, UpdatedAt = {now}
                    WHERE TransactionId = {dto.TransactionId} AND AttributeTypeId = {dto.AttributeTypeId}", ct);
            }
            else
            {
                // Remove if no value submitted
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    DELETE FROM [Inventory].[TransactionAttributes]
                    WHERE TransactionId = {dto.TransactionId} AND AttributeTypeId = {dto.AttributeTypeId}", ct);
            }
        }
        else if (hasValue)
        {
            // Insert new
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [Inventory].[TransactionAttributes]
                    (TransactionId, AttributeTypeId, DecimalValue, StringValue, IntValue, BoolValue, CreatedAt)
                VALUES ({dto.TransactionId}, {dto.AttributeTypeId}, {dto.DecimalValue}, {strVal}, {(int?)null}, {(bool?)null}, {now})", ct);
        }

        return Ok(new { success = true });
    }

    // GET /api/GrowerDelivery/WeightSheet/{id}  — header info only (no loads required)
    [HttpGet("WeightSheet/{id:long}")]
    public async Task<IActionResult> GetWeightSheet(long id, CancellationToken ct)
    {
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync(ct);

        var sql = @"
            SELECT
                ws.WeightSheetId,
                ws.ServerId,
                ws.LocationId,
                ws.Notes        AS WsNotes,
                ws.HaulerId,
                h.Description   AS HaulerName,
                ws.RateType,
                ws.CustomRateDescription,
                ws.LotId,
                lot.LotDescription,
                lot.ServerId    AS LotServerId,
                lot.LocationId  AS LotLocationId,
                p.Description   AS CropName,
                sg.SplitGroupDescription AS SplitName,
                COALESCE(
                    NULLIF(lsgAcct.EntityName, ''), lsgAcct.LookupName,
                    NULLIF(sgAcct.EntityName, ''), sgAcct.LookupName
                ) AS PrimaryAccountName
            FROM [warehouse].[WeightSheets] ws
            LEFT JOIN [account].[Haulers] h   ON h.Id = ws.HaulerId
            LEFT JOIN [Inventory].[Lots] lot  ON lot.LotId = ws.LotId
            LEFT JOIN [product].[Products] p  ON p.ProductId = lot.ProductId
            LEFT JOIN [account].[SplitGroups] sg ON sg.SplitGroupId = lot.SplitGroupId
            LEFT JOIN [account].[Accounts] sgAcct ON sgAcct.AccountId = sg.PrimaryAccountId
            LEFT JOIN [Inventory].[LotSplitGroups] lsg
                ON lsg.LotId = lot.LotId AND lsg.PrimaryAccount = 1
            LEFT JOIN [account].[Accounts] lsgAcct ON lsgAcct.AccountId = lsg.AccountId
            WHERE ws.WeightSheetId = @wsId
              AND ws.ServerId      = @serverId";

        using var cmd = conn.CreateCommand();
        cmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("@wsId", id);
        cmd.Parameters.AddWithValue("@serverId", _systemInfo.ServerId);

        using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct))
            return NotFound(new { message = "Weight sheet not found." });

        return Ok(new
        {
            WeightSheetId      = reader.GetInt64(reader.GetOrdinal("WeightSheetId")),
            ServerId           = reader.GetInt32(reader.GetOrdinal("ServerId")),
            LocationId         = reader.GetInt32(reader.GetOrdinal("LocationId")),
            WsNotes            = reader.IsDBNull(reader.GetOrdinal("WsNotes")) ? null : reader.GetString(reader.GetOrdinal("WsNotes")),
            HaulerId           = reader.IsDBNull(reader.GetOrdinal("HaulerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("HaulerId")),
            HaulerName         = reader.IsDBNull(reader.GetOrdinal("HaulerName")) ? null : reader.GetString(reader.GetOrdinal("HaulerName")),
            RateType           = reader.IsDBNull(reader.GetOrdinal("RateType")) ? null : reader.GetString(reader.GetOrdinal("RateType")),
            CustomRateDescription = reader.IsDBNull(reader.GetOrdinal("CustomRateDescription")) ? null : reader.GetString(reader.GetOrdinal("CustomRateDescription")),
            LotId              = reader.IsDBNull(reader.GetOrdinal("LotId")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("LotId")),
            LotDescription     = reader.IsDBNull(reader.GetOrdinal("LotDescription")) ? null : reader.GetString(reader.GetOrdinal("LotDescription")),
            LotServerId        = reader.IsDBNull(reader.GetOrdinal("LotServerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LotServerId")),
            LotLocationId      = reader.IsDBNull(reader.GetOrdinal("LotLocationId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LotLocationId")),
            CropName           = reader.IsDBNull(reader.GetOrdinal("CropName")) ? null : reader.GetString(reader.GetOrdinal("CropName")),
            SplitName          = reader.IsDBNull(reader.GetOrdinal("SplitName")) ? null : reader.GetString(reader.GetOrdinal("SplitName")),
            PrimaryAccountName = reader.IsDBNull(reader.GetOrdinal("PrimaryAccountName")) ? null : reader.GetString(reader.GetOrdinal("PrimaryAccountName")),
        });
    }

    // PATCH /api/GrowerDelivery/WeightSheet/{id}
    [HttpPatch("WeightSheet/{id:long}")]
    public async Task<IActionResult> UpdateWeightSheet(
        long id,
        [FromBody] UpdateWeightSheetHeaderDto dto,
        CancellationToken ct)
    {
        var ws = await _ctx.WeightSheets
            .FirstOrDefaultAsync(w => w.WeightSheetId == id && w.ServerId == _systemInfo.ServerId, ct);

        if (ws == null)
            return NotFound(new { message = "Weight sheet not found." });

        // ── PIN required when changing the lot ────────────────────────────────
        bool lotChanging = dto.LotId.HasValue && dto.LotId.Value != (ws.LotId ?? 0);
        string pinUserName = null;

        if (lotChanging)
        {
            if (!dto.Pin.HasValue || dto.Pin.Value <= 0)
                return BadRequest(new { message = "PIN is required to change the lot." });

            var pinUser = await _ctx.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Pin == dto.Pin.Value && u.IsActive, ct);

            if (pinUser is null)
                return Unauthorized(new { message = "Invalid or inactive PIN." });

            pinUserName = pinUser.UserName;
        }

        // Snapshot old values for audit
        long? oldLotId = ws.LotId;

        if (dto.HaulerId.HasValue)
            ws.HaulerId = dto.HaulerId.Value == 0 ? null : dto.HaulerId;

        if (dto.LotId.HasValue)
            ws.LotId = dto.LotId.Value == 0 ? null : dto.LotId;

        if (dto.Notes != null)
            ws.Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();

        ws.UpdatedAt = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);

        // ── Audit trail for lot change ────────────────────────────────────────
        if (lotChanging && pinUserName != null)
        {
            string oldJson = JsonSerializer.Serialize(new { LotId = oldLotId });
            string newJson = JsonSerializer.Serialize(new { LotId = ws.LotId });
            string keyJson = JsonSerializer.Serialize(new { WeightSheetId = id, ServerId = ws.ServerId });
            string tableName = "warehouse.WeightSheets";
            string action = "EDIT";
            int locationId = ws.LocationId;

            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
                VALUES ({locationId}, {pinUserName}, {tableName}, {action}, {keyJson}, {oldJson}, {newJson})",
                ct);
        }

        // Return refreshed data for header
        string haulerName = null;
        if (ws.HaulerId.HasValue)
            haulerName = await _ctx.Haulers.Where(h => h.Id == ws.HaulerId).Select(h => h.Description).FirstOrDefaultAsync(ct);

        // Return rich lot context so the header refreshes completely
        string lotDescription = null;
        int? lotServerId = null;
        int? lotLocationId = null;
        string cropName = null;
        string splitName = null;
        string primaryAccountName = null;

        if (ws.LotId.HasValue)
        {
            var lotInfo = await _ctx.Lots
                .AsNoTracking()
                .Where(l => l.LotId == ws.LotId)
                .Select(l => new
                {
                    l.LotDescription,
                    l.ServerId,
                    l.LocationId,
                    CropName = l.Product != null ? l.Product.Description : null,
                    SplitName = l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null,
                    PrimaryAccountName = l.SplitGroup != null
                        ? _ctx.Accounts
                              .Where(a => a.AccountId == l.SplitGroup.PrimaryAccountId)
                              .Select(a => a.EntityName != null && a.EntityName != "" ? a.EntityName : a.LookupName)
                              .FirstOrDefault()
                        : null,
                })
                .FirstOrDefaultAsync(ct);

            if (lotInfo != null)
            {
                lotDescription = lotInfo.LotDescription;
                lotServerId = lotInfo.ServerId;
                lotLocationId = lotInfo.LocationId;
                cropName = lotInfo.CropName;
                splitName = lotInfo.SplitName;
                primaryAccountName = lotInfo.PrimaryAccountName;
            }
        }

        return Ok(new
        {
            success = true,
            HaulerName = haulerName,
            LotDescription = lotDescription,
            LotId = ws.LotId,
            LotServerId = lotServerId,
            LotLocationId = lotLocationId,
            CropName = cropName,
            SplitName = splitName,
            PrimaryAccountName = primaryAccountName,
        });
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private async Task UpsertLotTrait(long lotId, int traitTypeId, string traitCode, DateTime now, CancellationToken ct)
    {
        // Delete existing trait of this type for the lot
        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM [Inventory].[LotTraits]
            WHERE LotId = {lotId} AND TraitTypeId = {traitTypeId}", ct);

        if (string.IsNullOrWhiteSpace(traitCode)) return;

        var trait = await _ctx.Traits
            .FirstOrDefaultAsync(t => t.TraitTypeId == traitTypeId && t.TraitCode == traitCode, ct);

        // Auto-create the trait if it doesn't exist (for State/County/Landlord)
        if (trait == null && (traitTypeId == 15 || traitTypeId == 16 || traitTypeId == 18))
        {
            trait = new Trait
            {
                TraitCode = traitCode,
                TraitTypeId = traitTypeId,
                Description = traitCode,
                IsActive = true,
                CreatedAt = now,
            };
            _ctx.Traits.Add(trait);
            await _ctx.SaveChangesAsync(ct);
        }

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
        AddString ("BOL",           dto.BOL);
        AddString ("TRUCK_ID",      dto.TruckId);
        AddString ("DRIVER",        dto.Driver);

        return list;
    }
}
