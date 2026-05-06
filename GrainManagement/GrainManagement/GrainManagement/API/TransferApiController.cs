using GrainManagement.Auth;
using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace GrainManagement.API;

/// <summary>
/// Transfer-weight-sheet endpoints. Mirrors GrowerDeliveryApiController for the
/// inter-location move flow: no Lot, instead an Item (variety) + source +
/// destination on the WS header, with Direction encoded per load.
/// </summary>
[UseAdminConnection]
[ApiController]
[Route("api/Transfer")]
[RequiresModule(nameof(Services.ModuleOptions.GrowerDelivery))]
public sealed class TransferApiController : ControllerBase
{
    // Direction strings as accepted on the WS create payload.
    private const string DirectionReceived = "Received";
    private const string DirectionShipped  = "Shipped";

    private readonly dbContext _ctx;
    private readonly ICurrentUser _currentUser;
    private readonly ILogger<TransferApiController> _logger;
    private readonly IWeightSheetNotifier _notifier;

    public TransferApiController(
        dbContext ctx,
        ICurrentUser currentUser,
        ILogger<TransferApiController> logger,
        IWeightSheetNotifier notifier)
    {
        _ctx = ctx;
        _currentUser = currentUser;
        _logger = logger;
        _notifier = notifier;
    }

    // POST /api/Transfer/WeightSheets
    // Creates a transfer weight sheet header row.
    [HttpPost("WeightSheets")]
    public async Task<IActionResult> CreateWeightSheet(
        [FromBody] CreateTransferWeightSheetDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });
        if (dto.ItemId <= 0)
            return BadRequest(new { message = "ItemId is required." });

        var direction = (dto.Direction ?? "").Trim();
        if (direction != DirectionReceived && direction != DirectionShipped)
            return BadRequest(new { message = "Direction must be 'Received' or 'Shipped'." });

        if (string.IsNullOrWhiteSpace(dto.RateType))
            return BadRequest(new { message = "Rate type is required for a transfer weight sheet." });

        // 'I' (In House) and 'N' (None) bypass hauler/rate entirely. Both
        // require CK_WeightSheets_RateType to permit those codes — see
        // SQL/AddInHouseRateType.sql.
        var rt = dto.RateType.Trim().ToUpperInvariant();
        bool isBypass = rt == "I" || rt == "N";
        if (isBypass)
        {
            dto.RateType = rt;
            dto.HaulerId = null;
            dto.Miles    = 0m;
            dto.Rate     = 0m;
            dto.CustomRateDescription = rt == "I" ? "In House" : "None";
        }
        else
        {
            if (!dto.HaulerId.HasValue || dto.HaulerId.Value <= 0)
                return BadRequest(new { message = "Hauler is required for a transfer weight sheet." });
            if (!dto.Rate.HasValue || dto.Rate.Value <= 0m)
                return BadRequest(new { message = "Rate is required for a transfer weight sheet." });
        }

        // Caller can leave Source/Destination unset for the current-location
        // side; we fill it in here from LocationId so callers can't get the
        // sides backwards.
        int sourceLoc = dto.SourceLocationId;
        int destLoc   = dto.DestinationLocationId;
        if (direction == DirectionReceived)
        {
            destLoc = dto.LocationId;
            if (sourceLoc <= 0 || sourceLoc == dto.LocationId)
                return BadRequest(new { message = "Received transfers require a Source location different from the current location." });
        }
        else
        {
            sourceLoc = dto.LocationId;
            if (destLoc <= 0 || destLoc == dto.LocationId)
                return BadRequest(new { message = "Shipped transfers require a Destination location different from the current location." });
        }

        // ── PIN validation ──────────────────────────────────────────────────
        if (!dto.Pin.HasValue || dto.Pin.Value <= 0)
            return BadRequest(new { message = "PIN is required to create a weight sheet." });

        var pinUser = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin.Value && u.IsActive, ct);
        if (pinUser is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        // Validate item exists and is active
        var itemExists = await _ctx.Items
            .AsNoTracking()
            .AnyAsync(i => i.ItemId == dto.ItemId && i.IsActive == true, ct);
        if (!itemExists)
            return BadRequest(new { message = "ItemId is not valid or not active." });

        try
        {
            var now = DateTime.UtcNow;
            var creationDate = DateOnly.FromDateTime(DateTime.Now);
            const string sheetType = "Transfer";

            var wsRowUid = Guid.NewGuid();
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [warehouse].[WeightSheets]
                    (LocationId, LotId, ItemId, SourceLocationId, DestinationLocationId,
                     WeightSheetType, RateType, HaulerId, Miles, CustomRateDescription, Rate,
                     CreationDate, CreatedAt, RowUid, WeightmasterName)
                VALUES
                    ({dto.LocationId}, NULL, {dto.ItemId}, {sourceLoc}, {destLoc},
                     {sheetType}, {dto.RateType}, {dto.HaulerId}, {dto.Miles},
                     {dto.CustomRateDescription}, {dto.Rate},
                     {creationDate}, {now}, {wsRowUid}, {pinUser.UserName})",
                ct);

            var conn = (SqlConnection)_ctx.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync(ct);

            long wsId;
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = _ctx.Database.CurrentTransaction
                    ?.GetDbTransaction() as SqlTransaction;
                cmd.CommandText = "SELECT WeightSheetId FROM [warehouse].[WeightSheets] WHERE RowUid = @uid";
                cmd.Parameters.AddWithValue("@uid", wsRowUid);
                var result = await cmd.ExecuteScalarAsync(ct)
                    ?? throw new InvalidOperationException("INSTEAD OF trigger did not produce a WeightSheetId row.");
                wsId = (long)result;
            }

            await _notifier.NotifyAsync(dto.LocationId, wsId, "ws-created", ct);

            return Ok(new
            {
                WeightSheetId = wsId,
                RowUid        = wsRowUid,
                Direction     = direction,
            });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create transfer WeightSheet for LocationId={LocationId}", dto.LocationId);
            return StatusCode(500, new { message = "Database error while creating transfer weight sheet." });
        }
    }

    // POST /api/Transfer
    // Creates a single transfer load tied to a transfer weight sheet.
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] TransferLoadDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });
        if (dto.WeightSheetUid is null || dto.WeightSheetUid == Guid.Empty)
            return BadRequest(new { message = "WeightSheetUid is required for transfer loads." });

        // ── Resolve & validate the parent transfer weight sheet ─────────────
        var ws = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(w => w.RowUid == dto.WeightSheetUid)
            .Select(w => new
            {
                w.WeightSheetId,
                w.WeightSheetType,
                w.LocationId,
                w.ItemId,
                w.SourceLocationId,
                w.DestinationLocationId,
                w.ClosedAt,
                w.StatusId,
            })
            .FirstOrDefaultAsync(ct);

        if (ws is null)
            return BadRequest(new { message = "Weight sheet not found." });
        if (!string.Equals(ws.WeightSheetType, "Transfer", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { message = "Weight sheet is not a transfer." });
        if (ws.ClosedAt.HasValue)
            return BadRequest(new { message = "Weight sheet is closed." });
        if (!ws.ItemId.HasValue || !ws.SourceLocationId.HasValue || !ws.DestinationLocationId.HasValue)
            return BadRequest(new { message = "Transfer weight sheet header is incomplete." });

        // Direction at the current location: +1 if we're the destination
        // (receiving the load), -1 if we're the source (shipping it out).
        short direction;
        if (ws.DestinationLocationId.Value == dto.LocationId) direction = 1;
        else if (ws.SourceLocationId.Value == dto.LocationId) direction = -1;
        else return BadRequest(new { message = "Current location is neither the source nor the destination of the weight sheet." });

        // ── Resolve lbs UOM ──────────────────────────────────────────────────
        var uomId = await _ctx.UnitOfMeasures
            .AsNoTracking()
            .Where(u => u.IsActive == true && u.Code == "LBS")
            .Select(u => u.UomId)
            .FirstOrDefaultAsync(ct);
        if (uomId == 0)
            return StatusCode(500, new { message = "LBS unit of measure is not configured in the database." });

        // ── Quantity rules (mirror GrowerDeliveryApiController.Create) ──────
        bool isTruck   = dto.StartQty.HasValue;
        bool hasDirect = dto.DirectQty.HasValue;
        if (!isTruck && !hasDirect)
            return BadRequest(new { message = "Either a truck weight (StartQty) or a direct quantity is required." });
        if (isTruck && hasDirect)
            return BadRequest(new { message = "Provide truck weights OR a direct quantity, not both." });

        if (isTruck)
        {
            if (!dto.StartQtyMethodId.HasValue || !dto.StartQtySourceTypeId.HasValue
                || string.IsNullOrWhiteSpace(dto.StartQtyLocation)
                || string.IsNullOrWhiteSpace(dto.StartQtySourceDescription))
                return BadRequest(new { message = "StartQty source fields are required when StartQty is provided." });

            if (dto.EndQty.HasValue
                && (!dto.EndQtyMethodId.HasValue || !dto.EndQtySourceTypeId.HasValue
                    || string.IsNullOrWhiteSpace(dto.EndQtyLocation)
                    || string.IsNullOrWhiteSpace(dto.EndQtySourceDescription)))
                return BadRequest(new { message = "EndQty source fields are required when EndQty is provided." });
        }
        if (hasDirect)
        {
            if (!dto.DirectQtyMethodId.HasValue || !dto.DirectQtySourceTypeId.HasValue
                || string.IsNullOrWhiteSpace(dto.DirectQtyLocation)
                || string.IsNullOrWhiteSpace(dto.DirectQtySourceDescription))
                return BadRequest(new { message = "DirectQty source fields are required when DirectQty is provided." });
        }

        // TruckId is required for both flows — same rule as the intake path.
        if (string.IsNullOrWhiteSpace(dto.TruckId))
            return BadRequest(new { message = "Truck ID is required." });

        // Resolve ProductId for the item — InventoryTransactionDetail requires
        // a non-null ProductId.
        var productId = await _ctx.Items
            .AsNoTracking()
            .Where(i => i.ItemId == ws.ItemId.Value)
            .Select(i => (int?)i.ProductId)
            .FirstOrDefaultAsync(ct);
        if (!productId.HasValue)
            return BadRequest(new { message = "Item does not have a valid Product." });

        // Manual-source rule: CreatedByUserId is required if any source is MANUAL.
        var sourceTypeIds = new HashSet<int>();
        if (dto.StartQtySourceTypeId.HasValue)  sourceTypeIds.Add(dto.StartQtySourceTypeId.Value);
        if (dto.EndQtySourceTypeId.HasValue)    sourceTypeIds.Add(dto.EndQtySourceTypeId.Value);
        if (dto.DirectQtySourceTypeId.HasValue) sourceTypeIds.Add(dto.DirectQtySourceTypeId.Value);

        var manualCount = await _ctx.QuantitySourceTypes
            .AsNoTracking()
            .Where(q => sourceTypeIds.Contains(q.QuantitySourceTypeId) && q.Code == "MANUAL")
            .CountAsync(ct);
        if (manualCount > 0)
        {
            if (!dto.CreatedByUserId.HasValue || dto.CreatedByUserId.Value <= 0)
                return BadRequest(new { message = "CreatedByUserId is required when any quantity source is Manual." });
        }

        // Validate container splits (only on the receiving side).
        if (dto.ToContainers != null && dto.ToContainers.Count > 0)
        {
            var sumPct = dto.ToContainers.Sum(c => c.Percent);
            if (sumPct < 99.99m || sumPct > 100.01m)
                return BadRequest(new { message = "Destination container splits must total 100%." });
        }

        // TruckId uniqueness on incomplete loads at this location. (TruckId
        // is required for transfer loads and validated above.)
        {
            var normalizedTruckId = dto.TruckId.Trim().ToUpperInvariant();
            dto.TruckId = normalizedTruckId;

            var truckIdAttrTypeId = await _ctx.TransactionAttributeTypes
                .AsNoTracking()
                .Where(t => t.Code == "TRUCK_ID" && t.IsActive == true)
                .Select(t => t.Id)
                .FirstOrDefaultAsync(ct);

            if (truckIdAttrTypeId > 0)
            {
                var duplicateExists = await _ctx.TransactionAttributes
                    .AsNoTracking()
                    .Where(ta => ta.AttributeTypeId == truckIdAttrTypeId
                              && ta.StringValue == normalizedTruckId)
                    .Join(_ctx.InventoryTransactionDetails.AsNoTracking(),
                          ta => ta.TransactionId,
                          itd => itd.TransactionId,
                          (ta, itd) => new { ta, itd })
                    .Join(_ctx.InventoryTransactions.AsNoTracking(),
                          x => x.itd.TransactionId,
                          txn => txn.TransactionId,
                          (x, txn) => new { x.itd, txn })
                    .Where(x => x.txn.LocationId == dto.LocationId
                              && x.itd.CompletedAt == null
                              && x.itd.IsVoided == false)
                    .AnyAsync(ct);

                if (duplicateExists)
                    return BadRequest(new { message = $"Truck ID '{normalizedTruckId}' already has an incomplete load at this location. Complete or void the existing load first." });
            }
        }

        DateTime? completedAt = isTruck
            ? (dto.EndQty.HasValue ? dto.CompletedAt ?? DateTime.UtcNow : null)
            : dto.CompletedAt ?? DateTime.UtcNow;

        // ── Insert transaction header via raw SQL (INSTEAD OF INSERT trigger) ──
        var now = DateTime.UtcNow;
        var txnRowUid = Guid.NewGuid();
        long transactionId;
        try
        {
            var conn = (SqlConnection)_ctx.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync(ct);

            using var cmd = conn.CreateCommand();
            cmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
            cmd.CommandText = @"
                DECLARE @uid UNIQUEIDENTIFIER = @rowUid;
                INSERT INTO [Inventory].[InventoryTransactions] (LocationId, CreatedAt, RowUid)
                VALUES (@locId, @created, @uid);
                SELECT TOP 1 TransactionId
                FROM [Inventory].[InventoryTransactions]
                WHERE RowUid = @uid OR (LocationId = @locId AND CreatedAt = @created)
                ORDER BY TransactionId DESC;";
            cmd.Parameters.AddWithValue("@locId", dto.LocationId);
            cmd.Parameters.AddWithValue("@created", now);
            cmd.Parameters.AddWithValue("@rowUid", txnRowUid);
            var result = await cmd.ExecuteScalarAsync(ct)
                ?? throw new InvalidOperationException("Failed to retrieve TransactionId after insert.");
            transactionId = (long)result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save Transfer header for WS={WsId}", ws.WeightSheetId);
            return StatusCode(500, new { message = "Database error while saving transfer." });
        }

        // ── Insert transaction detail (LotId NULL, TxnType per direction) ──
        // The DB constraint splits transfer rows by side: TRANSFER_IN at the
        // destination location (Direction=+1), TRANSFER_OUT at the source
        // location (Direction=-1).
        var detail = new InventoryTransactionDetail
        {
            TransactionId = transactionId,
            LotId         = null,
            ProductId     = productId.Value,
            ItemId        = ws.ItemId,
            TxnType       = direction == 1 ? "TRANSFER_IN" : "TRANSFER_OUT",
            Direction     = direction,
            UomId         = uomId,
            AccountId     = null,
            SplitGroupId  = null,
            StartQty      = isTruck ? dto.StartQty : null,
            EndQty        = isTruck ? dto.EndQty : null,
            DirectQty     = hasDirect ? dto.DirectQty : null,
            StartQtyLocationQuantityMethodId          = isTruck ? dto.StartQtyLocationQuantityMethodId : null,
            StartQtyLocationQuantityMethodDescription = isTruck ? dto.StartQtyLocationQuantityMethodDescription?.Trim() : null,
            EndQtyLocationQuantityMethodId            = isTruck && dto.EndQty.HasValue ? dto.EndQtyLocationQuantityMethodId : null,
            EndQtyLocationQuantityMethodDescription   = isTruck && dto.EndQty.HasValue ? dto.EndQtyLocationQuantityMethodDescription?.Trim() : null,
            DirectQtyLocationQuantityMethodId         = hasDirect ? dto.DirectQtyLocationQuantityMethodId : null,
            DirectQtyLocationQuantityMethodDescription = hasDirect ? dto.DirectQtyLocationQuantityMethodDescription?.Trim() : null,
            StartedAt     = isTruck ? dto.StartedAt : dto.StartedAt ?? now,
            CompletedAt   = completedAt,
            RefType       = dto.RefType ?? "WeightSheet",
            RefId         = dto.RefId   ?? dto.WeightSheetUid,
            Notes         = dto.Notes,
            TxnAt         = now,
            IsVoided      = false,
            CreatedByUserId = dto.CreatedByUserId,
        };

        // Defensive clamp: if both timestamps are present and end < start
        // (e.g. clock skew between client and server, or an edit that picked
        // up an out-of-order pair), align CompletedAt to StartedAt so the
        // CK_InventoryTxn_CompletedAfterStarted constraint passes.
        if (detail.StartedAt.HasValue && detail.CompletedAt.HasValue
            && detail.CompletedAt.Value < detail.StartedAt.Value)
        {
            _logger.LogWarning(
                "Transfer detail TxnId={TxnId} had CompletedAt {Completed} < StartedAt {Started}; clamping.",
                transactionId, detail.CompletedAt, detail.StartedAt);
            detail.CompletedAt = detail.StartedAt;
        }

        _ctx.InventoryTransactionDetails.Add(detail);
        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to save Transfer detail for TxnId={TxnId}", transactionId);
            return StatusCode(500, new { message = "Database error while saving transfer detail." });
        }

        // ── Container splits (destination side only — inbound transfers) ────
        if (direction == 1 && dto.ToContainers != null && dto.ToContainers.Count > 0)
        {
            foreach (var split in dto.ToContainers)
            {
                _ctx.InventoryTransactionDetailToContainers.Add(new InventoryTransactionDetailToContainer
                {
                    TransactionId = transactionId,
                    ContainerId   = split.ContainerId,
                    Percent       = split.Percent,
                });
            }
            try { await _ctx.SaveChangesAsync(ct); }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to save container splits for TxnId={TxnId}", transactionId);
                return StatusCode(500, new { message = "Database error while saving container splits." });
            }
        }

        // ── Quantity sources ────────────────────────────────────────────────
        if (dto.StartQtySourceTypeId.HasValue)
        {
            _ctx.TransactionQuantitySources.Add(new TransactionQuantitySource
            {
                TransactionId     = transactionId,
                QuantityField     = "START",
                MethodId          = dto.StartQtyMethodId,
                SourceTypeId      = dto.StartQtySourceTypeId.Value,
                Location          = dto.StartQtyLocation?.Trim(),
                SourceDescription = dto.StartQtySourceDescription.Trim(),
            });
        }
        if (dto.EndQtySourceTypeId.HasValue)
        {
            _ctx.TransactionQuantitySources.Add(new TransactionQuantitySource
            {
                TransactionId     = transactionId,
                QuantityField     = "END",
                MethodId          = dto.EndQtyMethodId,
                SourceTypeId      = dto.EndQtySourceTypeId.Value,
                Location          = dto.EndQtyLocation?.Trim(),
                SourceDescription = dto.EndQtySourceDescription.Trim(),
            });
        }
        if (dto.DirectQtySourceTypeId.HasValue)
        {
            _ctx.TransactionQuantitySources.Add(new TransactionQuantitySource
            {
                TransactionId     = transactionId,
                QuantityField     = "DIRECT",
                MethodId          = dto.DirectQtyMethodId,
                SourceTypeId      = dto.DirectQtySourceTypeId.Value,
                Location          = dto.DirectQtyLocation?.Trim(),
                SourceDescription = dto.DirectQtySourceDescription.Trim(),
            });
        }
        if (dto.StartQtySourceTypeId.HasValue || dto.EndQtySourceTypeId.HasValue || dto.DirectQtySourceTypeId.HasValue)
        {
            try { await _ctx.SaveChangesAsync(ct); }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to save quantity sources for TxnId={TxnId}", transactionId);
                return StatusCode(500, new { message = "Database error while saving quantity sources." });
            }
        }

        // ── Transaction attributes (BOL / TruckId / Driver / quality) ───────
        var attributes = BuildAttributes(dto);
        if (attributes.Count > 0)
        {
            var codes = attributes.Select(a => a.Code).ToHashSet();
            var typeMap = await _ctx.TransactionAttributeTypes
                .AsNoTracking()
                .Where(t => t.IsActive == true && codes.Contains(t.Code))
                .ToDictionaryAsync(t => t.Code, t => t.Id, ct);

            var attrNow = DateTime.UtcNow;
            foreach (var attr in attributes)
            {
                if (!typeMap.TryGetValue(attr.Code, out var typeId))
                    continue;

                _ctx.TransactionAttributes.Add(new TransactionAttribute
                {
                    TransactionAttributesUid = Guid.NewGuid(),
                    TransactionId            = transactionId,
                    AttributeTypeId          = typeId,
                    DecimalValue             = attr.DecimalValue,
                    StringValue              = attr.StringValue,
                    IntValue                 = null,
                    BoolValue                = null,
                    CreatedAt                = attrNow,
                });
            }
            try { await _ctx.SaveChangesAsync(ct); }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to save transaction attributes for TxnId={TxnId}", transactionId);
                return StatusCode(500, new { message = "Database error while saving load attributes." });
            }
        }

        await _notifier.NotifyAsync(dto.LocationId, ws.WeightSheetId, "load-created", ct);

        return Ok(new { id = transactionId });
    }

    // GET /api/Transfer/Loads?wsId={wsId}
    // Loads on a transfer weight sheet, ordered by TxnAt desc.
    [HttpGet("Loads")]
    public async Task<IActionResult> GetLoads([FromQuery] long wsId, CancellationToken ct)
    {
        if (wsId <= 0)
            return BadRequest(new { message = "wsId is required." });

        var ws = await _ctx.WeightSheets
            .AsNoTracking()
            .Include(w => w.Hauler)
            .FirstOrDefaultAsync(w => w.WeightSheetId == wsId, ct);
        if (ws is null)
            return NotFound(new { message = "Weight sheet not found." });

        var sourceLoc = ws.SourceLocationId.HasValue
            ? await _ctx.Locations.AsNoTracking()
                .Where(l => l.LocationId == ws.SourceLocationId.Value)
                .Select(l => new { l.LocationId, l.Name }).FirstOrDefaultAsync(ct)
            : null;
        var destLoc = ws.DestinationLocationId.HasValue
            ? await _ctx.Locations.AsNoTracking()
                .Where(l => l.LocationId == ws.DestinationLocationId.Value)
                .Select(l => new { l.LocationId, l.Name }).FirstOrDefaultAsync(ct)
            : null;
        var item = ws.ItemId.HasValue
            ? await _ctx.Items.AsNoTracking()
                .Where(i => i.ItemId == ws.ItemId.Value)
                .Select(i => new { i.ItemId, i.Description }).FirstOrDefaultAsync(ct)
            : null;

        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync(ct);

        const string sql = @"
            SELECT
                itd.TransactionId,
                itd.Direction,
                itd.StartQty AS InWeight,
                itd.EndQty   AS OutWeight,
                itd.DirectQty,
                itd.NetQty   AS Net,
                itd.StartedAt,
                itd.CompletedAt,
                itd.Notes,
                itd.IsVoided,
                tc.ContainerId      AS ContainerId,
                c.Description       AS ContainerDescription,
                truckAttr.StringValue AS TruckId,
                bolAttr.StringValue   AS BOL,
                drvAttr.StringValue   AS Driver
            FROM [warehouse].[WeightSheets] ws
            INNER JOIN [Inventory].[InventoryTransactionDetails] itd
                ON itd.RefId = ws.RowUid AND itd.RefType = 'WeightSheet'
            LEFT JOIN [Inventory].[InventoryTransactionDetailToContainers] tc
                ON tc.TransactionId = itd.TransactionId
            LEFT JOIN [container].[Containers] c
                ON c.ContainerId = tc.ContainerId
            LEFT JOIN [Inventory].[TransactionAttributes] truckAttr
                ON truckAttr.TransactionId = itd.TransactionId
                AND truckAttr.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'TRUCK_ID')
            LEFT JOIN [Inventory].[TransactionAttributes] bolAttr
                ON bolAttr.TransactionId = itd.TransactionId
                AND bolAttr.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'BOL')
            LEFT JOIN [Inventory].[TransactionAttributes] drvAttr
                ON drvAttr.TransactionId = itd.TransactionId
                AND drvAttr.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'DRIVER')
            WHERE ws.WeightSheetId = @wsId
            ORDER BY itd.TxnAt DESC";

        var loads = new List<object>();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@wsId", wsId);
            using var reader = await cmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                loads.Add(new
                {
                    TransactionId       = reader.GetInt64(reader.GetOrdinal("TransactionId")),
                    Direction           = reader.GetInt16(reader.GetOrdinal("Direction")),
                    InWeight            = reader.IsDBNull(reader.GetOrdinal("InWeight"))   ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("InWeight")),
                    OutWeight           = reader.IsDBNull(reader.GetOrdinal("OutWeight"))  ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("OutWeight")),
                    DirectQty           = reader.IsDBNull(reader.GetOrdinal("DirectQty"))  ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("DirectQty")),
                    Net                 = reader.IsDBNull(reader.GetOrdinal("Net"))        ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Net")),
                    StartedAt           = reader.IsDBNull(reader.GetOrdinal("StartedAt"))  ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("StartedAt")),
                    CompletedAt         = reader.IsDBNull(reader.GetOrdinal("CompletedAt"))? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CompletedAt")),
                    Notes               = reader.IsDBNull(reader.GetOrdinal("Notes"))      ? "" : reader.GetString(reader.GetOrdinal("Notes")),
                    IsVoided            = reader.GetBoolean(reader.GetOrdinal("IsVoided")),
                    ContainerDescription= reader.IsDBNull(reader.GetOrdinal("ContainerDescription")) ? "" : reader.GetString(reader.GetOrdinal("ContainerDescription")),
                    TruckId             = reader.IsDBNull(reader.GetOrdinal("TruckId"))    ? "" : reader.GetString(reader.GetOrdinal("TruckId")),
                    BOL                 = reader.IsDBNull(reader.GetOrdinal("BOL"))        ? "" : reader.GetString(reader.GetOrdinal("BOL")),
                    Driver              = reader.IsDBNull(reader.GetOrdinal("Driver"))     ? "" : reader.GetString(reader.GetOrdinal("Driver")),
                });
            }
        }

        return Ok(new
        {
            WeightSheet = new
            {
                ws.WeightSheetId,
                ws.As400Id,
                ws.RowUid,
                ws.LocationId,
                ws.WeightSheetType,
                ws.HaulerId,
                HaulerName            = ws.Hauler?.Description,
                ws.RateType,
                ws.Miles,
                ws.Rate,
                ws.CustomRateDescription,
                ws.CreationDate,
                ws.ClosedAt,
                ws.StatusId,
                Item = item,
                SourceLocation = sourceLoc,
                DestinationLocation = destLoc,
                Direction = ws.DestinationLocationId == ws.LocationId ? "Received" : "Shipped",
            },
            Loads = loads,
        });
    }

    // GET /api/Transfer/{txnId} — single transfer load with detail + containers + sources + attributes.
    [HttpGet("{txnId:long}")]
    public async Task<IActionResult> GetLoad(long txnId, CancellationToken ct)
    {
        var detail = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .Include(d => d.Transaction)
            .FirstOrDefaultAsync(d => d.TransactionId == txnId, ct);
        if (detail is null)
            return NotFound(new { message = $"Transaction {txnId} not found." });

        // Validate by the parent WS type, not by the detail's stale TxnType.
        // Loads can be moved across WS types via the legacy intake-side move
        // endpoint, which used to leave TxnType='RECEIVE' on a transfer WS.
        // We look at the parent and let the editor unstuck the row on save.
        var parentType = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(w => w.RowUid == detail.RefId)
            .Select(w => w.WeightSheetType)
            .FirstOrDefaultAsync(ct);
        if (!string.Equals(parentType, "Transfer", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { message = "Transaction is not on a transfer weight sheet." });

        var containers = await _ctx.InventoryTransactionDetailToContainers
            .AsNoTracking()
            .Where(tc => tc.TransactionId == txnId)
            .Select(tc => new { tc.ContainerId, tc.Percent })
            .ToListAsync(ct);

        var sources = await _ctx.TransactionQuantitySources
            .AsNoTracking()
            .Where(s => s.TransactionId == txnId)
            .Select(s => new { s.QuantityField, s.MethodId, s.SourceTypeId, s.Location, s.SourceDescription })
            .ToListAsync(ct);

        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync(ct);

        var attributes = new Dictionary<string, object>();
        using (var attrCmd = conn.CreateCommand())
        {
            attrCmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
            attrCmd.CommandText = @"
                SELECT at.Code, ta.DecimalValue, ta.StringValue
                FROM [Inventory].[TransactionAttributes] ta
                INNER JOIN [Inventory].[TransactionAttributeTypes] at ON at.Id = ta.AttributeTypeId
                WHERE ta.TransactionId = @txnId";
            attrCmd.Parameters.AddWithValue("@txnId", txnId);
            using var reader = await attrCmd.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                var code = reader.GetString(0);
                if (!reader.IsDBNull(1))      attributes[code] = reader.GetDecimal(1);
                else if (!reader.IsDBNull(2)) attributes[code] = reader.GetString(2);
            }
        }

        return Ok(new
        {
            detail.TransactionId,
            detail.ItemId,
            LocationId   = detail.Transaction.LocationId,
            detail.Direction,
            detail.TxnType,
            detail.StartQty,
            detail.EndQty,
            detail.DirectQty,
            detail.StartedAt,
            detail.CompletedAt,
            detail.Notes,
            detail.RefType,
            detail.RefId,
            detail.IsVoided,
            detail.CreatedByUserId,
            IsTruck = detail.StartQty.HasValue,
            detail.StartQtyLocationQuantityMethodId,
            detail.StartQtyLocationQuantityMethodDescription,
            detail.EndQtyLocationQuantityMethodId,
            detail.EndQtyLocationQuantityMethodDescription,
            detail.DirectQtyLocationQuantityMethodId,
            detail.DirectQtyLocationQuantityMethodDescription,
            ToContainers = containers,
            Sources      = sources,
            Attributes   = attributes,
        });
    }

    // PUT /api/Transfer/{txnId} — update an existing transfer load.
    [HttpPut("{txnId:long}")]
    public async Task<IActionResult> UpdateLoad(
        long txnId,
        [FromBody] TransferLoadDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });

        var txn = await _ctx.InventoryTransactions
            .Include(t => t.InventoryTransactionDetail)
            .FirstOrDefaultAsync(t => t.TransactionId == txnId, ct);
        if (txn is null) return NotFound(new { message = $"Transaction {txnId} not found." });

        var detail = txn.InventoryTransactionDetail;
        if (detail is null) return NotFound(new { message = "Transaction detail not found." });
        if (detail.IsVoided)
            return BadRequest(new { message = "Cannot edit a voided load." });

        // Resolve the parent WS first — we validate by parent type rather than
        // by the detail's TxnType so loads moved cross-type via the legacy
        // intake-side move endpoint can still be edited (and will be re-stamped
        // to TRANSFER_IN/OUT on save).
        var ws = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(w => w.RowUid == detail.RefId)
            .Select(w => new { w.WeightSheetId, w.WeightSheetType, w.LocationId,
                               w.SourceLocationId, w.DestinationLocationId, w.ItemId })
            .FirstOrDefaultAsync(ct);
        if (ws is null) return BadRequest(new { message = "Parent weight sheet not found." });
        if (!string.Equals(ws.WeightSheetType, "Transfer", StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { message = "Transaction is not on a transfer weight sheet." });

        bool isTruck   = dto.StartQty.HasValue;
        bool hasDirect = dto.DirectQty.HasValue;
        if (!isTruck && !hasDirect)
            return BadRequest(new { message = "Either a truck weight (StartQty) or a direct quantity is required." });
        if (isTruck && hasDirect)
            return BadRequest(new { message = "Provide truck weights OR a direct quantity, not both." });

        if (string.IsNullOrWhiteSpace(dto.TruckId))
            return BadRequest(new { message = "Truck ID is required." });

        // ── Apply the edit ─────────────────────────────────────────────────
        // Re-stamp transfer-shape fields so a load that was moved here from a
        // delivery WS via the legacy intake-side move (which left TxnType=
        // 'RECEIVE' + LotId set) is healed on save.
        short stampedDirection = (ws.DestinationLocationId == ws.LocationId) ? (short)1 : (short)-1;
        detail.LotId     = null;
        detail.ItemId    = ws.ItemId;
        detail.Direction = stampedDirection;
        detail.TxnType   = stampedDirection == 1 ? "TRANSFER_IN" : "TRANSFER_OUT";

        detail.StartQty   = isTruck ? dto.StartQty : null;
        detail.EndQty     = isTruck ? dto.EndQty : null;
        detail.DirectQty  = hasDirect ? dto.DirectQty : null;
        detail.StartQtyLocationQuantityMethodId          = isTruck ? dto.StartQtyLocationQuantityMethodId : null;
        detail.StartQtyLocationQuantityMethodDescription = isTruck ? dto.StartQtyLocationQuantityMethodDescription?.Trim() : null;
        detail.EndQtyLocationQuantityMethodId            = isTruck && dto.EndQty.HasValue ? dto.EndQtyLocationQuantityMethodId : null;
        detail.EndQtyLocationQuantityMethodDescription   = isTruck && dto.EndQty.HasValue ? dto.EndQtyLocationQuantityMethodDescription?.Trim() : null;
        detail.DirectQtyLocationQuantityMethodId         = hasDirect ? dto.DirectQtyLocationQuantityMethodId : null;
        detail.DirectQtyLocationQuantityMethodDescription = hasDirect ? dto.DirectQtyLocationQuantityMethodDescription?.Trim() : null;
        detail.Notes      = dto.Notes;
        detail.UpdatedAt  = DateTime.UtcNow;
        detail.IsVoided   = false;
        detail.CreatedByUserId = dto.CreatedByUserId ?? detail.CreatedByUserId;

        // Capture timestamps. StartedAt is preserved when it already exists
        // (the original "truck arrived" moment) and only stamped on a fresh
        // truck load. CompletedAt is recorded when an Out weight is added on
        // edit — without this, weighing-out an existing load left the load
        // marked complete-by-weight but with no completion time. For direct
        // loads we mirror StartedAt onto CompletedAt so reports stay in sync.
        if (isTruck)
        {
            if (!detail.StartedAt.HasValue)
                detail.StartedAt = dto.StartedAt ?? DateTime.UtcNow;

            if (dto.EndQty.HasValue)
                detail.CompletedAt = dto.CompletedAt ?? detail.CompletedAt ?? DateTime.UtcNow;
            else
                detail.CompletedAt = null;
        }
        else if (hasDirect)
        {
            detail.StartedAt   = dto.StartedAt   ?? detail.StartedAt   ?? DateTime.UtcNow;
            detail.CompletedAt = dto.CompletedAt ?? detail.CompletedAt ?? detail.StartedAt;
        }

        // ── Replace container splits ───────────────────────────────────────
        var existingContainers = await _ctx.InventoryTransactionDetailToContainers
            .Where(tc => tc.TransactionId == txnId)
            .ToListAsync(ct);
        _ctx.InventoryTransactionDetailToContainers.RemoveRange(existingContainers);
        if (dto.ToContainers != null && dto.ToContainers.Count > 0)
        {
            foreach (var split in dto.ToContainers)
            {
                _ctx.InventoryTransactionDetailToContainers.Add(new InventoryTransactionDetailToContainer
                {
                    TransactionId = txnId,
                    ContainerId   = split.ContainerId,
                    Percent       = split.Percent,
                });
            }
        }

        // ── Replace quantity source records ────────────────────────────────
        var existingSources = await _ctx.TransactionQuantitySources
            .Where(s => s.TransactionId == txnId)
            .ToListAsync(ct);
        _ctx.TransactionQuantitySources.RemoveRange(existingSources);

        if (isTruck && dto.StartQtySourceTypeId.HasValue)
        {
            _ctx.TransactionQuantitySources.Add(new TransactionQuantitySource
            {
                TransactionId     = txnId,
                QuantityField     = "START",
                MethodId          = dto.StartQtyMethodId,
                SourceTypeId      = dto.StartQtySourceTypeId.Value,
                Location          = dto.StartQtyLocation?.Trim(),
                SourceDescription = dto.StartQtySourceDescription?.Trim() ?? string.Empty,
            });
        }
        if (isTruck && dto.EndQty.HasValue && dto.EndQtySourceTypeId.HasValue)
        {
            _ctx.TransactionQuantitySources.Add(new TransactionQuantitySource
            {
                TransactionId     = txnId,
                QuantityField     = "END",
                MethodId          = dto.EndQtyMethodId,
                SourceTypeId      = dto.EndQtySourceTypeId.Value,
                Location          = dto.EndQtyLocation?.Trim(),
                SourceDescription = dto.EndQtySourceDescription?.Trim() ?? string.Empty,
            });
        }
        if (hasDirect && dto.DirectQtySourceTypeId.HasValue)
        {
            _ctx.TransactionQuantitySources.Add(new TransactionQuantitySource
            {
                TransactionId     = txnId,
                QuantityField     = "DIRECT",
                MethodId          = dto.DirectQtyMethodId,
                SourceTypeId      = dto.DirectQtySourceTypeId.Value,
                Location          = dto.DirectQtyLocation?.Trim(),
                SourceDescription = dto.DirectQtySourceDescription?.Trim() ?? string.Empty,
            });
        }

        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update Transfer load TxnId={TxnId}", txnId);
            return StatusCode(500, new { message = "Database error while updating transfer load." });
        }

        // ── Replace transaction attributes ─────────────────────────────────
        var knownCodes = new[] { "MOISTURE", "PROTEIN", "OIL", "STARCH", "TEST_WEIGHT",
            "DOCKAGE", "FOREIGN_MATTER", "SPLITS", "DAMAGED", "GRADE",
            "BOL", "TRUCK_ID", "DRIVER" };
        var typeMap = await _ctx.TransactionAttributeTypes
            .AsNoTracking()
            .Where(t => t.IsActive == true && knownCodes.Contains(t.Code))
            .ToDictionaryAsync(t => t.Code, t => t.Id, ct);
        var knownIds = typeMap.Values.ToHashSet();
        var existingAttrs = await _ctx.TransactionAttributes
            .Where(a => a.TransactionId == txnId && knownIds.Contains(a.AttributeTypeId))
            .ToListAsync(ct);
        _ctx.TransactionAttributes.RemoveRange(existingAttrs);

        var attrNow = DateTime.UtcNow;
        foreach (var attr in BuildAttributes(dto))
        {
            if (!typeMap.TryGetValue(attr.Code, out var typeId)) continue;
            _ctx.TransactionAttributes.Add(new TransactionAttribute
            {
                TransactionAttributesUid = Guid.NewGuid(),
                TransactionId            = txnId,
                AttributeTypeId          = typeId,
                DecimalValue             = attr.DecimalValue,
                StringValue              = attr.StringValue,
                IntValue                 = null,
                BoolValue                = null,
                CreatedAt                = attrNow,
            });
        }
        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update transfer load attributes TxnId={TxnId}", txnId);
            return StatusCode(500, new { message = "Database error while updating load attributes." });
        }

        await _notifier.NotifyAsync(dto.LocationId, ws.WeightSheetId, "load-updated", ct);

        return Ok(new { id = txnId });
    }

    // GET /api/Transfer/{txnId}/move-candidates — open weight sheets at this
    // location (excluding the source). Includes both Transfer and Delivery
    // (intake) WSs so an operator can move a transfer load onto an intake
    // sheet when appropriate. The "effective" item is surfaced for each
    // candidate — Transfer WSs carry it on the header, Delivery WSs derive
    // it from the lot — so the client can warn on cross-variety moves.
    [HttpGet("{txnId:long}/move-candidates")]
    public async Task<IActionResult> GetMoveCandidates(long txnId, CancellationToken ct)
    {
        var detail = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.TransactionId == txnId, ct);
        if (detail is null) return NotFound(new { message = $"Transaction {txnId} not found." });
        if (detail.RefType != "WeightSheet" || !detail.RefId.HasValue)
            return BadRequest(new { message = "Load is not currently attached to a weight sheet." });

        var txn = await _ctx.InventoryTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TransactionId == txnId, ct);
        if (txn is null) return NotFound(new { message = $"Transaction {txnId} not found." });

        var sourceUid = detail.RefId.Value;
        var candidates = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(ws => ws.RowUid != sourceUid
                         && ws.LocationId == txn.LocationId
                         && ws.ClosedAt == null
                         && (ws.WeightSheetType == "Transfer" || ws.WeightSheetType == "Delivery"))
            .Select(ws => new
            {
                ws.WeightSheetId,
                ws.As400Id,
                ws.RowUid,
                ws.CreationDate,
                ws.StatusId,
                ws.WeightSheetType,
                // EffectiveItemId resolves to the WS's own ItemId for transfers
                // or the lot's ItemId for deliveries. Used for cross-variety
                // checks regardless of WS type.
                EffectiveItemId = ws.WeightSheetType == "Transfer"
                    ? ws.ItemId
                    : (ws.LotId != null
                        ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.ItemId).FirstOrDefault()
                        : null),
                Variety = ws.WeightSheetType == "Transfer"
                    ? (ws.ItemId != null
                        ? _ctx.Items.Where(i => i.ItemId == ws.ItemId).Select(i => i.Description).FirstOrDefault()
                        : null)
                    : (ws.LotId != null
                        ? _ctx.Lots.Where(l => l.LotId == ws.LotId)
                            .Select(l => _ctx.Items.Where(i => i.ItemId == l.ItemId).Select(i => i.Description).FirstOrDefault())
                            .FirstOrDefault()
                        : null),
                ws.LotId,
                LotDescription = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.LotDescription).FirstOrDefault()
                    : null,
                ws.SourceLocationId,
                SourceName = ws.SourceLocationId != null
                    ? _ctx.Locations.Where(l => l.LocationId == ws.SourceLocationId).Select(l => l.Name).FirstOrDefault()
                    : null,
                ws.DestinationLocationId,
                DestinationName = ws.DestinationLocationId != null
                    ? _ctx.Locations.Where(l => l.LocationId == ws.DestinationLocationId).Select(l => l.Name).FirstOrDefault()
                    : null,
                Direction = ws.WeightSheetType == "Transfer"
                    ? (ws.DestinationLocationId == ws.LocationId ? "Received" : "Shipped")
                    : (string)null,
                LoadCount = _ctx.InventoryTransactionDetails
                    .Count(d => d.RefId == ws.RowUid && d.RefType == "WeightSheet" && !d.IsVoided),
            })
            .ToListAsync(ct);

        var filtered = candidates
            .Where(c => c.LoadCount < 25)
            .OrderByDescending(c => c.CreationDate)
            .ToList();

        // Source WS info for cross-WS warnings. Source is always a transfer
        // WS in this controller; we still surface EffectiveItemId so the
        // client uses a single field for comparisons.
        var sourceWs = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(w => w.RowUid == sourceUid)
            .Select(w => new
            {
                w.WeightSheetId,
                w.As400Id,
                w.WeightSheetType,
                EffectiveItemId = w.ItemId,
                Variety = w.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == w.ItemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                w.SourceLocationId,
                w.DestinationLocationId,
            })
            .FirstOrDefaultAsync(ct);

        return Ok(new
        {
            Source     = sourceWs,
            Candidates = filtered,
        });
    }

    public class TransferMoveDto
    {
        public Guid TargetWeightSheetUid { get; set; }
        public int  Pin { get; set; }
    }

    // POST /api/Transfer/{txnId}/move — move a transfer load to another transfer WS.
    [HttpPost("{txnId:long}/move")]
    public async Task<IActionResult> MoveLoad(
        long txnId,
        [FromBody] TransferMoveDto dto,
        CancellationToken ct)
    {
        if (dto is null) return BadRequest(new { message = "Request body is required." });
        if (dto.TargetWeightSheetUid == Guid.Empty)
            return BadRequest(new { message = "TargetWeightSheetUid is required." });
        if (dto.Pin <= 0) return BadRequest(new { message = "PIN is required." });

        var pinUser = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin && u.IsActive, ct);
        if (pinUser is null) return Unauthorized(new { message = "Invalid or inactive PIN." });

        const int PrivilegeIdMoveLoads = 2;
        var hasPrivilege = await _ctx.UserPrivileges
            .AsNoTracking()
            .AnyAsync(p => p.UserId == pinUser.UserId && p.PrivilegeId == PrivilegeIdMoveLoads, ct);
        if (!hasPrivilege)
            return Unauthorized(new { message = "User does not have the Move Loads privilege." });

        var detail = await _ctx.InventoryTransactionDetails
            .FirstOrDefaultAsync(d => d.TransactionId == txnId, ct);
        if (detail is null) return NotFound(new { message = $"Transaction {txnId} not found." });
        if (detail.IsVoided) return BadRequest(new { message = "Cannot move a voided load." });
        if (detail.RefType != "WeightSheet" || !detail.RefId.HasValue)
            return BadRequest(new { message = "Load is not currently attached to a weight sheet." });

        var txn = await _ctx.InventoryTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TransactionId == txnId, ct);
        if (txn is null) return NotFound(new { message = $"Transaction {txnId} not found." });

        var sourceUid = detail.RefId.Value;
        if (sourceUid == dto.TargetWeightSheetUid)
            return BadRequest(new { message = "Load is already on that weight sheet." });

        var target = await _ctx.WeightSheets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.RowUid == dto.TargetWeightSheetUid, ct);
        if (target is null) return NotFound(new { message = "Destination weight sheet not found." });
        if (target.LocationId != txn.LocationId)
            return BadRequest(new { message = "Destination weight sheet is on a different location." });
        var targetType = (target.WeightSheetType ?? "").ToLowerInvariant();
        if (targetType != "transfer" && targetType != "delivery")
            return BadRequest(new { message = "Destination weight sheet type is not supported." });
        if (target.ClosedAt.HasValue)
            return BadRequest(new { message = "Destination weight sheet is closed." });

        var targetLoadCount = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .CountAsync(d => d.RefId == target.RowUid && d.RefType == "WeightSheet" && !d.IsVoided, ct);
        if (targetLoadCount >= 25)
            return BadRequest(new { message = "Destination weight sheet is full (25 loads max)." });

        var sourceWs = await _ctx.WeightSheets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.RowUid == sourceUid, ct);

        // Re-stamp the detail to match the new parent WS:
        //   * Delivery target: TxnType becomes 'RECEIVE', LotId is taken from
        //     the target lot, Direction is +1 (inbound).
        //   * Transfer target: keep transfer semantics — TxnType reflects the
        //     load's role at the current location (TRANSFER_IN at the
        //     destination, TRANSFER_OUT at the source).
        detail.RefId     = target.RowUid;
        detail.UpdatedAt = DateTime.UtcNow;
        if (targetType == "delivery")
        {
            if (!target.LotId.HasValue)
                return BadRequest(new { message = "Destination delivery weight sheet has no lot." });
            detail.LotId     = target.LotId.Value;
            detail.TxnType   = "RECEIVE";
            detail.Direction = 1;
        }
        else
        {
            short newDirection = (target.DestinationLocationId == target.LocationId) ? (short)1 : (short)-1;
            detail.LotId     = null;
            detail.Direction = newDirection;
            detail.TxnType   = newDirection == 1 ? "TRANSFER_IN" : "TRANSFER_OUT";
        }

        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to move transfer load TxnId={TxnId}", txnId);
            return StatusCode(500, new { message = "Database error while moving load." });
        }

        // Audit: same shape as intake MOVE_LOAD.
        string oldJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            ["As400Id"] = sourceWs?.As400Id,
            ["LoadId"]  = txnId,
        });
        string newJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            ["As400Id"] = target.As400Id,
            ["LoadId"]  = txnId,
        });
        string keyJson   = JsonSerializer.Serialize(new { TransactionId = txnId });
        string tableName = "Inventory.InventoryTransactionDetails";
        string action    = "MOVE_LOAD";
        int locationId   = txn.LocationId;
        string userName  = pinUser.UserName;

        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
            VALUES ({locationId}, {userName}, {tableName}, {action}, {keyJson}, {oldJson}, {newJson})",
            ct);

        if (sourceWs?.WeightSheetId is long fromTransferId)
            await _notifier.NotifyAsync(locationId, fromTransferId, "load-moved-out", ct);
        await _notifier.NotifyAsync(locationId, target.WeightSheetId, "load-moved-in", ct);

        return Ok(new
        {
            id                 = txnId,
            fromWeightSheetId  = sourceWs?.WeightSheetId,
            toWeightSheetId    = target.WeightSheetId,
            toWeightSheetUid   = target.RowUid,
        });
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private record AttributeEntry(string Code, decimal? DecimalValue, string StringValue);

    private static List<AttributeEntry> BuildAttributes(TransferLoadDto dto)
    {
        var list = new List<AttributeEntry>();
        void AddDecimal(string code, decimal? value)
        {
            if (value.HasValue && value.Value != 0m)
                list.Add(new AttributeEntry(code, value, null));
        }
        void AddString(string code, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                list.Add(new AttributeEntry(code, null, value.Trim()));
        }

        AddDecimal("MOISTURE",       dto.Moisture);
        AddDecimal("PROTEIN",        dto.Protein);
        AddDecimal("OIL",            dto.Oil);
        AddDecimal("STARCH",         dto.Starch);
        AddDecimal("TEST_WEIGHT",    dto.TestWeight);
        AddDecimal("DOCKAGE",        dto.Dockage);
        AddDecimal("FOREIGN_MATTER", dto.ForeignMatter);
        AddDecimal("SPLITS",         dto.Splits);
        AddDecimal("DAMAGED",        dto.Damaged);
        AddString ("GRADE",          dto.Grade);
        AddString ("BOL",            dto.BOL);
        AddString ("TRUCK_ID",       dto.TruckId);
        AddString ("DRIVER",         dto.Driver);
        return list;
    }
}
