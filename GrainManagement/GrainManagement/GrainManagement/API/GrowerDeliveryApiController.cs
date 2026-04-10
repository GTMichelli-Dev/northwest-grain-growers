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

[UseAdminConnection]
[ApiController]
[Route("api/GrowerDelivery")]
[RequiresModule(nameof(Services.ModuleOptions.GrowerDelivery))]
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

        // ── Quantity: must have truck flow (StartQty, optionally EndQty) OR direct, not both ──
        bool isTruck   = dto.StartQty.HasValue;
        bool hasDirect = dto.DirectQty.HasValue;

        if (!isTruck && !hasDirect)
            return BadRequest(new { message = "Either a truck weight (StartQty) or a direct quantity is required." });
        if (isTruck && hasDirect)
            return BadRequest(new { message = "Provide truck weights OR a direct quantity, not both." });

        // ── Quantity source validation ──────────────────────────────────────────
        // Truck flow: StartQty source always required; EndQty source required only when EndQty provided.
        if (isTruck)
        {
            if (!dto.StartQtyMethodId.HasValue)
                return BadRequest(new { message = "StartQtyMethodId is required when StartQty is provided." });
            if (!dto.StartQtySourceTypeId.HasValue)
                return BadRequest(new { message = "StartQtySourceTypeId is required when StartQty is provided." });
            if (string.IsNullOrWhiteSpace(dto.StartQtyLocation))
                return BadRequest(new { message = "StartQtyLocation is required when StartQty is provided." });
            if (string.IsNullOrWhiteSpace(dto.StartQtySourceDescription))
                return BadRequest(new { message = "StartQtySourceDescription is required when StartQty is provided." });

            if (dto.EndQty.HasValue)
            {
                if (!dto.EndQtyMethodId.HasValue)
                    return BadRequest(new { message = "EndQtyMethodId is required when EndQty is provided." });
                if (!dto.EndQtySourceTypeId.HasValue)
                    return BadRequest(new { message = "EndQtySourceTypeId is required when EndQty is provided." });
                if (string.IsNullOrWhiteSpace(dto.EndQtyLocation))
                    return BadRequest(new { message = "EndQtyLocation is required when EndQty is provided." });
                if (string.IsNullOrWhiteSpace(dto.EndQtySourceDescription))
                    return BadRequest(new { message = "EndQtySourceDescription is required when EndQty is provided." });
            }
        }

        // When DirectQty is provided, require source tracking fields.
        if (hasDirect)
        {
            if (!dto.DirectQtyMethodId.HasValue)
                return BadRequest(new { message = "DirectQtyMethodId is required when DirectQty is provided." });
            if (!dto.DirectQtySourceTypeId.HasValue)
                return BadRequest(new { message = "DirectQtySourceTypeId is required when DirectQty is provided." });
            if (string.IsNullOrWhiteSpace(dto.DirectQtyLocation))
                return BadRequest(new { message = "DirectQtyLocation is required when DirectQty is provided." });
            if (string.IsNullOrWhiteSpace(dto.DirectQtySourceDescription))
                return BadRequest(new { message = "DirectQtySourceDescription is required when DirectQty is provided." });
        }

        // Validate source types exist and are active
        var sourceTypeIds = new HashSet<int>();
        if (dto.StartQtySourceTypeId.HasValue)  sourceTypeIds.Add(dto.StartQtySourceTypeId.Value);
        if (dto.EndQtySourceTypeId.HasValue)    sourceTypeIds.Add(dto.EndQtySourceTypeId.Value);
        if (dto.DirectQtySourceTypeId.HasValue) sourceTypeIds.Add(dto.DirectQtySourceTypeId.Value);

        Dictionary<int, string> sourceTypeMap = new();
        if (sourceTypeIds.Count > 0)
        {
            sourceTypeMap = await _ctx.QuantitySourceTypes
                .AsNoTracking()
                .Where(q => q.IsActive && sourceTypeIds.Contains(q.QuantitySourceTypeId))
                .ToDictionaryAsync(q => q.QuantitySourceTypeId, q => q.Code, ct);

            foreach (var id in sourceTypeIds)
            {
                if (!sourceTypeMap.ContainsKey(id))
                    return BadRequest(new { message = $"QuantitySourceTypeId {id} is not valid or not active." });
            }
        }

        // Validate methods exist and are active
        var methodIds = new HashSet<int>();
        if (dto.StartQtyMethodId.HasValue)  methodIds.Add(dto.StartQtyMethodId.Value);
        if (dto.EndQtyMethodId.HasValue)    methodIds.Add(dto.EndQtyMethodId.Value);
        if (dto.DirectQtyMethodId.HasValue) methodIds.Add(dto.DirectQtyMethodId.Value);

        if (methodIds.Count > 0)
        {
            var activeMethods = await _ctx.QuantityMethods
                .AsNoTracking()
                .Where(m => m.IsActive && methodIds.Contains(m.QuantityMethodId))
                .Select(m => m.QuantityMethodId)
                .ToHashSetAsync(ct);

            foreach (var id in methodIds)
            {
                if (!activeMethods.Contains(id))
                    return BadRequest(new { message = $"QuantityMethodId {id} is not valid or not active." });
            }
        }

        // Validate method + source type combos against QuantityMethodSources cross-ref
        if (dto.StartQtyMethodId.HasValue && dto.StartQtySourceTypeId.HasValue)
        {
            var startComboValid = await _ctx.QuantityMethodSources
                .AsNoTracking()
                .AnyAsync(ms => ms.QuantityMethodId == dto.StartQtyMethodId.Value
                             && ms.QuantitySourceTypeId == dto.StartQtySourceTypeId.Value, ct);
            if (!startComboValid)
                return BadRequest(new { message = $"Source type {dto.StartQtySourceTypeId} is not allowed for method {dto.StartQtyMethodId} (StartQty)." });
        }

        if (dto.EndQtyMethodId.HasValue && dto.EndQtySourceTypeId.HasValue)
        {
            var endComboValid = await _ctx.QuantityMethodSources
                .AsNoTracking()
                .AnyAsync(ms => ms.QuantityMethodId == dto.EndQtyMethodId.Value
                             && ms.QuantitySourceTypeId == dto.EndQtySourceTypeId.Value, ct);
            if (!endComboValid)
                return BadRequest(new { message = $"Source type {dto.EndQtySourceTypeId} is not allowed for method {dto.EndQtyMethodId} (EndQty)." });
        }

        // Validate DirectQty method + source type combo
        if (dto.DirectQtyMethodId.HasValue && dto.DirectQtySourceTypeId.HasValue)
        {
            var directComboValid = await _ctx.QuantityMethodSources
                .AsNoTracking()
                .AnyAsync(ms => ms.QuantityMethodId == dto.DirectQtyMethodId.Value
                             && ms.QuantitySourceTypeId == dto.DirectQtySourceTypeId.Value, ct);
            if (!directComboValid)
                return BadRequest(new { message = $"Source type {dto.DirectQtySourceTypeId} is not allowed for method {dto.DirectQtyMethodId} (DirectQty)." });
        }

        // ── Resolve ProductId, ItemId, AccountId from the Lot ────────────────
        var lot = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LotId == dto.LotId)
            .Select(l => new
            {
                l.ProductId,
                l.ItemId,
                l.SplitGroupId,
                PrimaryAccountId = l.SplitGroup != null ? (long?)l.SplitGroup.PrimaryAccountId : null,
            })
            .FirstOrDefaultAsync(ct);

        if (lot is null)
            return BadRequest(new { message = $"Lot {dto.LotId} not found." });

        if (!lot.ProductId.HasValue)
            return BadRequest(new { message = $"Lot {dto.LotId} does not have a ProductId configured." });

        // If either source is MANUAL, CreatedByUserId is required
        bool anyManual = sourceTypeMap.Values.Any(code => code == "MANUAL");

        if (anyManual)
        {
            if (!dto.CreatedByUserId.HasValue || dto.CreatedByUserId.Value <= 0)
                return BadRequest(new { message = "CreatedByUserId is required when any quantity source is Manual." });

            var userExists = await _ctx.Users
                .AsNoTracking()
                .AnyAsync(u => u.UserId == dto.CreatedByUserId.Value && u.IsActive, ct);

            if (!userExists)
                return BadRequest(new { message = $"CreatedByUserId {dto.CreatedByUserId.Value} is not valid or not active." });
        }

        // ── Validate container splits (if provided) ────────────────────────────
        var toSplitError = ValidateContainerSplits(dto.ToContainers, "destination");
        if (toSplitError != null)
            return BadRequest(new { message = toSplitError });

        // ── TruckId uniqueness check for truck loads ─────────────────────────
        if (isTruck && !string.IsNullOrWhiteSpace(dto.TruckId))
        {
            var normalizedTruckId = dto.TruckId.Trim().ToUpperInvariant();
            dto.TruckId = normalizedTruckId;

            // Find the TRUCK_ID attribute type ID
            var truckIdAttrTypeId = await _ctx.TransactionAttributeTypes
                .AsNoTracking()
                .Where(t => t.Code == "TRUCK_ID" && t.IsActive == true)
                .Select(t => t.Id)
                .FirstOrDefaultAsync(ct);

            if (truckIdAttrTypeId > 0)
            {
                // Check for any incomplete load with the same TruckId at this location+server
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

        // ── Resolve CompletedAt per truck vs non-truck rules ────────────────
        // Truck: set CompletedAt only when EndQty is provided (scale-out)
        // Non-truck: always set CompletedAt
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

            // Use a single command to insert and retrieve the trigger-generated TransactionId.
            // The INSTEAD OF trigger intercepts the INSERT and generates the real row.
            // We retrieve it by querying the max TransactionId for this location immediately after.
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
                cmd.CommandText = @"
                    DECLARE @uid UNIQUEIDENTIFIER = @rowUid;

                    INSERT INTO [Inventory].[InventoryTransactions] (LocationId, CreatedAt, RowUid)
                    VALUES (@locId, @created, @uid);

                    -- Try RowUid first (if trigger preserves it), then fall back to MAX
                    SELECT TOP 1 TransactionId
                    FROM [Inventory].[InventoryTransactions]
                    WHERE RowUid = @uid
                       OR (LocationId = @locId AND CreatedAt = @created)
                    ORDER BY TransactionId DESC;";
                cmd.Parameters.AddWithValue("@locId", dto.LocationId);
                cmd.Parameters.AddWithValue("@created", now);
                cmd.Parameters.AddWithValue("@rowUid", txnRowUid);
                var result = await cmd.ExecuteScalarAsync(ct)
                    ?? throw new InvalidOperationException("Failed to retrieve TransactionId after insert.");
                transactionId = (long)result;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save GrowerDelivery header for LotId={LotId}", dto.LotId);
            return StatusCode(500, new { message = "Database error while saving delivery." });
        }

        // ── Insert transaction detail ───────────────────────────────────────────
        var detail = new InventoryTransactionDetail
        {
            TransactionId = transactionId,
            LotId        = dto.LotId,
            ProductId    = lot.ProductId.Value,
            ItemId       = lot.ItemId,
            TxnType      = "RECEIVE",
            Direction    = 1,
            UomId        = uomId,
            AccountId    = lot.PrimaryAccountId,
            SplitGroupId = lot.SplitGroupId,
            StartQty     = isTruck ? dto.StartQty : null,
            EndQty       = isTruck ? dto.EndQty : null,
            DirectQty    = hasDirect ? dto.DirectQty : null,
            StartQtyLocationQuantityMethodId          = isTruck ? dto.StartQtyLocationQuantityMethodId : null,
            StartQtyLocationQuantityMethodDescription = isTruck ? dto.StartQtyLocationQuantityMethodDescription?.Trim() : null,
            EndQtyLocationQuantityMethodId            = isTruck && dto.EndQty.HasValue ? dto.EndQtyLocationQuantityMethodId : null,
            EndQtyLocationQuantityMethodDescription   = isTruck && dto.EndQty.HasValue ? dto.EndQtyLocationQuantityMethodDescription?.Trim() : null,
            DirectQtyLocationQuantityMethodId         = hasDirect ? dto.DirectQtyLocationQuantityMethodId : null,
            DirectQtyLocationQuantityMethodDescription = hasDirect ? dto.DirectQtyLocationQuantityMethodDescription?.Trim() : null,
            StartedAt    = isTruck ? dto.StartedAt : dto.StartedAt ?? now,
            CompletedAt  = completedAt,
            RefType      = dto.RefType,
            RefId        = dto.RefId,
            Notes        = dto.Notes,
            TxnAt        = now,
            IsVoided     = false,
            CreatedByUserId = dto.CreatedByUserId,
        };

        _ctx.InventoryTransactionDetails.Add(detail);

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to save GrowerDelivery for LotId={LotId}", dto.LotId);
            return StatusCode(500, new { message = "Database error while saving delivery." });
        }

        // ── Save destination container splits ────────────────────────────────────
        if (dto.ToContainers?.Count > 0)
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

            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to save container splits for TransactionId={TxnId}", transactionId);
                return StatusCode(500, new { message = "Database error while saving container splits." });
            }
        }

        // ── Save quantity source records ────────────────────────────────────────
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
            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to save quantity sources for TransactionId={TxnId}", transactionId);
                return StatusCode(500, new { message = "Database error while saving quantity sources." });
            }
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

                var attrNow = DateTime.UtcNow;
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    INSERT INTO [Inventory].[TransactionAttributes]
                        (TransactionId, AttributeTypeId, DecimalValue, StringValue, IntValue, BoolValue, CreatedAt)
                    VALUES ({transactionId}, {typeId}, {attr.DecimalValue}, {attr.StringValue}, {(int?)null}, {(bool?)null}, {attrNow})",
                    ct);
            }
        }

        // ── Link to weight sheet directly via RefId ────────────────────────────
        if (dto.WeightSheetUid.HasValue)
        {
            detail.RefType = "WeightSheet";
            detail.RefId   = dto.WeightSheetUid.Value;

            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to link delivery to weight sheet for TransactionId={TxnId}", transactionId);
                return StatusCode(500, new { message = "Database error while linking to weight sheet." });
            }
        }

        // ── Completion status ───────────────────────────────────────────────────
        bool hasQty       = isTruck ? dto.EndQty.HasValue : hasDirect;
        bool hasContainer = dto.ToContainers?.Count > 0;
        bool isComplete   = hasQty && hasContainer;

        return Ok(new
        {
            id         = transactionId,
            isComplete,
            hasQuantity  = hasQty,
            hasContainer,
        });
    }

    // PUT /api/GrowerDelivery/{transactionId}
    [HttpPut("{transactionId:long}")]
    public async Task<IActionResult> Update(
        long transactionId,
        [FromBody] GrowerDeliveryDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });

        if (dto.LotId <= 0)
            return BadRequest(new { message = "LotId is required." });
        if (dto.LocationId <= 0)
            return BadRequest(new { message = "LocationId is required." });

        // ── Load existing transaction with detail ────────────────────────────
        var txn = await _ctx.InventoryTransactions
            .Include(t => t.InventoryTransactionDetail)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId, ct);

        if (txn is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });

        var detail = txn.InventoryTransactionDetail;
        if (detail is null)
            return NotFound(new { message = $"Transaction detail for {transactionId} not found." });

        if (detail.IsVoided)
            return BadRequest(new { message = "Cannot edit a voided transaction." });

        // ── Detect weight changes on previously recorded values ─────────────
        bool startQtyChanged = detail.StartQty.HasValue && dto.StartQty != detail.StartQty;
        bool endQtyChanged   = detail.EndQty.HasValue   && dto.EndQty != detail.EndQty;
        bool weightsModified = startQtyChanged || endQtyChanged;

        string weightEditUserName = null;
        if (weightsModified)
        {
            if (!dto.WeightEditPin.HasValue || dto.WeightEditPin.Value <= 0)
                return BadRequest(new { message = "PIN is required to modify previously recorded weights.", requirePin = true });

            var pinUser = await _ctx.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Pin == dto.WeightEditPin.Value && u.IsActive, ct);

            if (pinUser is null)
                return Unauthorized(new { message = "Invalid or inactive PIN." });

            weightEditUserName = pinUser.UserName;
        }

        // Snapshot old weights for audit (before update)
        var oldWeights = new { detail.StartQty, detail.EndQty, detail.DirectQty };

        // ── Resolve lbs UOM ──────────────────────────────────────────────────
        var uomId = await _ctx.UnitOfMeasures
            .AsNoTracking()
            .Where(u => u.IsActive == true && u.Code == "LBS")
            .Select(u => u.UomId)
            .FirstOrDefaultAsync(ct);

        if (uomId == 0)
            return StatusCode(500, new { message = "LBS unit of measure is not configured in the database." });

        // ── Determine truck vs non-truck ────────────────────────────────────
        bool isTruck   = dto.StartQty.HasValue;
        bool hasDirect = dto.DirectQty.HasValue;

        if (!isTruck && !hasDirect)
            return BadRequest(new { message = "Either a truck weight (StartQty) or a direct quantity is required." });
        if (isTruck && hasDirect)
            return BadRequest(new { message = "Provide truck weights OR a direct quantity, not both." });

        // ── Resolve ProductId, ItemId, AccountId from the Lot ───────────────
        var lot = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LotId == dto.LotId)
            .Select(l => new
            {
                l.ProductId,
                l.ItemId,
                l.SplitGroupId,
                PrimaryAccountId = l.SplitGroup != null ? (long?)l.SplitGroup.PrimaryAccountId : null,
            })
            .FirstOrDefaultAsync(ct);

        if (lot is null)
            return BadRequest(new { message = $"Lot {dto.LotId} not found." });

        if (!lot.ProductId.HasValue)
            return BadRequest(new { message = $"Lot {dto.LotId} does not have a ProductId configured." });

        // ── Resolve CompletedAt per truck vs non-truck rules ────────────────
        DateTime? completedAt = isTruck
            ? (dto.EndQty.HasValue ? dto.CompletedAt ?? DateTime.UtcNow : null)
            : dto.CompletedAt ?? DateTime.UtcNow;

        // ── Update the detail record ─────────────────────────────────────────
        detail.LotId        = dto.LotId;
        detail.ProductId    = lot.ProductId.Value;
        detail.ItemId       = lot.ItemId;
        detail.TxnType      = "RECEIVE";
        detail.Direction    = 1;
        detail.UomId        = uomId;
        detail.AccountId    = lot.PrimaryAccountId;
        detail.SplitGroupId = lot.SplitGroupId;
        detail.StartQty     = isTruck ? dto.StartQty : null;
        detail.EndQty       = isTruck ? dto.EndQty : null;
        detail.DirectQty    = hasDirect ? dto.DirectQty : null;
        detail.StartQtyLocationQuantityMethodId          = isTruck ? dto.StartQtyLocationQuantityMethodId : null;
        detail.StartQtyLocationQuantityMethodDescription = isTruck ? dto.StartQtyLocationQuantityMethodDescription?.Trim() : null;
        detail.EndQtyLocationQuantityMethodId            = isTruck && dto.EndQty.HasValue ? dto.EndQtyLocationQuantityMethodId : null;
        detail.EndQtyLocationQuantityMethodDescription   = isTruck && dto.EndQty.HasValue ? dto.EndQtyLocationQuantityMethodDescription?.Trim() : null;
        detail.DirectQtyLocationQuantityMethodId         = hasDirect ? dto.DirectQtyLocationQuantityMethodId : null;
        detail.DirectQtyLocationQuantityMethodDescription = hasDirect ? dto.DirectQtyLocationQuantityMethodDescription?.Trim() : null;
        detail.StartedAt    = isTruck ? dto.StartedAt : dto.StartedAt ?? DateTime.UtcNow;
        detail.CompletedAt  = completedAt;
        detail.Notes        = dto.Notes;
        detail.UpdatedAt    = DateTime.UtcNow;
        detail.IsVoided     = false;
        detail.CreatedByUserId = dto.CreatedByUserId ?? detail.CreatedByUserId;

        // ── Update container splits ──────────────────────────────────────────
        var toSplitError = ValidateContainerSplits(dto.ToContainers, "destination");
        if (toSplitError != null)
            return BadRequest(new { message = toSplitError });

        var existingToContainers = await _ctx.InventoryTransactionDetailToContainers
            .Where(tc => tc.TransactionId == transactionId)
            .ToListAsync(ct);
        _ctx.InventoryTransactionDetailToContainers.RemoveRange(existingToContainers);

        if (dto.ToContainers?.Count > 0)
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
        }

        // ── Update quantity source records ───────────────────────────────────
        var existingSources = await _ctx.TransactionQuantitySources
            .Where(s => s.TransactionId == transactionId)
            .ToListAsync(ct);
        _ctx.TransactionQuantitySources.RemoveRange(existingSources);

        if (isTruck && dto.StartQtySourceTypeId.HasValue)
        {
            _ctx.TransactionQuantitySources.Add(new TransactionQuantitySource
            {
                TransactionId     = transactionId,
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
                TransactionId     = transactionId,
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
                TransactionId     = transactionId,
                QuantityField     = "DIRECT",
                MethodId          = dto.DirectQtyMethodId,
                SourceTypeId      = dto.DirectQtySourceTypeId.Value,
                Location          = dto.DirectQtyLocation?.Trim(),
                SourceDescription = dto.DirectQtySourceDescription?.Trim() ?? string.Empty,
            });
        }

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update GrowerDelivery TransactionId={TxnId}", transactionId);
            return StatusCode(500, new { message = "Database error while updating delivery." });
        }

        // ── Audit trail for weight modifications ────────────────────────────────
        if (weightsModified && weightEditUserName != null)
        {
            var newWeights = new { detail.StartQty, detail.EndQty, detail.DirectQty };
            string oldJson   = JsonSerializer.Serialize(oldWeights);
            string newJson   = JsonSerializer.Serialize(newWeights);
            string keyJson   = JsonSerializer.Serialize(new { TransactionId = transactionId });
            string tableName = "Inventory.InventoryTransactionDetails";
            string action    = "WT_EDIT";
            int locationId   = txn.LocationId;

            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
                VALUES ({locationId}, {weightEditUserName}, {tableName}, {action}, {keyJson}, {oldJson}, {newJson})",
                ct);
        }

        // ── Completion status ───────────────────────────────────────────────────
        bool hasQty       = isTruck ? dto.EndQty.HasValue : hasDirect;
        bool hasContainer = dto.ToContainers?.Count > 0;
        bool isComplete   = hasQty && hasContainer;

        return Ok(new
        {
            id = transactionId,
            isComplete,
            hasQuantity  = hasQty,
            hasContainer,
        });
    }

    // GET /api/GrowerDelivery/{transactionId}
    // Returns a single delivery transaction with detail, containers, and attributes for edit mode.
    [HttpGet("{transactionId:long}")]
    public async Task<IActionResult> Get(long transactionId, CancellationToken ct)
    {
        var detail = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .Include(d => d.Transaction)
            .FirstOrDefaultAsync(d => d.TransactionId == transactionId, ct);

        if (detail is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });

        // Container splits
        var containers = await _ctx.InventoryTransactionDetailToContainers
            .AsNoTracking()
            .Where(tc => tc.TransactionId == transactionId)
            .Select(tc => new { tc.ContainerId, tc.Percent })
            .ToListAsync(ct);

        // Quantity sources
        var sources = await _ctx.TransactionQuantitySources
            .AsNoTracking()
            .Where(s => s.TransactionId == transactionId)
            .Select(s => new { s.QuantityField, s.MethodId, s.SourceTypeId, s.Location, s.SourceDescription })
            .ToListAsync(ct);

        // Attributes (grain quality)
        var conn = (SqlConnection)_ctx.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync(ct);

        var attrSql = @"
            SELECT at.Code, ta.DecimalValue, ta.StringValue
            FROM [Inventory].[TransactionAttributes] ta
            INNER JOIN [Inventory].[TransactionAttributeTypes] at ON at.Id = ta.AttributeTypeId
            WHERE ta.TransactionId = @txnId";

        using var attrCmd = conn.CreateCommand();
        attrCmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
        attrCmd.CommandText = attrSql;
        attrCmd.Parameters.AddWithValue("@txnId", transactionId);

        var attributes = new Dictionary<string, object>();
        using (var reader = await attrCmd.ExecuteReaderAsync(ct))
        {
            while (await reader.ReadAsync(ct))
            {
                var code = reader.GetString(0);
                if (!reader.IsDBNull(1))
                    attributes[code] = reader.GetDecimal(1);
                else if (!reader.IsDBNull(2))
                    attributes[code] = reader.GetString(2);
            }
        }

        bool isTruck = detail.StartQty.HasValue;

        return Ok(new
        {
            detail.TransactionId,
            detail.LotId,
            detail.ProductId,
            detail.ItemId,
            LocationId = detail.Transaction.LocationId,
            detail.AccountId,
            detail.SplitGroupId,
            detail.StartQty,
            detail.EndQty,
            detail.DirectQty,
            detail.StartedAt,
            detail.CompletedAt,
            detail.Notes,
            detail.RefType,
            detail.RefId,
            detail.IsVoided,
            detail.VoidReason,
            detail.CreatedByUserId,
            IsTruck = isTruck,
            // Quantity method snapshots
            detail.StartQtyLocationQuantityMethodId,
            detail.StartQtyLocationQuantityMethodDescription,
            detail.EndQtyLocationQuantityMethodId,
            detail.EndQtyLocationQuantityMethodDescription,
            detail.DirectQtyLocationQuantityMethodId,
            detail.DirectQtyLocationQuantityMethodDescription,
            // Related
            ToContainers = containers,
            Sources = sources,
            Attributes = attributes,
        });
    }

    // GET /api/GrowerDelivery/ValidatePin?pin=
    [HttpGet("ValidatePin")]
    public async Task<IActionResult> ValidatePin([FromQuery] int pin, CancellationToken ct)
    {
        if (pin <= 0)
            return BadRequest(new { message = "PIN is required." });

        var user = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == pin && u.IsActive, ct);

        if (user is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        return Ok(new { UserId = user.UserId, UserName = user.UserName });
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
                ws.RowUid,
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
                LoadCount = _ctx.InventoryTransactionDetails.Count(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet"),
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

        // ── PIN validation ──────────────────────────────────────────────────
        if (!dto.Pin.HasValue || dto.Pin.Value <= 0)
            return BadRequest(new { message = "PIN is required to create a weight sheet." });

        var pinUser = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin.Value && u.IsActive, ct);

        if (pinUser is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

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
                    (LocationId, LotId, WeightSheetType, RateType, HaulerId, Miles, CustomRateDescription, Rate, CreationDate, CreatedAt, RowUid, WeightmasterName)
                VALUES
                    ({dto.LocationId}, {dto.LotId}, {sheetType}, {dto.RateType}, {dto.HaulerId}, {dto.Miles}, {dto.CustomRateDescription}, {dto.Rate}, {creationDate}, {now}, {wsRowUid}, {pinUser.UserName})",
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
                itd.TransactionId,
                itd.StartQty            AS InWeight,
                itd.EndQty              AS OutWeight,
                itd.NetQty              AS Net,
                itd.TxnAt,
                itd.StartedAt,
                itd.CompletedAt,
                itd.Notes,
                tc.ContainerId          AS ContainerId,
                c.Description           AS ContainerDescription,
                tc.[Percent]            AS ContainerPercent,
                attr1.DecimalValue      AS Attr1Value,
                attr1.StringValue       AS Attr1String,
                at1.Description         AS Attr1Description,
                attr2.DecimalValue      AS Attr2Value,
                attr2.StringValue       AS Attr2String,
                at2.Description         AS Attr2Description,
                lot.LotDescription,
                lot.LotId,
                CASE WHEN startSrcType.Code = 'MANUAL' THEN 1 ELSE 0 END AS StartIsManual,
                CASE WHEN endSrcType.Code = 'MANUAL' THEN 1 ELSE 0 END AS EndIsManual
            FROM [warehouse].[WeightSheets] ws
            LEFT JOIN [account].[Haulers] h
                ON h.Id = ws.HaulerId
            INNER JOIN [Inventory].[InventoryTransactionDetails] itd
                ON itd.RefId = ws.RowUid AND itd.RefType = 'WeightSheet'
            LEFT JOIN [Inventory].[InventoryTransactionDetailToContainers] tc
                ON tc.TransactionId = itd.TransactionId
            LEFT JOIN [container].[Containers] c
                ON c.ContainerId = tc.ContainerId
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
            LEFT JOIN [Inventory].[TransactionQuantitySources] startSrc
                ON startSrc.TransactionId = itd.TransactionId AND startSrc.QuantityField = 'START'
            LEFT JOIN [Inventory].[TransactionQuantitySources] endSrc
                ON endSrc.TransactionId = itd.TransactionId AND endSrc.QuantityField = 'END'
            LEFT JOIN [Inventory].[QuantitySourceTypes] startSrcType
                ON startSrcType.QuantitySourceTypeId = startSrc.SourceTypeId
            LEFT JOIN [Inventory].[QuantitySourceTypes] endSrcType
                ON endSrcType.QuantitySourceTypeId = endSrc.SourceTypeId
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
                TransactionId          = reader.GetInt64(reader.GetOrdinal("TransactionId")),
                InWeight               = reader.IsDBNull(reader.GetOrdinal("InWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("InWeight")),
                OutWeight              = reader.IsDBNull(reader.GetOrdinal("OutWeight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("OutWeight")),
                Net                    = reader.IsDBNull(reader.GetOrdinal("Net")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Net")),
                TxnAt                  = reader.GetDateTime(reader.GetOrdinal("TxnAt")),
                StartedAt              = reader.IsDBNull(reader.GetOrdinal("StartedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("StartedAt")),
                CompletedAt            = reader.IsDBNull(reader.GetOrdinal("CompletedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("CompletedAt")),
                Notes                  = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes")),
                ContainerId            = reader.IsDBNull(reader.GetOrdinal("ContainerId")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("ContainerId")),
                ContainerDescription   = reader.IsDBNull(reader.GetOrdinal("ContainerDescription")) ? null : reader.GetString(reader.GetOrdinal("ContainerDescription")),
                ContainerPercent       = reader.IsDBNull(reader.GetOrdinal("ContainerPercent")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("ContainerPercent")),
                Attr1Value             = reader.IsDBNull(reader.GetOrdinal("Attr1Value")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Attr1Value")),
                Attr1String            = reader.IsDBNull(reader.GetOrdinal("Attr1String")) ? null : reader.GetString(reader.GetOrdinal("Attr1String")),
                Attr1Description       = reader.IsDBNull(reader.GetOrdinal("Attr1Description")) ? null : reader.GetString(reader.GetOrdinal("Attr1Description")),
                Attr2Value             = reader.IsDBNull(reader.GetOrdinal("Attr2Value")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Attr2Value")),
                Attr2String            = reader.IsDBNull(reader.GetOrdinal("Attr2String")) ? null : reader.GetString(reader.GetOrdinal("Attr2String")),
                Attr2Description       = reader.IsDBNull(reader.GetOrdinal("Attr2Description")) ? null : reader.GetString(reader.GetOrdinal("Attr2Description")),
                LotDescription         = reader.IsDBNull(reader.GetOrdinal("LotDescription")) ? null : reader.GetString(reader.GetOrdinal("LotDescription")),
                LotId                  = reader.IsDBNull(reader.GetOrdinal("LotId")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("LotId")),
                StartIsManual          = reader.GetInt32(reader.GetOrdinal("StartIsManual")) == 1,
                EndIsManual            = reader.GetInt32(reader.GetOrdinal("EndIsManual")) == 1,
            });
        }

        return Ok(results);
    }

    // POST /api/GrowerDelivery/WeightSheetDeliveryLoads/UpdateBin
    [HttpPost("WeightSheetDeliveryLoads/UpdateBin")]
    public async Task<IActionResult> UpdateDeliveryLoadBin(
        [FromBody] UpdateLoadBinDto dto,
        CancellationToken ct)
    {
        if (dto.TransactionId <= 0)
            return BadRequest(new { message = "TransactionId is required." });

        var transactionId = dto.TransactionId;

        // Remove existing destination container splits
        var existing = await _ctx.InventoryTransactionDetailToContainers
            .Where(tc => tc.TransactionId == transactionId)
            .ToListAsync(ct);
        _ctx.InventoryTransactionDetailToContainers.RemoveRange(existing);

        // Insert new single-container split at 100%
        if (dto.ContainerId > 0)
        {
            _ctx.InventoryTransactionDetailToContainers.Add(new InventoryTransactionDetailToContainer
            {
                TransactionId = transactionId,
                ContainerId   = dto.ContainerId,
                Percent       = 100m,
            });
        }

        await _ctx.SaveChangesAsync(ct);
        return Ok();
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

    // POST /api/GrowerDelivery/WeightSheetDeliveryLoads/UpdateNotes
    [HttpPost("WeightSheetDeliveryLoads/UpdateNotes")]
    public async Task<IActionResult> UpdateDeliveryLoadNotes(
        [FromBody] UpdateLoadNotesDto dto,
        CancellationToken ct)
    {
        if (dto.TransactionId <= 0)
            return BadRequest(new { message = "TransactionId is required." });

        var detail = await _ctx.InventoryTransactionDetails
            .FirstOrDefaultAsync(d => d.TransactionId == dto.TransactionId, ct);

        if (detail is null)
            return NotFound(new { message = "Transaction not found." });

        detail.Notes     = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
        detail.UpdatedAt = DateTime.UtcNow;

        await _ctx.SaveChangesAsync(ct);
        return Ok(new { success = true });
    }

    // GET /api/GrowerDelivery/LotSplitGroup/{lotId} — split group accounts + percentages
    [HttpGet("LotSplitGroup/{lotId:long}")]
    public async Task<IActionResult> GetLotSplitGroup(long lotId, CancellationToken ct)
    {
        var rows = await _ctx.LotSplitGroups
            .Where(lsg => lsg.LotId == lotId)
            .Include(lsg => lsg.Account)
            .OrderByDescending(lsg => lsg.PrimaryAccount)
            .ThenByDescending(lsg => lsg.SplitPercent)
            .Select(lsg => new {
                lsg.AccountId,
                AccountName = lsg.Account.EntityName ?? lsg.Account.LookupName,
                lsg.SplitPercent,
                lsg.PrimaryAccount,
            })
            .AsNoTracking()
            .ToListAsync(ct);

        return Ok(rows);
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
                ws.As400Id      AS WsAs400Id,
                ws.ServerId,
                ws.LocationId,
                ws.Notes        AS WsNotes,
                ws.HaulerId,
                h.Description   AS HaulerName,
                ws.RateType,
                ws.CustomRateDescription,
                ws.Miles,
                ws.Rate,
                ws.LotId,
                lot.As400Id     AS LotAs400Id,
                lot.LotDescription,
                lot.ServerId    AS LotServerId,
                lot.LocationId  AS LotLocationId,
                p.Description   AS CropName,
                sg.SplitGroupId,
                sg.SplitGroupDescription AS SplitName,
                ws.WeightmasterName,
                ws.CreationDate,
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
            WsAs400Id          = reader.IsDBNull(reader.GetOrdinal("WsAs400Id")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("WsAs400Id")),
            ServerId           = reader.GetInt32(reader.GetOrdinal("ServerId")),
            LocationId         = reader.GetInt32(reader.GetOrdinal("LocationId")),
            WsNotes            = reader.IsDBNull(reader.GetOrdinal("WsNotes")) ? null : reader.GetString(reader.GetOrdinal("WsNotes")),
            HaulerId           = reader.IsDBNull(reader.GetOrdinal("HaulerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("HaulerId")),
            HaulerName         = reader.IsDBNull(reader.GetOrdinal("HaulerName")) ? null : reader.GetString(reader.GetOrdinal("HaulerName")),
            RateType           = reader.IsDBNull(reader.GetOrdinal("RateType")) ? null : reader.GetString(reader.GetOrdinal("RateType")),
            CustomRateDescription = reader.IsDBNull(reader.GetOrdinal("CustomRateDescription")) ? null : reader.GetString(reader.GetOrdinal("CustomRateDescription")),
            Miles              = reader.IsDBNull(reader.GetOrdinal("Miles")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Miles")),
            Rate               = reader.IsDBNull(reader.GetOrdinal("Rate")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Rate")),
            LotId              = reader.IsDBNull(reader.GetOrdinal("LotId")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("LotId")),
            LotAs400Id         = reader.IsDBNull(reader.GetOrdinal("LotAs400Id")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("LotAs400Id")),
            LotDescription     = reader.IsDBNull(reader.GetOrdinal("LotDescription")) ? null : reader.GetString(reader.GetOrdinal("LotDescription")),
            LotServerId        = reader.IsDBNull(reader.GetOrdinal("LotServerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LotServerId")),
            LotLocationId      = reader.IsDBNull(reader.GetOrdinal("LotLocationId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LotLocationId")),
            CropName           = reader.IsDBNull(reader.GetOrdinal("CropName")) ? null : reader.GetString(reader.GetOrdinal("CropName")),
            SplitGroupId       = reader.IsDBNull(reader.GetOrdinal("SplitGroupId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SplitGroupId")),
            SplitName          = reader.IsDBNull(reader.GetOrdinal("SplitName")) ? null : reader.GetString(reader.GetOrdinal("SplitName")),
            PrimaryAccountName = reader.IsDBNull(reader.GetOrdinal("PrimaryAccountName")) ? null : reader.GetString(reader.GetOrdinal("PrimaryAccountName")),
            WeightmasterName   = reader.IsDBNull(reader.GetOrdinal("WeightmasterName")) ? null : reader.GetString(reader.GetOrdinal("WeightmasterName")),
            CreationDate       = reader.GetDateTime(reader.GetOrdinal("CreationDate")).ToString("MM/dd/yyyy"),
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

        // Use raw SQL since WeightSheets may be a keyless entity in EF
        var sets = new List<string>();
        var parameters = new List<SqlParameter>();

        if (dto.HaulerId.HasValue)
        {
            sets.Add("HaulerId = @haulerId");
            parameters.Add(new SqlParameter("@haulerId", dto.HaulerId.Value == 0 ? DBNull.Value : dto.HaulerId.Value));
            ws.HaulerId = dto.HaulerId.Value == 0 ? null : dto.HaulerId;
        }

        if (dto.LotId.HasValue)
        {
            sets.Add("LotId = @lotId");
            parameters.Add(new SqlParameter("@lotId", dto.LotId.Value == 0 ? DBNull.Value : dto.LotId.Value));
            ws.LotId = dto.LotId.Value == 0 ? null : dto.LotId;
        }

        if (dto.Notes != null)
        {
            var notesVal = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
            sets.Add("Notes = @notes");
            parameters.Add(new SqlParameter("@notes", (object)notesVal ?? DBNull.Value));
            ws.Notes = notesVal;
        }

        if (dto.RateType != null)
        {
            sets.Add("RateType = @rateType");
            parameters.Add(new SqlParameter("@rateType", dto.RateType));
        }

        if (dto.Miles.HasValue)
        {
            sets.Add("Miles = @miles");
            parameters.Add(new SqlParameter("@miles", dto.Miles.Value));
        }

        if (dto.CustomRateDescription != null)
        {
            var descVal = string.IsNullOrWhiteSpace(dto.CustomRateDescription) ? null : dto.CustomRateDescription.Trim();
            sets.Add("CustomRateDescription = @customRateDesc");
            parameters.Add(new SqlParameter("@customRateDesc", (object)descVal ?? DBNull.Value));
        }

        if (dto.Rate.HasValue)
        {
            sets.Add("Rate = @rate");
            parameters.Add(new SqlParameter("@rate", dto.Rate.Value));
        }

        sets.Add("UpdatedAt = @updatedAt");
        parameters.Add(new SqlParameter("@updatedAt", DateTime.UtcNow));
        parameters.Add(new SqlParameter("@wsId", id));
        parameters.Add(new SqlParameter("@serverId", _systemInfo.ServerId));

        if (sets.Count > 1) // more than just UpdatedAt
        {
            var sql = $"UPDATE [warehouse].[WeightSheets] SET {string.Join(", ", sets)} WHERE WeightSheetId = @wsId AND ServerId = @serverId";
            var conn = (SqlConnection)_ctx.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync(ct);
            using var cmd = conn.CreateCommand();
            cmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
            cmd.CommandText = sql;
            cmd.Parameters.AddRange(parameters.ToArray());
            await cmd.ExecuteNonQueryAsync(ct);
        }

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
            if (value.HasValue && value.Value != 0m)
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

    /// <summary>
    /// Validates a list of container split rows. Returns an error message or null if valid.
    /// Null/empty splits are allowed (container assignment is optional).
    /// </summary>
    private static string ValidateContainerSplits(List<ContainerSplitDto> splits, string side)
    {
        if (splits == null || splits.Count == 0)
            return null;

        if (splits.Any(s => s.Percent <= 0))
            return $"All {side} container percentages must be greater than zero.";

        if (splits.Select(s => s.ContainerId).Distinct().Count() != splits.Count)
            return $"Duplicate container IDs in {side} container splits.";

        var sum = splits.Sum(s => s.Percent);
        if (Math.Abs(sum - 100m) > 0.0001m)
            return $"The {side} container percentages must total 100 (got {sum}).";

        return null;
    }

    // GET /api/GrowerDelivery/{transactionId}/void-eligibility
    // Returns whether this delivery ticket can be voided or restored.
    [HttpGet("{transactionId:long}/void-eligibility")]
    public async Task<IActionResult> GetVoidEligibility(long transactionId, CancellationToken ct)
    {
        var detail = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.TransactionId == transactionId, ct);

        if (detail is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });

        bool createdToday = detail.TxnAt.Date == DateTime.UtcNow.Date;

        return Ok(new
        {
            TransactionId = transactionId,
            IsVoided      = detail.IsVoided,
            CanVoid       = !detail.IsVoided && createdToday,
            CanRestore    = detail.IsVoided,
            Reason        = !createdToday && !detail.IsVoided
                ? "Ticket can only be voided on the same calendar day it was created."
                : null,
        });
    }

    // POST /api/GrowerDelivery/{transactionId}/void
    // Voids a delivery ticket. Requires PIN, void reason, and same-day creation.
    [HttpPost("{transactionId:long}/void")]
    public async Task<IActionResult> VoidDelivery(
        long transactionId,
        [FromBody] VoidDeliveryDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.Pin <= 0)
            return BadRequest(new { message = "PIN is required." });
        if (string.IsNullOrWhiteSpace(dto.VoidReason))
            return BadRequest(new { message = "Void reason is required." });

        // PIN validation
        var pinUser = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin && u.IsActive, ct);

        if (pinUser is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        // Load transaction detail
        var txn = await _ctx.InventoryTransactions
            .Include(t => t.InventoryTransactionDetail)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId, ct);

        if (txn is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });

        var detail = txn.InventoryTransactionDetail;
        if (detail is null)
            return NotFound(new { message = $"Transaction detail for {transactionId} not found." });

        if (detail.IsVoided)
            return BadRequest(new { message = "Transaction is already voided." });

        // Same-day check
        bool createdToday = detail.TxnAt.Date == DateTime.UtcNow.Date;
        if (!createdToday)
            return BadRequest(new { message = "Ticket can only be voided on the same calendar day it was created." });

        // Snapshot old values for audit
        string oldJson = JsonSerializer.Serialize(new
        {
            detail.IsVoided,
            detail.VoidReason,
            detail.VoidedByUserName,
        });

        // Apply void
        detail.IsVoided          = true;
        detail.VoidReason        = dto.VoidReason.Trim();
        detail.VoidedByUserName  = pinUser.UserName;
        detail.UpdatedAt         = DateTime.UtcNow;

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to void delivery TransactionId={TxnId}", transactionId);
            return StatusCode(500, new { message = "Database error while voiding delivery." });
        }

        // Audit trail
        string newJson = JsonSerializer.Serialize(new
        {
            detail.IsVoided,
            detail.VoidReason,
            detail.VoidedByUserName,
        });
        string keyJson   = JsonSerializer.Serialize(new { TransactionId = transactionId });
        string tableName = "Inventory.InventoryTransactionDetails";
        string action    = "VOID";
        int locationId   = txn.LocationId;

        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
            VALUES ({locationId}, {pinUser.UserName}, {tableName}, {action}, {keyJson}, {oldJson}, {newJson})",
            ct);

        return Ok(new { id = transactionId, isVoided = true });
    }

    // POST /api/GrowerDelivery/{transactionId}/restore
    // Restores (unvoids) a previously voided delivery ticket.
    [HttpPost("{transactionId:long}/restore")]
    public async Task<IActionResult> RestoreDelivery(
        long transactionId,
        [FromBody] RestoreDeliveryDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.Pin <= 0)
            return BadRequest(new { message = "PIN is required." });

        // PIN validation
        var pinUser = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin && u.IsActive, ct);

        if (pinUser is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        // Load transaction detail
        var txn = await _ctx.InventoryTransactions
            .Include(t => t.InventoryTransactionDetail)
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId, ct);

        if (txn is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });

        var detail = txn.InventoryTransactionDetail;
        if (detail is null)
            return NotFound(new { message = $"Transaction detail for {transactionId} not found." });

        if (!detail.IsVoided)
            return BadRequest(new { message = "Transaction is not voided." });

        // Snapshot old values for audit
        string oldJson = JsonSerializer.Serialize(new
        {
            detail.IsVoided,
            detail.VoidReason,
            detail.VoidedByUserName,
        });

        // Restore
        detail.IsVoided          = false;
        detail.VoidReason        = null;
        detail.VoidedByUserName  = null;
        detail.UpdatedAt         = DateTime.UtcNow;

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to restore delivery TransactionId={TxnId}", transactionId);
            return StatusCode(500, new { message = "Database error while restoring delivery." });
        }

        // Audit trail
        string newJson = JsonSerializer.Serialize(new
        {
            detail.IsVoided,
            detail.VoidReason,
            VoidedByUserName = (string)null,
            RestoredByUserName = pinUser.UserName,
        });
        string keyJson   = JsonSerializer.Serialize(new { TransactionId = transactionId });
        string tableName = "Inventory.InventoryTransactionDetails";
        string action    = "RESTORE";
        int locationId   = txn.LocationId;

        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
            VALUES ({locationId}, {pinUser.UserName}, {tableName}, {action}, {keyJson}, {oldJson}, {newJson})",
            ct);

        return Ok(new { id = transactionId, isVoided = false });
    }

    // GET /api/GrowerDelivery/{transactionId}/completion-status
    // Returns whether a delivery load is complete (quantity + container requirements met).
    [HttpGet("{transactionId:long}/completion-status")]
    public async Task<IActionResult> GetCompletionStatus(long transactionId, CancellationToken ct)
    {
        var detail = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.TransactionId == transactionId, ct);

        if (detail is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });

        bool isTruck       = detail.StartQty.HasValue;
        bool hasQuantity   = isTruck ? detail.EndQty.HasValue : detail.DirectQty.HasValue;
        bool hasContainer  = await _ctx.InventoryTransactionDetailToContainers
            .AsNoTracking()
            .AnyAsync(tc => tc.TransactionId == transactionId, ct);

        bool isComplete = hasQuantity && hasContainer;

        var messages = new List<string>();
        if (!hasQuantity)
            messages.Add(isTruck ? "EndQty (scale-out weight) is required to complete the load." : "DirectQty is required to complete the load.");
        if (!hasContainer)
            messages.Add("At least one container must be selected to complete the load.");

        return Ok(new
        {
            TransactionId = transactionId,
            IsComplete    = isComplete,
            HasQuantity   = hasQuantity,
            HasContainer  = hasContainer,
            IsTruck       = isTruck,
            Messages      = messages,
        });
    }
}
