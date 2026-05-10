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
    // Business rule: a weight sheet transitions out of Open (StatusId 0) once
    // it accumulates this many non-voided loads. At/above this threshold the
    // sheet becomes Pending — finished or not depending on whether every load
    // has outbound weight, protein, and a destination bin set.
    private const int WeightSheetMaxLoads = 25;

    // Status constants mirror warehouse.WeightSheetStatuses (see
    // SQL/AddWeightSheetStatus.sql). Kept here so the controller doesn't have
    // magic numbers scattered through it.
    private const byte StatusOpen               = 0;
    private const byte StatusPendingNotFinished = 1;
    private const byte StatusPendingFinished    = 2;
    private const byte StatusClosed             = 3;

    private readonly dbContext _ctx;
    private readonly ILogger<GrowerDeliveryApiController> _logger;
    private readonly SystemInfoSnapshot _systemInfo;
    private readonly IWeightSheetNotifier _notifier;
    private readonly GrainManagement.Services.Warehouse.IPriorDayWeightSheetGuard _priorDayGuard;
    private readonly GrainManagement.Services.Camera.ICameraCaptureTrigger _cameraTrigger;

    public GrowerDeliveryApiController(
        dbContext ctx,
        ILogger<GrowerDeliveryApiController> logger,
        SystemInfoSnapshot systemInfo,
        IWeightSheetNotifier notifier,
        GrainManagement.Services.Warehouse.IPriorDayWeightSheetGuard priorDayGuard,
        GrainManagement.Services.Camera.ICameraCaptureTrigger cameraTrigger)
    {
        _ctx = ctx;
        _logger = logger;
        _systemInfo = systemInfo;
        _notifier = notifier;
        _priorDayGuard = priorDayGuard;
        _cameraTrigger = cameraTrigger;
    }

    /// <summary>
    /// Returns a 409 Conflict result when there are open weight sheets at
    /// the given location that were created on a prior day, so creation
    /// endpoints (new WS, new load) refuse to add today's work until the
    /// operator runs End Of Day on those stale sheets. Returns null when
    /// it's clear to proceed.
    /// </summary>
    private async Task<IActionResult?> RejectIfPriorDayOpenAsync(int locationId, CancellationToken ct)
    {
        var stale = await _priorDayGuard.GetPriorDayOpenWeightSheetIdsAsync(locationId, ct);
        if (stale.Count == 0) return null;
        return Conflict(new
        {
            message = stale.Count == 1
                ? "There is an open weight sheet from a previous day. Run End Of Day on it before creating new work."
                : $"There are {stale.Count} open weight sheets from previous days. Run End Of Day on them before creating new work.",
            staleWeightSheetIds = stale,
        });
    }

    // Resolves a user-name to attribute audit rows to when no PIN was required
    // (e.g. SET_PENDING / REOPEN). Reads the Remote-Admin cookie set by
    // AuthController.ValidateRemoteAdminPin; falls back to "system" otherwise.
    private string ResolveAuditUserName()
    {
        try
        {
            var req = HttpContext?.Request;
            if (req != null && req.Cookies.TryGetValue("GrainMgmt_RemoteAdminUser", out var raRaw))
            {
                var name = Uri.UnescapeDataString(raRaw ?? "");
                if (!string.IsNullOrWhiteSpace(name)) return name;
            }
        }
        catch { /* fall through */ }
        return "system";
    }

    // GET /api/GrowerDelivery/Location/{locationId}/AddWeightSheetCheck
    //
    // Returns 200 OK when a new weight sheet can be created at this
    // location, or 4xx with { message } describing why not. Mirrors the
    // prior-day gate that POST /api/GrowerDelivery/WeightSheets enforces at
    // save-time so the New Weight Sheet button can fail fast instead of
    // letting the operator fill the entire form first.
    [HttpGet("Location/{locationId:int}/AddWeightSheetCheck")]
    public async Task<IActionResult> AddWeightSheetCheck(int locationId, CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        var stale = await RejectIfPriorDayOpenAsync(locationId, ct);
        if (stale != null) return stale;

        return Ok();
    }

    // GET /api/GrowerDelivery/WeightSheet/{wsId}/AddLoadCheck
    //
    // Returns 200 OK when a new load can be added to this weight sheet, or
    // 4xx with { message } describing why not. Mirrors the gates that POST
    // /api/GrowerDelivery enforces at save-time so the UI can fail fast on
    // the New Load button click instead of after the operator fills the
    // entire load form.
    [HttpGet("WeightSheet/{wsId:long}/AddLoadCheck")]
    public async Task<IActionResult> AddLoadCheck(long wsId, CancellationToken ct)
    {
        var ws = await _ctx.WeightSheets.AsNoTracking()
            .Where(w => w.WeightSheetId == wsId)
            .Select(w => new { w.LotId, w.LocationId, w.StatusId })
            .FirstOrDefaultAsync(ct);
        if (ws is null)
            return NotFound(new { message = "Weight sheet not found." });

        // StatusId > 0 means Pending or Closed — no new loads regardless of
        // lot or prior-day state.
        if (ws.StatusId > 0)
            return Conflict(new { message = "This weight sheet is no longer open. New loads cannot be added." });

        if (!ws.LotId.HasValue)
            return BadRequest(new { message = "This weight sheet has no lot assigned." });

        var lotIsOpen = await _ctx.Lots.AsNoTracking()
            .Where(l => l.LotId == ws.LotId.Value)
            .Select(l => (bool?)l.IsOpen)
            .FirstOrDefaultAsync(ct);
        if (lotIsOpen != true)
            return Conflict(new { message = "This lot has been closed. New loads cannot be added to weight sheets on a closed lot." });

        var stale = await RejectIfPriorDayOpenAsync(ws.LocationId, ct);
        if (stale != null) return stale;

        return Ok();
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

        // ── Prior-day guard: refuse new loads while yesterday's WSs are open ──
        var staleResult = await RejectIfPriorDayOpenAsync(dto.LocationId, ct);
        if (staleResult != null) return staleResult;

        // ── Closed-lot guard: block new loads on a closed lot ────────────────
        var lotStatus = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LotId == dto.LotId)
            .Select(l => new { l.IsOpen })
            .FirstOrDefaultAsync(ct);
        if (lotStatus is null)
            return BadRequest(new { message = "Lot not found." });
        if (!lotStatus.IsOpen)
            return BadRequest(new { message = "This lot has been closed. New loads cannot be added to weight sheets on a closed lot." });

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
        // PrimaryAccountId is taken from the lot's own LotSplitGroups row flagged
        // PrimaryAccount = 1 (the authoritative per-lot primary), NOT from
        // SplitGroup.PrimaryAccountId — override-mode groups have a null value
        // there and would otherwise drop the account on the transaction.
        var lot = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LotId == dto.LotId)
            .Select(l => new
            {
                l.ProductId,
                l.ItemId,
                l.SplitGroupId,
                PrimaryAccountId = _ctx.LotSplitGroups
                                       .Where(lsg => lsg.LotId == l.LotId && lsg.PrimaryAccount)
                                       .Select(lsg => (long?)lsg.AccountId)
                                       .FirstOrDefault(),
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

        // ── Insert TransactionAttributes via EF tracking ─────────────────────
        // Entity is keyed by TransactionAttributesUid; we let EF track + SaveChanges.
        var attributes = BuildAttributes(dto);

        if (attributes.Count > 0)
        {
            // Load all active attribute type codes in a single query
            var codes = attributes.Select(a => a.Code).ToHashSet();
            var typeMap = await _ctx.TransactionAttributeTypes
                .AsNoTracking()
                .Where(t => t.IsActive == true && codes.Contains(t.Code))
                .ToDictionaryAsync(t => t.Code, t => t.Id, ct);

            var attrNow = DateTime.UtcNow;
            foreach (var attr in attributes)
            {
                if (!typeMap.TryGetValue(attr.Code, out var typeId))
                {
                    _logger.LogWarning("TransactionAttributeType not found for code {Code}", attr.Code);
                    continue;
                }

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

            await _ctx.SaveChangesAsync(ct);
        }

        // IS_END_DUMP carries a bool — out-of-band from BuildAttributes
        // which only handles decimal/string values. Skipped silently
        // when the DTO didn't include an answer.
        await WriteIsEndDumpAsync(transactionId, dto.IsEndDump, ct);

        // ── Link to weight sheet directly via RefId ────────────────────────────
        //
        // If the target weight sheet is already at or above the load cap, spill
        // this new load onto a fresh weight sheet that copies the lot + hauler
        // header of the full one. The full sheet is then finalized via
        // RecomputeWeightSheetStatusAsync (Open → PendingFinished or
        // PendingNotFinished depending on whether every load has outbound
        // weight, protein, and a bin set).
        Guid? overflowWsUid    = null;
        long? overflowWsId     = null;
        if (dto.WeightSheetUid.HasValue)
        {
            var linkWsUid = dto.WeightSheetUid.Value;

            var existingCount = await _ctx.InventoryTransactionDetails
                .CountAsync(itd => itd.RefType == "WeightSheet"
                                && itd.RefId   == linkWsUid
                                && !itd.IsVoided, ct);

            if (existingCount >= WeightSheetMaxLoads)
            {
                // Spill to a new sheet. The helper inserts a new row via raw
                // SQL (so the AutoGenerateIDs trigger generates WeightSheetId)
                // and returns its RowUid.
                var spill = await CreateOverflowWeightSheetAsync(linkWsUid, ct);
                if (spill.NewWsRowUid is null)
                {
                    _logger.LogError(
                        "Failed to create overflow weight sheet for full WS RowUid={Uid}", linkWsUid);
                    return StatusCode(500, new { message = "Database error while spilling to a new weight sheet." });
                }

                overflowWsUid = spill.NewWsRowUid.Value;
                overflowWsId  = spill.NewWsId;

                // The new load goes onto the overflow sheet.
                detail.RefType = "WeightSheet";
                detail.RefId   = overflowWsUid.Value;
            }
            else
            {
                detail.RefType = "WeightSheet";
                detail.RefId   = linkWsUid;
            }

            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to link delivery to weight sheet for TransactionId={TxnId}", transactionId);
                return StatusCode(500, new { message = "Database error while linking to weight sheet." });
            }

            // Recompute the status of the sheet(s) we touched.
            //   - If we spilled, the original (full) sheet needs to transition
            //     from Open → Pending* now that no more loads will be added.
            //   - Otherwise, the sheet we just added to may now be at the cap
            //     and ready to transition.
            await RecomputeWeightSheetStatusAsync(linkWsUid, ct);
            if (overflowWsUid.HasValue)
            {
                // Also recompute the new sheet — it's just one load so it'll
                // stay Open, but the helper is a no-op in that case and keeps
                // the logic uniform.
                await RecomputeWeightSheetStatusAsync(overflowWsUid.Value, ct);
            }
        }

        // ── Completion status ───────────────────────────────────────────────────
        bool hasQty       = isTruck ? dto.EndQty.HasValue : hasDirect;
        bool hasContainer = dto.ToContainers?.Count > 0;
        bool isComplete   = hasQty && hasContainer;

        // Push WS-update notifications so the dashboard + open loads pages
        // refresh without operator action. Notify both the original target
        // sheet and the spill sheet (if any).
        var primaryWsId = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(w => w.RowUid == detail.RefId)
            .Select(w => (long?)w.WeightSheetId)
            .FirstOrDefaultAsync(ct);
        if (primaryWsId.HasValue)
            await _notifier.NotifyAsync(dto.LocationId, primaryWsId.Value, "load-created", ct);
        if (overflowWsId.HasValue)
            await _notifier.NotifyAsync(dto.LocationId, overflowWsId.Value, "ws-created", ct);

        // Camera capture — fire whenever a weight was recorded as part of this
        // create. Inbound on StartQty, outbound on EndQty. Pure fire-and-forget;
        // see CameraCaptureTrigger.
        var loadKey = transactionId.ToString();
        if (isTruck && dto.StartQty.HasValue)
            await _cameraTrigger.FireAsync(loadKey, "in",  dto.LocationId, scaleId: null, ct);
        if (isTruck && dto.EndQty.HasValue)
            await _cameraTrigger.FireAsync(loadKey, "out", dto.LocationId, scaleId: null, ct);

        return Ok(new
        {
            id         = transactionId,
            isComplete,
            hasQuantity  = hasQty,
            hasContainer,
            // Overflow info: non-null when the target WS was already at the
            // load cap and this ticket was moved to a fresh sheet. The client
            // can use these to redirect the user to the new sheet's page.
            spilledToWeightSheetId  = overflowWsId,
            spilledToWeightSheetUid = overflowWsUid,
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
        // PrimaryAccountId from LotSplitGroups.PrimaryAccount = 1 — see notes on
        // the other transaction-creation path; override-mode groups need this.
        var lot = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LotId == dto.LotId)
            .Select(l => new
            {
                l.ProductId,
                l.ItemId,
                l.SplitGroupId,
                PrimaryAccountId = _ctx.LotSplitGroups
                                       .Where(lsg => lsg.LotId == l.LotId && lsg.PrimaryAccount)
                                       .Select(lsg => (long?)lsg.AccountId)
                                       .FirstOrDefault(),
            })
            .FirstOrDefaultAsync(ct);

        if (lot is null)
            return BadRequest(new { message = $"Lot {dto.LotId} not found." });

        if (!lot.ProductId.HasValue)
            return BadRequest(new { message = $"Lot {dto.LotId} does not have a ProductId configured." });

        // ── Update the detail record ─────────────────────────────────────────
        // StartedAt is preserved when already set (the original truck-in moment).
        // CompletedAt is now recorded on edit when an Out weight is added —
        // weighing-out a previously in-only load needs a completion time so
        // reports + status checks see it as finished. The audit trail still
        // records the value change separately.
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
        detail.Notes        = dto.Notes;
        detail.UpdatedAt    = DateTime.UtcNow;
        detail.IsVoided     = false;
        detail.CreatedByUserId = dto.CreatedByUserId ?? detail.CreatedByUserId;

        // Capture timestamps. StartedAt is preserved if already set; CompletedAt
        // is recorded the first time an Out weight is captured on edit.
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

        // ── Upsert TransactionAttributes (grain quality + transport + free-text) ──
        // For each attribute, delete the existing row (if any) and insert the new
        // value when one was supplied. This keeps the edit flow aligned with Create
        // and uses code-based lookup so swapped IDs don't corrupt the data.
        _logger.LogInformation(
            "GrowerDelivery Update attributes TxnId={TxnId} BOL='{BOL}' Truck='{Truck}' Driver='{Driver}'",
            transactionId, dto.BOL ?? "<null>", dto.TruckId ?? "<null>", dto.Driver ?? "<null>");

        var updateAttributes = BuildAttributes(dto);
        _logger.LogInformation("GrowerDelivery Update BuildAttributes returned {Count} entries: {Codes}",
            updateAttributes.Count, string.Join(",", updateAttributes.Select(a => a.Code)));
        {
            // Load the full active type map so we can also delete rows that were
            // cleared in the UI (attribute no longer has a value).
            var knownCodes = new[] { "MOISTURE", "PROTEIN", "OIL", "STARCH", "TEST_WEIGHT",
                "DOCKAGE", "FOREIGN_MATTER", "SPLITS", "DAMAGED", "GRADE",
                "BOL", "TRUCK_ID", "DRIVER" };

            var typeMapUpd = await _ctx.TransactionAttributeTypes
                .AsNoTracking()
                .Where(t => t.IsActive == true && knownCodes.Contains(t.Code))
                .ToDictionaryAsync(t => t.Code, t => t.Id, ct);

            // Delete every known-code row for this transaction up front so that
            // cleared fields don't linger and swapped IDs can't cause double rows.
            // TransactionAttributes is now keyed by TransactionAttributesUid,
            // so EF Core can track + remove rows cleanly.
            var knownIds = typeMapUpd.Values.ToHashSet();
            var existingRows = await _ctx.TransactionAttributes
                .Where(a => a.TransactionId == transactionId && knownIds.Contains(a.AttributeTypeId))
                .ToListAsync(ct);
            _ctx.TransactionAttributes.RemoveRange(existingRows);

            // Re-insert attributes that have values.
            var attrNow = DateTime.UtcNow;
            foreach (var attr in updateAttributes)
            {
                if (!typeMapUpd.TryGetValue(attr.Code, out var typeId))
                {
                    _logger.LogWarning("TransactionAttributeType not found for code {Code}", attr.Code);
                    continue;
                }

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

            await _ctx.SaveChangesAsync(ct);
        }

        // IS_END_DUMP follows the same delete-then-insert pattern but
        // uses BoolValue, so it lives outside the BuildAttributes loop.
        await WriteIsEndDumpAsync(transactionId, dto.IsEndDump, ct);

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

        // Recompute the parent WS status — editing an existing load may now
        // satisfy the outbound/protein/bin rules and move the sheet from
        // PendingNotFinished to PendingFinished (or vice versa if cleared).
        if (detail.RefType == "WeightSheet" && detail.RefId.HasValue)
        {
            await RecomputeWeightSheetStatusAsync(detail.RefId.Value, ct);
        }

        // Push WS-update notification so other open browsers see the change.
        if (detail.RefType == "WeightSheet" && detail.RefId.HasValue)
        {
            var editedWsId = await _ctx.WeightSheets
                .AsNoTracking()
                .Where(w => w.RowUid == detail.RefId)
                .Select(w => (long?)w.WeightSheetId)
                .FirstOrDefaultAsync(ct);
            if (editedWsId.HasValue)
                await _notifier.NotifyAsync(dto.LocationId, editedWsId.Value, "load-updated", ct);
        }

        // ── Completion status ───────────────────────────────────────────────────
        bool hasQty       = isTruck ? dto.EndQty.HasValue : hasDirect;
        bool hasContainer = dto.ToContainers?.Count > 0;
        bool isComplete   = hasQty && hasContainer;

        // Camera capture — fire whenever a weight was just captured OR
        // re-captured (value changed). A correction is intentionally a fresh
        // photo: the previous image on disk is overwritten by TicketImage
        // upload, so the saved picture always matches the saved weight.
        // Resaves that don't touch the weight do nothing.
        var loadKey = transactionId.ToString();
        if (isTruck && dto.StartQty.HasValue && dto.StartQty != oldWeights.StartQty)
            await _cameraTrigger.FireAsync(loadKey, "in",  dto.LocationId, scaleId: null, ct);
        if (isTruck && dto.EndQty.HasValue && dto.EndQty != oldWeights.EndQty)
            await _cameraTrigger.FireAsync(loadKey, "out", dto.LocationId, scaleId: null, ct);

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

    // GET /api/GrowerDelivery/ValidatePin?pin=&requiredPrivilegeId=
    //
    // When requiredPrivilegeId is supplied, the response is 403 Forbidden if
    // the user behind the PIN does not hold that privilege. Callers gate
    // sensitive actions (edit lot = priv 10, modify received WS = priv 12,
    // modify transfer WS = priv 13) by passing the appropriate id up front.
    [HttpGet("ValidatePin")]
    public async Task<IActionResult> ValidatePin(
        [FromQuery] int pin,
        [FromQuery] int? requiredPrivilegeId,
        CancellationToken ct)
    {
        if (pin <= 0)
            return BadRequest(new { message = "PIN is required." });

        var user = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == pin && u.IsActive, ct);

        if (user is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        var privileges = await _ctx.UserPrivileges
            .AsNoTracking()
            .Where(p => p.UserId == user.UserId)
            .Select(p => p.PrivilegeId)
            .ToListAsync(ct);

        if (requiredPrivilegeId.HasValue
            && !GrainManagement.Constants.Privileges.HasPrivilege(privileges, requiredPrivilegeId.Value))
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                message = "User does not have permission to perform this action.",
                userName = user.UserName,
                requiredPrivilegeId = requiredPrivilegeId.Value,
            });
        }

        return Ok(new { UserId = user.UserId, UserName = user.UserName, Privileges = privileges });
    }

    // GET /api/GrowerDelivery/EndOfDayCheck?locationId=
    //
    // Audits every still-open weight sheet at the location for End-Of-Day
    // readiness. A weight sheet is reported back when at least one of its
    // non-voided loads is "incomplete" (missing outbound weight or bin) or is
    // missing a protein reading. The status string is composed for direct
    // use as the grid column on the dashboard's End-Of-Day modal:
    //
    //   "Not Complete"                    — incomplete loads only
    //   "No Protein Set"                  — missing-protein loads only
    //   "No Protein Set / Not Complete"   — both
    //
    // WSs where every load is complete AND has protein are omitted — they
    // need no operator attention.
    [HttpGet("EndOfDayCheck")]
    public async Task<IActionResult> EndOfDayCheck(
        [FromQuery] int locationId,
        CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        // Resolve the protein attribute type id once. If the code is missing
        // the audit can't run — fail loudly so a misconfigured DB is obvious.
        var proteinAttrId = await _ctx.TransactionAttributeTypes
            .AsNoTracking()
            .Where(t => t.Code == "PROTEIN")
            .Select(t => (int?)t.Id)
            .FirstOrDefaultAsync(ct);
        if (proteinAttrId is null)
            return Problem("PROTEIN transaction attribute type is not configured.");

        // Pull every open WS at this location/server, with per-WS counts of
        // incomplete vs missing-protein loads. Subqueries are inlined into
        // the projection so EF doesn't see a bare IQueryable in the result
        // shape (which would fail translation).
        var proteinId = proteinAttrId.Value;
        var rows = await (
            from ws in _ctx.WeightSheets.AsNoTracking()
            where ws.LocationId == locationId
                  && ws.ServerId == _systemInfo.ServerId
                  && ws.StatusId < 3 // not closed
            select new
            {
                ws.WeightSheetId,
                ws.As400Id,
                ws.RowUid,
                ws.WeightSheetType,
                ws.LotId,
                LotDescription = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.LotDescription).FirstOrDefault()
                    : null,
                LotAs400Id = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => (long?)l.As400Id).FirstOrDefault()
                    : null,
                TotalLoads = _ctx.InventoryTransactionDetails.Count(itd =>
                    itd.RefId == ws.RowUid
                    && itd.RefType == "WeightSheet"
                    && !itd.IsVoided),
                IncompleteLoadCount = _ctx.InventoryTransactionDetails.Count(itd =>
                    itd.RefId == ws.RowUid
                    && itd.RefType == "WeightSheet"
                    && !itd.IsVoided
                    // Direct-quantity loads are "complete" once DirectQty is
                    // set. Truck loads need both EndQty and a container.
                    && itd.DirectQty == null
                    && (itd.EndQty == null
                        || !_ctx.InventoryTransactionDetailToContainers.Any(tc => tc.TransactionId == itd.TransactionId))),
                MissingProteinCount = _ctx.InventoryTransactionDetails.Count(itd =>
                    itd.RefId == ws.RowUid
                    && itd.RefType == "WeightSheet"
                    && !itd.IsVoided
                    && !_ctx.TransactionAttributes.Any(a =>
                        a.TransactionId == itd.TransactionId
                        && a.AttributeTypeId == proteinId
                        && a.DecimalValue != null
                        && a.DecimalValue > 0)),
            })
            .ToListAsync(ct);

        // Project to the audit DTO and drop already-clean WSs.
        var issues = rows
            .Select(r =>
            {
                var status = (r.IncompleteLoadCount > 0, r.MissingProteinCount > 0) switch
                {
                    (true,  true)  => "No Protein Set / Not Complete",
                    (true,  false) => "Not Complete",
                    (false, true)  => "No Protein Set",
                    _              => "Complete",
                };
                return new
                {
                    r.WeightSheetId,
                    r.As400Id,
                    WeightSheetIdDisplay = r.As400Id > 0 ? r.As400Id.ToString() : r.WeightSheetId.ToString(),
                    r.WeightSheetType,
                    r.LotId,
                    LotIdDisplay = r.LotAs400Id.HasValue && r.LotAs400Id.Value > 0
                        ? r.LotAs400Id.Value.ToString()
                        : (r.LotId.HasValue ? r.LotId.Value.ToString() : ""),
                    r.LotDescription,
                    r.TotalLoads,
                    r.IncompleteLoadCount,
                    r.MissingProteinCount,
                    Status = status,
                };
            })
            .Where(r => r.Status != "Complete")
            .OrderBy(r => r.WeightSheetType)
            .ThenBy(r => r.WeightSheetIdDisplay)
            .ToList();

        return Ok(issues);
    }

    // GET /api/GrowerDelivery/OpenWeightSheets?locationId=&statusBucket=&fromDate=&toDate=
    // statusBucket: "open" (default), "pending", "closed", or "all"
    //   open    = StatusId IN (0, 1)   — only untouched sheets on this bucket,
    //                                     the date range is ignored
    //   pending = StatusId = 2         — PendingFinished
    //   closed  = StatusId = 3         — end-of-day closed
    //   all     = any status
    // fromDate/toDate are inclusive bounds on CreationDate (applied to all
    // buckets except "open", matching the Warehouse page UX where the date
    // picker is only visible on non-Open selections).
    [HttpGet("OpenWeightSheets")]
    public async Task<IActionResult> GetOpenWeightSheets(
        [FromQuery] int locationId,
        [FromQuery] string? statusBucket,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        var bucket = (statusBucket ?? "open").Trim().ToLowerInvariant();
        if (bucket != "open" && bucket != "pending" && bucket != "closed" && bucket != "all")
            bucket = "open";

        var query = _ctx.WeightSheets
            .AsNoTracking()
            .Where(ws => ws.LocationId == locationId && ws.ServerId == _systemInfo.ServerId);

        // Bucket filter. The "open" bucket covers StatusId 0 and 1 per the
        // business rules: Open and PendingNotFinished are both shown in the
        // warehouse worklist until they move to PendingFinished or Closed.
        if (bucket == "open")
            query = query.Where(ws => ws.StatusId == 0 || ws.StatusId == 1);
        else if (bucket == "pending")
            query = query.Where(ws => ws.StatusId == 2);
        else if (bucket == "closed")
            query = query.Where(ws => ws.StatusId == 3);
        // "all" — no status filter

        // Date range — only applied on non-Open buckets.
        if (bucket != "open")
        {
            if (fromDate.HasValue)
            {
                var from = DateOnly.FromDateTime(fromDate.Value.Date);
                query = query.Where(ws => ws.CreationDate >= from);
            }
            if (toDate.HasValue)
            {
                var to = DateOnly.FromDateTime(toDate.Value.Date);
                query = query.Where(ws => ws.CreationDate <= to);
            }
        }

        var sheets = await query
            .OrderByDescending(ws => ws.CreatedAt)
            .Select(ws => new
            {
                ws.WeightSheetId,
                ws.As400Id,
                ws.RowUid,
                ws.WeightSheetType,
                CreationDate   = ws.CreationDate.ToString("MM/dd/yyyy"),
                ws.StatusId,
                IsOpen         = ws.StatusId < 3,
                LotDescription = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.LotDescription).FirstOrDefault()
                    : null,
                ws.LotId,
                SplitGroupDescription = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId)
                        .Select(l => l.SplitGroup != null ? l.SplitGroup.SplitGroupDescription : null)
                        .FirstOrDefault()
                    : null,
                // Primary account comes from LotSplitGroups.PrimaryAccount = 1 rather
                // than SplitGroup.PrimaryAccountId so override-mode groups still show
                // the chosen producer (the split-group-level value is null for those).
                AccountName = ws.LotId != null
                    ? _ctx.LotSplitGroups
                          .Where(lsg => lsg.LotId == ws.LotId && lsg.PrimaryAccount)
                          .Join(_ctx.Accounts,
                                lsg => lsg.AccountId,
                                a => a.AccountId,
                                (lsg, a) => a.LookupName)
                          .FirstOrDefault()
                    : null,
                // Item lives on the lot for Delivery WSs and on the WS itself
                // for Transfer WSs — fall back to ws.ItemId so transfer rows
                // still surface the variety in the grid.
                ItemDescription = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId && l.ItemId != null)
                        .Select(l => _ctx.Items
                            .Where(i => i.ItemId == l.ItemId)
                            .Select(i => i.Description).FirstOrDefault())
                        .FirstOrDefault()
                    : (ws.ItemId != null
                        ? _ctx.Items.Where(i => i.ItemId == ws.ItemId).Select(i => i.Description).FirstOrDefault()
                        : null),
                ws.RateType,
                ws.CustomRateDescription,
                // RateType='I' = In House (operator's own equipment, no hauler).
                // Display "In House" so the column isn't blank.
                HaulerName = ws.RateType == "I"
                    ? "In House"
                    : (ws.HaulerId != null
                        ? _ctx.Haulers.Where(h => h.Id == ws.HaulerId).Select(h => h.Description).FirstOrDefault()
                        : null),
                LoadCount = _ctx.InventoryTransactionDetails.Count(itd => itd.RefId == ws.RowUid && itd.RefType == "WeightSheet"),
                // "In yard" — non-voided truck loads that have arrived (StartedAt
                // recorded) but haven't completed (no CompletedAt yet).
                LoadsInYard = _ctx.InventoryTransactionDetails.Count(itd =>
                    itd.RefId == ws.RowUid && itd.RefType == "WeightSheet"
                    && !itd.IsVoided
                    && itd.StartedAt != null && itd.CompletedAt == null),
                WsNotes  = ws.Notes,
                LotAs400Id = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => (long?)l.As400Id).FirstOrDefault()
                    : null,
                // Source / destination names for transfer WSs. Null on deliveries.
                SourceLocationName      = ws.SourceLocationId != null
                    ? _ctx.Locations.Where(l => l.LocationId == ws.SourceLocationId).Select(l => l.Name).FirstOrDefault()
                    : null,
                DestinationLocationName = ws.DestinationLocationId != null
                    ? _ctx.Locations.Where(l => l.LocationId == ws.DestinationLocationId).Select(l => l.Name).FirstOrDefault()
                    : null,
                ws.SourceLocationId,
                ws.DestinationLocationId,
                ws.LocationId,
                // For Delivery WSs, LotType comes from the lot. For Transfer WSs
                // there's no lot — derive from the Item's SEED (31) / WAREHOUSE (32)
                // trait so the row tinting and labels still pick the right flavor.
                LotType = ws.LotId != null
                    ? (int?)_ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => (int)l.LotType).FirstOrDefault()
                    : (ws.ItemId != null
                        ? (_ctx.ItemTraits.Any(it => it.ItemId == ws.ItemId.Value && it.TraitId == 31) ? (int?)0
                            : (_ctx.ItemTraits.Any(it => it.ItemId == ws.ItemId.Value && it.TraitId == 32) ? (int?)1 : null))
                        : null),
            })
            .ToListAsync(ct);

        return Ok(sheets);
    }

    // GET /api/GrowerDelivery/OpenLots?locationId=
    // Returns open lots at this location for the lot-selection pickers. Lots.IsOpen
    // is the only selection criterion — we intentionally do NOT re-filter by the
    // WEIGHTSHEET system trait here, both because the picker only needs open lots
    // and because skipping the LotTraits join keeps this query fast.
    [HttpGet("OpenLots")]
    public async Task<IActionResult> GetOpenLots([FromQuery] int locationId, CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        var lots = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LocationId == locationId && l.IsOpen &&
                        l.ServerId == _systemInfo.ServerId)
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
                // Primary account comes from LotSplitGroups.PrimaryAccount = 1 rather
                // than SplitGroup.PrimaryAccountId, so override-mode groups (where
                // SplitGroup.PrimaryAccountId is null) still show the chosen producer.
                AccountName           = _ctx.LotSplitGroups
                                            .Where(lsg => lsg.LotId == l.LotId && lsg.PrimaryAccount)
                                            .Join(_ctx.Accounts,
                                                  lsg => lsg.AccountId,
                                                  a => a.AccountId,
                                                  (lsg, a) => a.LookupName)
                                            .FirstOrDefault(),
                l.ItemId,
                ItemDescription       = l.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == l.ItemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                CropName              = l.Product != null ? l.Product.Description : null,
                LotType    = (int)l.LotType,
                State      = l.LotTraits.Where(t => t.TraitTypeId == 15).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                County     = l.LotTraits.Where(t => t.TraitTypeId == 16).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                Landlord   = l.LotTraits.Where(t => t.TraitTypeId == 18).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                FarmNumber = l.LotTraits.Where(t => t.TraitTypeId == 19).Select(t => t.Trait.TraitCode).FirstOrDefault(),
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

    // NOTE: The split-group reference endpoints (all groups + per-group percents)
    // live in SplitGroupsApiController under /api/SplitGroups so the /Admin view
    // can call them without the GrowerDelivery module gate on this controller.

    // GET /api/GrowerDelivery/WeightSheetLots?locationId=&status=&fromDate=&toDate=
    // Returns weight sheet lots at the location — management view.
    //
    // status   : "open" (default), "closed", or "all"
    // fromDate : inclusive lower bound on Lot.CreatedAt. Optional.
    // toDate   : inclusive upper bound on Lot.CreatedAt. Optional.
    //
    // When status == "open" no date window is applied by default — open lots are
    // bounded in count and we want them all regardless of age.
    // When status is "closed" or "all" and fromDate is not supplied, we default
    // to the last 90 days so the page stays responsive on locations with years
    // of history. Users can push fromDate back to see older closed lots.
    [HttpGet("WeightSheetLots")]
    public async Task<IActionResult> GetWeightSheetLots(
        [FromQuery] int locationId,
        [FromQuery] string? status,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        CancellationToken ct)
    {
        if (locationId <= 0)
            return BadRequest(new { message = "locationId is required." });

        var normalizedStatus = (status ?? "open").Trim().ToLowerInvariant();
        if (normalizedStatus != "open" && normalizedStatus != "closed" && normalizedStatus != "all")
            normalizedStatus = "open";

        // Date window rules:
        //  - open only: no default window (caller can still pass dates if they want).
        //  - closed/all with no fromDate: default to last 90 days.
        //  - any explicit fromDate/toDate is always honored.
        DateTime? fromUtc = fromDate?.Date;
        if (fromUtc is null && normalizedStatus != "open")
            fromUtc = DateTime.UtcNow.Date.AddDays(-90);

        // toDate is inclusive, so use the start of the next day as an exclusive upper bound.
        DateTime? toExclusiveUtc = toDate.HasValue ? toDate.Value.Date.AddDays(1) : (DateTime?)null;

        var query = _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LocationId == locationId &&
                        l.ServerId == _systemInfo.ServerId &&
                        l.LotTraits.Any(t => t.TraitTypeId == 12));

        if (normalizedStatus == "open")
            query = query.Where(l => l.IsOpen);
        else if (normalizedStatus == "closed")
            query = query.Where(l => !l.IsOpen);

        if (fromUtc.HasValue)
            query = query.Where(l => l.CreatedAt >= fromUtc.Value);
        if (toExclusiveUtc.HasValue)
            query = query.Where(l => l.CreatedAt < toExclusiveUtc.Value);

        var lots = await query
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
                // Primary account comes from the lot's own LotSplitGroups row flagged
                // PrimaryAccount = 1 (guaranteed to exist by CK_LotSplitGroups_RequiresPrimary).
                // The SplitGroup.PrimaryAccountId is null for override-mode groups, so
                // projecting from there would drop the primary account on those lots.
                AccountId             = _ctx.LotSplitGroups
                                            .Where(lsg => lsg.LotId == l.LotId && lsg.PrimaryAccount)
                                            .Select(lsg => (long?)lsg.AccountId)
                                            .FirstOrDefault(),
                AccountAs400Id        = _ctx.LotSplitGroups
                                            .Where(lsg => lsg.LotId == l.LotId && lsg.PrimaryAccount)
                                            .Join(_ctx.Accounts,
                                                  lsg => lsg.AccountId,
                                                  a => a.AccountId,
                                                  (lsg, a) => (long?)a.As400AccountId)
                                            .FirstOrDefault(),
                AccountName           = _ctx.LotSplitGroups
                                            .Where(lsg => lsg.LotId == l.LotId && lsg.PrimaryAccount)
                                            .Join(_ctx.Accounts,
                                                  lsg => lsg.AccountId,
                                                  a => a.AccountId,
                                                  (lsg, a) => a.LookupName)
                                            .FirstOrDefault(),
                l.ItemId,
                ItemDescription       = l.ItemId != null
                    ? _ctx.Items.Where(i => i.ItemId == l.ItemId).Select(i => i.Description).FirstOrDefault()
                    : null,
                l.Notes,
                HasClosedWeightSheet  = _ctx.WeightSheets.Any(ws => ws.LotId == l.LotId && ws.ClosedAt != null),
                LotType    = (int)l.LotType,
                State      = l.LotTraits.Where(t => t.TraitTypeId == 15).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                County     = l.LotTraits.Where(t => t.TraitTypeId == 16).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                Landlord   = l.LotTraits.Where(t => t.TraitTypeId == 18).Select(t => t.Trait.TraitCode).FirstOrDefault(),
                FarmNumber = l.LotTraits.Where(t => t.TraitTypeId == 19).Select(t => t.Trait.TraitCode).FirstOrDefault(),
            })
            .ToListAsync(ct);

        return Ok(lots);
    }

    // POST /api/GrowerDelivery/WeightSheetLots/{id}/close
    //   Closes a lot (IsOpen = false). The lot can only be closed when EVERY
    //   non-voided load on it (across every WS that uses it) is complete —
    //   has a recorded final weight and a bin/container. Loads that have
    //   weight + bin but no protein are surfaced via a separate
    //   `requiresProteinAck` response so the client can confirm and retry.
    //
    //   Body: { acceptMissingProtein: bool } (optional)
    [HttpPost("WeightSheetLots/{id:long}/close")]
    public async Task<IActionResult> CloseLot(
        long id,
        [FromBody] SetPendingDto dto = null,
        CancellationToken ct = default)
    {
        dto ??= new SetPendingDto();

        var lot = await _ctx.Lots.FindAsync(new object[] { id }, ct);
        if (lot is null) return NotFound(new { message = "Lot not found." });
        if (!lot.IsOpen) return Ok(new { lot.LotId, lot.IsOpen });

        var lotLoads = await (
            from d in _ctx.InventoryTransactionDetails.AsNoTracking()
            where d.LotId == id && d.RefType == "WeightSheet" && !d.IsVoided
            join w in _ctx.WeightSheets.AsNoTracking() on d.RefId equals w.RowUid
            select new
            {
                d.TransactionId,
                d.StartQty,
                d.EndQty,
                d.DirectQty,
                w.WeightSheetId,
                w.As400Id,
                HasContainer = _ctx.InventoryTransactionDetailToContainers
                    .Any(c => c.TransactionId == d.TransactionId),
                ProteinValue = _ctx.TransactionAttributes
                    .Where(a => a.TransactionId == d.TransactionId
                                && a.AttributeType.Code == "PROTEIN")
                    .Select(a => (decimal?)a.DecimalValue)
                    .FirstOrDefault(),
            }
        ).ToListAsync(ct);

        var incomplete = lotLoads
            .Select(l =>
            {
                bool hasFinalWeight = l.DirectQty.HasValue
                    || (l.StartQty.HasValue && l.EndQty.HasValue);
                string reason = !hasFinalWeight
                    ? "missing outbound weight"
                    : (!l.HasContainer ? "missing bin" : null);
                return (l, reason);
            })
            .Where(x => x.reason != null)
            .Select(x => new
            {
                x.l.WeightSheetId,
                x.l.As400Id,
                x.l.TransactionId,
                Reason = x.reason,
            })
            .ToList();

        if (incomplete.Count > 0)
        {
            return BadRequest(new
            {
                message = "Lot cannot be closed — some loads are not complete.",
                incompleteLoads = incomplete,
            });
        }

        var missingProtein = lotLoads
            .Where(l => !(l.ProteinValue.HasValue && l.ProteinValue.Value > 0))
            .Select(l => new { l.WeightSheetId, l.As400Id, l.TransactionId })
            .ToList();

        if (missingProtein.Count > 0 && !dto.AcceptMissingProtein)
        {
            return Ok(new
            {
                requiresProteinAck = true,
                missingProteinLoads = missingProtein,
            });
        }

        lot.IsOpen    = false;
        lot.UpdatedAt = DateTime.UtcNow;

        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to close lot {LotId}", id);
            return StatusCode(500, new { message = "Database error while closing lot." });
        }

        // Lot close affects every WS that uses the lot — push an update to
        // each so any open browser refreshes.
        var lotWsIds = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(w => w.LotId == id)
            .Select(w => new { w.LocationId, w.WeightSheetId })
            .ToListAsync(ct);
        foreach (var w in lotWsIds)
            await _notifier.NotifyAsync(w.LocationId, w.WeightSheetId, "lot-closed", ct);

        return Ok(new { lot.LotId, lot.IsOpen });
    }

    // POST /api/GrowerDelivery/WeightSheetLots/{id}/open  — re-opens a closed lot
    [HttpPost("WeightSheetLots/{id:long}/open")]
    public async Task<IActionResult> OpenLot(long id, CancellationToken ct)
    {
        var lot = await _ctx.Lots.FindAsync(new object[] { id }, ct);
        if (lot is null) return NotFound(new { message = "Lot not found." });

        if (lot.IsOpen)
            return Ok(new { lot.LotId, lot.IsOpen });

        // Same-day-only guard: a lot may only be re-opened on the same local day it was closed.
        // UpdatedAt is set to UtcNow at close time (see CloseLot above).
        var closedLocalDate = lot.UpdatedAt?.ToLocalTime().Date;
        if (closedLocalDate != DateTime.Now.Date)
        {
            return BadRequest(new
            {
                message = "This lot was closed on a previous day and cannot be re-opened. Please contact the office.",
                cannotReopen = true,
            });
        }

        lot.IsOpen    = true;
        lot.UpdatedAt = DateTime.UtcNow;

        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to re-open lot {LotId}", id);
            return StatusCode(500, new { message = "Database error while re-opening lot." });
        }

        // Push lot-reopened to every WS for this lot.
        var openedLotWsIds = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(w => w.LotId == id)
            .Select(w => new { w.LocationId, w.WeightSheetId })
            .ToListAsync(ct);
        foreach (var w in openedLotWsIds)
            await _notifier.NotifyAsync(w.LocationId, w.WeightSheetId, "lot-reopened", ct);

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

        // ── PIN + priv 9 (Add Lots) validation ──────────────────────────────
        // Client gates the New Lot button on a priv-9 PIN prompt up front;
        // this is the server-side defense in case the gate is bypassed.
        if (!dto.Pin.HasValue || dto.Pin.Value <= 0)
            return BadRequest(new { message = "PIN is required to create a lot." });

        var pinUser = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin.Value && u.IsActive, ct);

        if (pinUser is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        var pinPrivIds = await _ctx.UserPrivileges
            .AsNoTracking()
            .Where(p => p.UserId == pinUser.UserId)
            .Select(p => p.PrivilegeId)
            .ToListAsync(ct);
        if (!GrainManagement.Constants.Privileges.HasPrivilege(
                pinPrivIds, GrainManagement.Constants.Privileges.AddLots))
            return StatusCode(StatusCodes.Status403Forbidden,
                new { message = "User does not have permission to create lots." });

        // Load split group + percents in a single query
        var sg = await _ctx.SplitGroups
            .AsNoTracking()
            .Include(s => s.SplitGroupPercents)
            .FirstOrDefaultAsync(s => s.SplitGroupId == dto.SplitGroupId, ct);

        if (sg is null)
            return NotFound(new { message = "Split group not found." });

        // Override-account path: when the split group has no PrimaryAccountId, the caller
        // must supply an active producer account to act as the lot's primary account.
        long? overrideAccountId = null;
        if (sg.PrimaryAccountId is null)
        {
            if (!dto.OverrideAccountId.HasValue || dto.OverrideAccountId.Value <= 0)
                return BadRequest(new { message = "This split group has no primary account. An override account is required." });

            var overrideAcct = await _ctx.Accounts
                .AsNoTracking()
                .Where(a => a.AccountId == dto.OverrideAccountId.Value && a.IsProducer && a.IsActive)
                .Select(a => new { a.AccountId })
                .FirstOrDefaultAsync(ct);

            if (overrideAcct is null)
                return BadRequest(new { message = "Override account must be an active producer account." });

            overrideAccountId = overrideAcct.AccountId;
        }

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
            int lotType = dto.LotType ?? 0; // default Seed

            // INSTEAD OF INSERT trigger generates LotId — pass RowUid so we can find the row back
            var rowUid = Guid.NewGuid();
            string createdBy = pinUser.UserName;
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [Inventory].[Lots] (LocationId, SplitGroupId, LotDescription, ItemId, ProductId, Notes, IsOpen, CreatedAt, RowUid, CreatedByUserName, LotType)
                VALUES ({dto.LocationId}, {dto.SplitGroupId}, {lotDesc}, {itemId}, {productId}, {notes}, 1, {now}, {rowUid}, {createdBy}, {lotType})",
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

            // Insert FarmNumber trait (TraitTypeId=19) if provided
            if (!string.IsNullOrWhiteSpace(dto.FarmNumber))
            {
                var farmTrait = await _ctx.Traits
                    .FirstOrDefaultAsync(t => t.TraitTypeId == 19 && t.TraitCode == dto.FarmNumber.Trim(), ct);
                if (farmTrait == null)
                {
                    farmTrait = new Trait { TraitCode = dto.FarmNumber.Trim(), TraitTypeId = 19, Description = dto.FarmNumber.Trim(), IsActive = true, CreatedAt = now };
                    _ctx.Traits.Add(farmTrait);
                    await _ctx.SaveChangesAsync(ct);
                }
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    INSERT INTO [Inventory].[LotTraits] (LotId, TraitId, TraitTypeId, IsExclusive, CreatedAt)
                    VALUES ({lotId}, {farmTrait.TraitId}, {19}, 1, {now})", ct);
            }

            // The CK_LotSplitGroups_RequiresPrimary check constraint demands that
            // every LotId in LotSplitGroups has at least one row with PrimaryAccount=1,
            // which means the *first* row we insert for a lot must be the primary.
            //
            // Determine the effective primary account:
            //   - Normal path : sg.PrimaryAccountId (the 'G' row from AS400)
            //   - Override path: the caller-supplied override account id
            // Then insert the primary row first, then the rest of the percents,
            // skipping any percent row that collides with the primary (the override
            // account may already be one of the landlord percents).
            long? effectivePrimaryAccountId = sg.PrimaryAccountId ?? overrideAccountId;

            var primaryPercent = effectivePrimaryAccountId.HasValue
                ? sg.SplitGroupPercents.FirstOrDefault(p => p.AccountId == effectivePrimaryAccountId.Value)
                : null;

            // 1) Primary row first.
            {
                var primaryRowGuid = Guid.NewGuid();
                var primaryAccountIdForInsert = effectivePrimaryAccountId!.Value;
                var primarySplitPercent = primaryPercent?.SplitPercent ?? 0m;
                bool primaryFlag = true;
                await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                    INSERT INTO [Inventory].[LotSplitGroups] (RowGuid, LotId, AccountId, SplitPercent, PrimaryAccount)
                    VALUES ({primaryRowGuid}, {lotId}, {primaryAccountIdForInsert}, {primarySplitPercent}, {primaryFlag})",
                    ct);
            }

            // 2) Remaining (non-primary) percent rows.
            foreach (var p in sg.SplitGroupPercents)
            {
                if (effectivePrimaryAccountId.HasValue && p.AccountId == effectivePrimaryAccountId.Value)
                    continue; // already inserted as the primary row above

                var rowGuid = Guid.NewGuid();
                bool isPrimary = false;
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
            // Surface the innermost exception message so the client (and the user
            // reporting the issue) can see exactly what SQL Server rejected. This
            // matches how update errors were already handled elsewhere and avoids
            // the "Database error" opaque message masking the real cause.
            var rootMsg = ex.GetBaseException().Message;
            return StatusCode(500, new
            {
                message = "Database error while creating weight sheet lot: " + rootMsg,
                detail  = rootMsg,
            });
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
        // ── PIN + priv 10 (Modify Lots) validation ──────────────────────────
        // Client gates the Edit Lot Properties link on a priv-10 PIN prompt up
        // front; this is the server-side enforcement in case the gate is
        // bypassed by direct API call.
        if (!dto.Pin.HasValue || dto.Pin.Value <= 0)
            return BadRequest(new { message = "PIN is required." });

        var pinUser = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin.Value && u.IsActive, ct);

        if (pinUser is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        var pinPrivIds = await _ctx.UserPrivileges
            .AsNoTracking()
            .Where(p => p.UserId == pinUser.UserId)
            .Select(p => p.PrivilegeId)
            .ToListAsync(ct);
        if (!GrainManagement.Constants.Privileges.HasPrivilege(
                pinPrivIds, GrainManagement.Constants.Privileges.ModifyLot))
            return StatusCode(StatusCodes.Status403Forbidden,
                new { message = "User does not have permission to edit lots." });

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
            Landlord   = await _ctx.LotTraits.Where(t => t.LotId == id && t.TraitTypeId == 18).Select(t => t.Trait.TraitCode).FirstOrDefaultAsync(ct),
            FarmNumber = await _ctx.LotTraits.Where(t => t.LotId == id && t.TraitTypeId == 19).Select(t => t.Trait.TraitCode).FirstOrDefaultAsync(ct),
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

            // Upsert FarmNumber trait (TraitTypeId=19)
            await UpsertLotTrait(id, 19, dto.FarmNumber?.Trim(), now, ct);
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
            FarmNumber     = notesOnly ? oldValues.FarmNumber : dto.FarmNumber?.Trim(),
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

        // ── Prior-day guard: refuse new WSs while yesterday's are open ────
        var staleResult = await RejectIfPriorDayOpenAsync(dto.LocationId, ct);
        if (staleResult != null) return staleResult;

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

            await _notifier.NotifyAsync(dto.LocationId, wsId, "ws-created", ct);

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
                bolAttr.StringValue     AS BOL,
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
            LEFT JOIN [Inventory].[TransactionAttributeTypes] at1
                ON at1.Code = 'PROTEIN'
            LEFT JOIN [Inventory].[TransactionAttributes] attr1
                ON attr1.TransactionId = itd.TransactionId AND attr1.AttributeTypeId = at1.Id
            LEFT JOIN [Inventory].[TransactionAttributeTypes] at2
                ON at2.Code = 'MOISTURE'
            LEFT JOIN [Inventory].[TransactionAttributes] attr2
                ON attr2.TransactionId = itd.TransactionId AND attr2.AttributeTypeId = at2.Id
            LEFT JOIN [Inventory].[TransactionAttributes] bolAttr
                ON bolAttr.TransactionId = itd.TransactionId
                AND bolAttr.AttributeTypeId = (SELECT TOP 1 Id FROM [Inventory].[TransactionAttributeTypes] WHERE Code = 'BOL')
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
              -- Listing without a specific wsId still defaults to deliveries
              -- only; targeting a specific wsId returns whatever type it is.
              AND (@wsId > 0 OR ws.WeightSheetType = 'Delivery')
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
                BOL                    = reader.IsDBNull(reader.GetOrdinal("BOL")) ? null : reader.GetString(reader.GetOrdinal("BOL")),
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
        await NotifyWsForTxnAsync(transactionId, "load-bin-updated", ct);
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

        // Resolve AttributeTypeId from AttributeCode if provided (preferred over hardcoded Ids)
        int resolvedAttrTypeId = dto.AttributeTypeId;
        if (!string.IsNullOrWhiteSpace(dto.AttributeCode))
        {
            var code = dto.AttributeCode.Trim().ToUpperInvariant();
            var lookup = await _ctx.TransactionAttributeTypes
                .AsNoTracking()
                .Where(t => t.IsActive == true && t.Code == code)
                .Select(t => (int?)t.Id)
                .FirstOrDefaultAsync(ct);
            if (lookup == null)
                return BadRequest(new { message = $"Unknown attribute code '{dto.AttributeCode}'." });
            resolvedAttrTypeId = lookup.Value;
        }

        if (resolvedAttrTypeId <= 0)
            return BadRequest(new { message = "AttributeTypeId or AttributeCode is required." });
        if (dto.DecimalValue.HasValue && dto.DecimalValue.Value <= 0)
            return BadRequest(new { message = "Decimal value must be greater than zero." });

        var now = DateTime.UtcNow;
        var hasDecimal = dto.DecimalValue.HasValue;
        var hasString  = !string.IsNullOrWhiteSpace(dto.StringValue);
        var hasValue   = hasDecimal || hasString;
        var strVal     = hasString ? dto.StringValue.Trim() : (string)null;

        // Check if attribute already exists (EF tracked — keyed entity)
        var existing = await _ctx.TransactionAttributes
            .FirstOrDefaultAsync(a => a.TransactionId == dto.TransactionId && a.AttributeTypeId == resolvedAttrTypeId, ct);

        if (existing != null)
        {
            if (hasValue)
            {
                existing.DecimalValue = dto.DecimalValue;
                existing.StringValue  = strVal;
                existing.IntValue     = null;
                existing.BoolValue    = null;
                existing.UpdatedAt    = now;
            }
            else
            {
                // Remove if no value submitted
                _ctx.TransactionAttributes.Remove(existing);
            }
        }
        else if (hasValue)
        {
            _ctx.TransactionAttributes.Add(new TransactionAttribute
            {
                TransactionAttributesUid = Guid.NewGuid(),
                TransactionId            = dto.TransactionId,
                AttributeTypeId          = resolvedAttrTypeId,
                DecimalValue             = dto.DecimalValue,
                StringValue              = strVal,
                IntValue                 = null,
                BoolValue                = null,
                CreatedAt                = now,
            });
        }

        await _ctx.SaveChangesAsync(ct);
        await NotifyWsForTxnAsync(dto.TransactionId, "load-attribute-updated", ct);
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
        await NotifyWsForTxnAsync(dto.TransactionId, "load-notes-updated", ct);
        return Ok(new { success = true });
    }

    // Resolves the weight sheet (LocationId + WeightSheetId) that owns a load
    // and pushes a SignalR update so the dashboard + open loads pages refresh.
    // Used by inline-edit endpoints that don't already carry WS context.
    private async Task NotifyWsForTxnAsync(long transactionId, string changeKind, CancellationToken ct)
    {
        var ws = await (
            from d in _ctx.InventoryTransactionDetails.AsNoTracking()
            where d.TransactionId == transactionId && d.RefType == "WeightSheet" && d.RefId != null
            join w in _ctx.WeightSheets.AsNoTracking() on d.RefId equals w.RowUid
            select new { w.WeightSheetId, w.LocationId }
        ).FirstOrDefaultAsync(ct);

        if (ws != null)
            await _notifier.NotifyAsync(ws.LocationId, ws.WeightSheetId, changeKind, ct);
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

        // Transfer WSs have no lot — variety / source / destination ride on
        // the WS header. We surface all of those alongside the lot fields so
        // the same endpoint serves both flows; the client picks which to show.
        // LotType for transfers is derived from the item's SEED (TraitId=31) /
        // WAREHOUSE (TraitId=32) trait so the row tinting still picks the
        // right flavor.
        var sql = @"
            SELECT
                ws.WeightSheetId,
                ws.As400Id      AS WsAs400Id,
                ws.WeightSheetType,
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
                ws.StatusId,
                ws.ItemId       AS WsItemId,
                wsItem.Description AS WsItemDescription,
                ws.SourceLocationId,
                srcLoc.Name     AS SourceLocationName,
                ws.DestinationLocationId,
                dstLoc.Name     AS DestinationLocationName,
                lot.As400Id     AS LotAs400Id,
                lot.LotDescription,
                lot.ServerId    AS LotServerId,
                lot.LocationId  AS LotLocationId,
                lot.ItemId      AS LotItemId,
                CASE
                    WHEN lot.LotId IS NOT NULL THEN lot.LotType
                    WHEN ws.ItemId IS NOT NULL THEN
                        CASE
                            WHEN EXISTS (SELECT 1 FROM [product].[ItemTraits] it
                                         WHERE it.ItemId = ws.ItemId AND it.TraitId = 31) THEN 0
                            WHEN EXISTS (SELECT 1 FROM [product].[ItemTraits] it
                                         WHERE it.ItemId = ws.ItemId AND it.TraitId = 32) THEN 1
                            ELSE NULL
                        END
                    ELSE NULL
                END AS LotType,
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
            LEFT JOIN [product].[Items] wsItem ON wsItem.ItemId = ws.ItemId
            LEFT JOIN [system].[Locations] srcLoc ON srcLoc.LocationId = ws.SourceLocationId
            LEFT JOIN [system].[Locations] dstLoc ON dstLoc.LocationId = ws.DestinationLocationId
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
            WeightSheetType    = reader.IsDBNull(reader.GetOrdinal("WeightSheetType")) ? null : reader.GetString(reader.GetOrdinal("WeightSheetType")),
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
            StatusId           = reader.IsDBNull(reader.GetOrdinal("StatusId")) ? (byte)0 : reader.GetByte(reader.GetOrdinal("StatusId")),
            // Transfer-WS-only header fields (NULL for delivery WSs).
            WsItemId               = reader.IsDBNull(reader.GetOrdinal("WsItemId")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("WsItemId")),
            WsItemDescription      = reader.IsDBNull(reader.GetOrdinal("WsItemDescription")) ? null : reader.GetString(reader.GetOrdinal("WsItemDescription")),
            SourceLocationId       = reader.IsDBNull(reader.GetOrdinal("SourceLocationId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("SourceLocationId")),
            SourceLocationName     = reader.IsDBNull(reader.GetOrdinal("SourceLocationName")) ? null : reader.GetString(reader.GetOrdinal("SourceLocationName")),
            DestinationLocationId  = reader.IsDBNull(reader.GetOrdinal("DestinationLocationId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DestinationLocationId")),
            DestinationLocationName= reader.IsDBNull(reader.GetOrdinal("DestinationLocationName")) ? null : reader.GetString(reader.GetOrdinal("DestinationLocationName")),
            LotAs400Id         = reader.IsDBNull(reader.GetOrdinal("LotAs400Id")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("LotAs400Id")),
            LotDescription     = reader.IsDBNull(reader.GetOrdinal("LotDescription")) ? null : reader.GetString(reader.GetOrdinal("LotDescription")),
            LotServerId        = reader.IsDBNull(reader.GetOrdinal("LotServerId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LotServerId")),
            LotLocationId      = reader.IsDBNull(reader.GetOrdinal("LotLocationId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LotLocationId")),
            LotItemId          = reader.IsDBNull(reader.GetOrdinal("LotItemId")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("LotItemId")),
            LotType            = reader.IsDBNull(reader.GetOrdinal("LotType")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("LotType")),
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

        // ── PIN + priv 10 (Modify Lots) required when changing the lot ───────
        // The client gates the Reassign Lot button on a PIN-with-priv-10 prompt
        // up front; this server-side check is the second line of defense in
        // case the client gate is bypassed.
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

            var pinPrivIds = await _ctx.UserPrivileges
                .AsNoTracking()
                .Where(p => p.UserId == pinUser.UserId)
                .Select(p => p.PrivilegeId)
                .ToListAsync(ct);
            if (!GrainManagement.Constants.Privileges.HasPrivilege(
                    pinPrivIds, GrainManagement.Constants.Privileges.ModifyLot))
                return StatusCode(StatusCodes.Status403Forbidden,
                    new { message = "User does not have permission to change the lot on a weight sheet." });

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
                    // Primary account comes from LotSplitGroups.PrimaryAccount = 1 so
                    // override-mode groups (SplitGroup.PrimaryAccountId is null) still
                    // show the chosen producer.
                    PrimaryAccountName = _ctx.LotSplitGroups
                                             .Where(lsg => lsg.LotId == l.LotId && lsg.PrimaryAccount)
                                             .Join(_ctx.Accounts,
                                                   lsg => lsg.AccountId,
                                                   a => a.AccountId,
                                                   (lsg, a) => a.EntityName != null && a.EntityName != "" ? a.EntityName : a.LookupName)
                                             .FirstOrDefault(),
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

        // Auto-create the trait if it doesn't exist (for State/County/Landlord/FarmNumber)
        if (trait == null && (traitTypeId == 15 || traitTypeId == 16 || traitTypeId == 18 || traitTypeId == 19))
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

    /// <summary>
    /// GET /api/GrowerDelivery/{id}/IsEndDump — single-attribute lookup
    /// for the load edit form to prefill the End Dump checkbox.
    /// Returns { IsEndDump: bool? } where null means "no answer was
    /// recorded for this load yet."
    /// </summary>
    [HttpGet("{transactionId:long}/IsEndDump")]
    public async Task<IActionResult> GetIsEndDump(long transactionId, CancellationToken ct)
    {
        var value = await _ctx.TransactionAttributes.AsNoTracking()
            .Where(a => a.TransactionId == transactionId
                        && a.AttributeType.Code == TransactionAttributeCodes.IsEndDump)
            .Select(a => a.BoolValue)
            .FirstOrDefaultAsync(ct);
        return Ok(new { IsEndDump = value });
    }

    /// <summary>
    /// Writes (or clears) the IS_END_DUMP transaction attribute on the
    /// load. The attribute carries a bool, so it can't go through the
    /// decimal/string-only <see cref="BuildAttributes"/> path. Always
    /// deletes any existing row first so the edit flow flips cleanly
    /// when the operator changes the answer.
    /// </summary>
    private async Task WriteIsEndDumpAsync(long transactionId, bool? value, CancellationToken ct)
    {
        var typeId = await _ctx.TransactionAttributeTypes.AsNoTracking()
            .Where(t => t.IsActive == true && t.Code == TransactionAttributeCodes.IsEndDump)
            .Select(t => (int?)t.Id)
            .FirstOrDefaultAsync(ct);
        if (typeId == null) return; // type row not seeded yet — no-op.

        var existing = await _ctx.TransactionAttributes
            .Where(a => a.TransactionId == transactionId && a.AttributeTypeId == typeId.Value)
            .ToListAsync(ct);
        if (existing.Count > 0)
            _ctx.TransactionAttributes.RemoveRange(existing);

        if (value.HasValue)
        {
            _ctx.TransactionAttributes.Add(new TransactionAttribute
            {
                TransactionAttributesUid = Guid.NewGuid(),
                TransactionId = transactionId,
                AttributeTypeId = typeId.Value,
                BoolValue = value.Value,
                CreatedAt = DateTime.UtcNow,
            });
        }
        await _ctx.SaveChangesAsync(ct);
    }

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
    /// Result of a spill operation: (NewWsRowUid, NewWsId) is populated when
    /// the INSERT + trigger-driven id lookup both succeed.
    /// </summary>
    private readonly record struct OverflowWeightSheet(Guid? NewWsRowUid, long? NewWsId);

    /// <summary>
    /// Creates a fresh warehouse.WeightSheets row that copies the lot + hauler
    /// header of the given (full) source weight sheet. Used when a load save
    /// arrives on a sheet that already holds WeightSheetMaxLoads loads — the
    /// caller then links the new load to the returned sheet instead of the
    /// full one.
    ///
    /// The INSERT goes through raw SQL so the trg_WeightSheets_AutoGenerateIDs
    /// INSTEAD OF trigger can populate WeightSheetId / BaseId / As400Id; we
    /// read those back by selecting on the supplied RowUid.
    /// </summary>
    private async Task<OverflowWeightSheet> CreateOverflowWeightSheetAsync(
        Guid sourceWsUid, CancellationToken ct)
    {
        // Pull every header field we want to propagate onto the new sheet.
        var src = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(w => w.RowUid == sourceWsUid)
            .Select(w => new
            {
                w.LocationId,
                w.LotId,
                w.WeightSheetType,
                w.RateType,
                w.HaulerId,
                w.Miles,
                w.CustomRateDescription,
                w.Rate,
                w.WeightmasterName,
            })
            .FirstOrDefaultAsync(ct);

        if (src is null)
            return new OverflowWeightSheet(null, null);

        var now          = DateTime.UtcNow;
        var creationDate = DateOnly.FromDateTime(DateTime.Now);
        var newWsRowUid  = Guid.NewGuid();

        try
        {
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [warehouse].[WeightSheets]
                    (LocationId, LotId, WeightSheetType, RateType, HaulerId, Miles, CustomRateDescription, Rate, CreationDate, CreatedAt, RowUid, WeightmasterName)
                VALUES
                    ({src.LocationId}, {src.LotId}, {src.WeightSheetType}, {src.RateType}, {src.HaulerId}, {src.Miles}, {src.CustomRateDescription}, {src.Rate}, {creationDate}, {now}, {newWsRowUid}, {src.WeightmasterName})",
                ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex,
                "Failed to insert overflow WeightSheet copying source RowUid={Uid}", sourceWsUid);
            return new OverflowWeightSheet(null, null);
        }

        // Read back the trigger-generated WeightSheetId via the known RowUid.
        long? newWsId = null;
        try
        {
            var conn = (SqlConnection)_ctx.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync(ct);

            using var cmd = conn.CreateCommand();
            cmd.Transaction = _ctx.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
            cmd.CommandText = "SELECT WeightSheetId FROM [warehouse].[WeightSheets] WHERE RowUid = @uid";
            cmd.Parameters.AddWithValue("@uid", newWsRowUid);
            var result = await cmd.ExecuteScalarAsync(ct);
            if (result is long id)
                newWsId = id;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Overflow WeightSheet inserted but WeightSheetId lookup failed for RowUid={Uid}", newWsRowUid);
        }

        return new OverflowWeightSheet(newWsRowUid, newWsId);
    }

    /// <summary>
    /// Recomputes warehouse.WeightSheets.StatusId after a load is created or
    /// edited on the given weight sheet. Rules:
    ///
    ///   - If the sheet is null or already Closed (StatusId 3), do nothing.
    ///   - If non-voided load count &lt; WeightSheetMaxLoads, do nothing (the
    ///     sheet stays Open — we never auto-demote back to 0).
    ///   - If load count &gt;= WeightSheetMaxLoads:
    ///       * If every load has EndQty, a PROTEIN attribute, and a destination
    ///         container, transition to PendingFinished (2).
    ///       * Otherwise transition to PendingNotFinished (1).
    ///
    /// Safe to call repeatedly; the method only SaveChangesAsync when the
    /// computed status differs from the current value.
    /// </summary>
    private async Task RecomputeWeightSheetStatusAsync(Guid weightSheetUid, CancellationToken ct)
    {
        if (weightSheetUid == Guid.Empty) return;

        var ws = await _ctx.WeightSheets
            .FirstOrDefaultAsync(w => w.RowUid == weightSheetUid, ct);
        if (ws is null) return;
        if (ws.StatusId >= StatusClosed) return;        // don't touch a Closed sheet

        // Count non-voided loads linked to this weight sheet.
        var loadCount = await _ctx.InventoryTransactionDetails
            .CountAsync(itd => itd.RefType == "WeightSheet"
                            && itd.RefId   == weightSheetUid
                            && !itd.IsVoided, ct);

        if (loadCount < WeightSheetMaxLoads)
            return; // still Open — nothing to do

        // Resolve the active PROTEIN attribute type id once.
        var proteinTypeId = await _ctx.TransactionAttributeTypes
            .AsNoTracking()
            .Where(t => t.IsActive == true && t.Code == "PROTEIN")
            .Select(t => (int?)t.Id)
            .FirstOrDefaultAsync(ct);

        // Pull the TransactionIds for every non-voided load on this sheet.
        var txnIds = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .Where(itd => itd.RefType == "WeightSheet"
                       && itd.RefId   == weightSheetUid
                       && !itd.IsVoided)
            .Select(itd => new { itd.TransactionId, itd.EndQty })
            .ToListAsync(ct);

        // A load is "finished" iff:
        //   1. EndQty (outbound weight) is non-null.
        //   2. A PROTEIN TransactionAttribute exists with a decimal value.
        //   3. At least one InventoryTransactionDetailToContainer (bin) exists.
        bool allFinished = true;
        if (proteinTypeId is null)
        {
            allFinished = false; // can't verify protein without the type row
        }
        else
        {
            var ids = txnIds.Select(x => x.TransactionId).ToList();

            var withProtein = await _ctx.TransactionAttributes
                .AsNoTracking()
                .Where(a => ids.Contains(a.TransactionId)
                         && a.AttributeTypeId == proteinTypeId.Value
                         && a.DecimalValue != null)
                .Select(a => a.TransactionId)
                .Distinct()
                .ToListAsync(ct);
            var withProteinSet = withProtein.ToHashSet();

            var withContainer = await _ctx.InventoryTransactionDetailToContainers
                .AsNoTracking()
                .Where(c => ids.Contains(c.TransactionId))
                .Select(c => c.TransactionId)
                .Distinct()
                .ToListAsync(ct);
            var withContainerSet = withContainer.ToHashSet();

            foreach (var row in txnIds)
            {
                if (row.EndQty is null
                    || !withProteinSet.Contains(row.TransactionId)
                    || !withContainerSet.Contains(row.TransactionId))
                {
                    allFinished = false;
                    break;
                }
            }
        }

        byte target = allFinished ? StatusPendingFinished : StatusPendingNotFinished;
        if (ws.StatusId != target)
        {
            ws.StatusId = target;
            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex,
                    "Failed to update WeightSheet.StatusId for RowUid={Uid}", weightSheetUid);
            }
        }
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

    public sealed class DeleteLoadDto
    {
        public int Pin { get; set; }
    }

    // DELETE /api/GrowerDelivery/{transactionId}
    //
    // Hard-deletes a load that hasn't been weighed out yet — used to clean
    // up a no-show truck or a mis-typed entry without leaving a void row in
    // the audit trail. Refuses to touch loads that have an outbound weight,
    // a direct quantity, or a CompletedAt — those must go through Void.
    //
    // Requires PIN + DeleteLoad privilege (14, with admin priv 7 bypass).
    [HttpDelete("{transactionId:long}")]
    public async Task<IActionResult> DeleteUnweighedLoad(
        long transactionId,
        [FromBody] DeleteLoadDto dto,
        CancellationToken ct)
    {
        if (dto is null || dto.Pin <= 0)
            return BadRequest(new { message = "PIN is required." });

        var pinUser = await _ctx.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin && u.IsActive, ct);
        if (pinUser is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        var pinPrivIds = await _ctx.UserPrivileges.AsNoTracking()
            .Where(p => p.UserId == pinUser.UserId)
            .Select(p => p.PrivilegeId)
            .ToListAsync(ct);
        if (!GrainManagement.Constants.Privileges.HasPrivilege(
                pinPrivIds, GrainManagement.Constants.Privileges.DeleteLoad))
            return StatusCode(StatusCodes.Status403Forbidden,
                new { message = "User does not have permission to delete loads." });

        var detail = await _ctx.InventoryTransactionDetails
            .FirstOrDefaultAsync(d => d.TransactionId == transactionId, ct);
        if (detail is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });

        // Precondition: load must be unweighed-out. Once a load has an
        // outbound weight, a direct quantity, or a completion timestamp,
        // it can no longer be deleted.
        if (detail.EndQty.HasValue || detail.DirectQty.HasValue || detail.CompletedAt.HasValue)
            return BadRequest(new
            {
                message = "This load has already been weighed out and cannot be deleted."
            });

        var locationId = await _ctx.InventoryTransactions.AsNoTracking()
            .Where(t => t.TransactionId == transactionId)
            .Select(t => (int?)t.LocationId)
            .FirstOrDefaultAsync(ct) ?? 0;

        // Pull the WS info + TruckId attribute so the audit row carries the
        // human-meaningful identifiers (WS number, truck) alongside the raw
        // FKs. Without these, an audit reader has to chase the deleted
        // RefId guid across schemas to recover what was removed.
        var wsInfo = (detail.RefType == "WeightSheet" && detail.RefId.HasValue)
            ? await _ctx.WeightSheets.AsNoTracking()
                .Where(w => w.RowUid == detail.RefId.Value)
                .Select(w => new { w.WeightSheetId, w.As400Id })
                .FirstOrDefaultAsync(ct)
            : null;

        var truckId = await _ctx.TransactionAttributes.AsNoTracking()
            .Where(a => a.TransactionId == transactionId
                        && a.AttributeType.Code == "TRUCK_ID")
            .Select(a => a.StringValue)
            .FirstOrDefaultAsync(ct);

        // Snapshot for audit before the rows disappear. Captures everything
        // an investigator would need to identify what was deleted: the WS
        // it lived on, the truck, the inbound weight, and when it arrived.
        var oldJson = JsonSerializer.Serialize(new
        {
            detail.TransactionId,
            WeightSheetId = wsInfo?.WeightSheetId,
            WeightSheetAs400Id = wsInfo?.As400Id,
            detail.RefId,
            detail.RefType,
            detail.LotId,
            detail.AccountId,
            detail.ProductId,
            TruckId = truckId,
            StartQty = detail.StartQty,
            StartedAt = detail.StartedAt,
            detail.EndQty,
            detail.DirectQty,
            detail.CompletedAt,
        });

        // Cascade-delete the FK chain. Order matters: child rows first so
        // we don't trip an RI constraint. InventoryMovements are only
        // created on completion — guaranteed empty for an unweighed load.
        try
        {
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                DELETE FROM [Inventory].[TransactionAttributes]
                WHERE TransactionId = {transactionId}", ct);
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                DELETE FROM [Inventory].[TransactionQuantitySources]
                WHERE TransactionId = {transactionId}", ct);
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                DELETE FROM [Inventory].[InventoryTransactionDetailToContainers]
                WHERE TransactionId = {transactionId}", ct);
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                DELETE FROM [Inventory].[InventoryTransactionDetails]
                WHERE TransactionId = {transactionId}", ct);
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                DELETE FROM [Inventory].[InventoryTransactions]
                WHERE TransactionId = {transactionId}", ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to delete load TransactionId={TxnId}", transactionId);
            return StatusCode(500, new { message = "Database error while deleting the load." });
        }

        // Audit row.
        string keyJson   = JsonSerializer.Serialize(new { TransactionId = transactionId });
        string tableName = "Inventory.InventoryTransactionDetails";
        string action    = "DELETE";
        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
            VALUES ({locationId}, {pinUser.UserName}, {tableName}, {action}, {keyJson}, {oldJson}, NULL)",
            ct);

        return Ok(new { id = transactionId, deleted = true });
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

    // ────────────────────────────────────────────────────────────────────────
    // Move-load: re-parent an existing delivery load to a different weight
    // sheet. Both the source and destination must belong to this server +
    // location, the destination must be open, hold the same lot, and have
    // fewer than WeightSheetMaxLoads non-voided loads. The acting user must
    // hold PrivilegeId 2 ("Move Loads") and supply a valid PIN.
    // ────────────────────────────────────────────────────────────────────────

    private const int PrivilegeIdMoveLoads = 2;

    public class MoveLoadDto
    {
        public Guid TargetWeightSheetUid { get; set; }
        public int  Pin { get; set; }
    }

    // GET /api/GrowerDelivery/{transactionId}/move-candidates
    // Returns weight sheets the load can be moved to: this server + location,
    // open status, under 25 non-voided loads. The current sheet is excluded.
    // The lot/variety on each candidate is included so the client can warn
    // when the operator is moving across a different lot/variety.
    [HttpGet("{transactionId:long}/move-candidates")]
    public async Task<IActionResult> GetMoveLoadCandidates(long transactionId, CancellationToken ct)
    {
        var detail = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.TransactionId == transactionId, ct);

        if (detail is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });
        if (detail.RefType != "WeightSheet" || !detail.RefId.HasValue)
            return BadRequest(new { message = "Load is not currently attached to a weight sheet." });

        var txn = await _ctx.InventoryTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId, ct);
        if (txn is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });

        // Candidates: any open WS at this server+location with < cap (any lot).
        var sourceUid = detail.RefId.Value;
        var candidates = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(ws => ws.RowUid != sourceUid
                         && ws.ServerId   == _systemInfo.ServerId
                         && ws.LocationId == txn.LocationId
                         && (ws.StatusId == StatusOpen || ws.StatusId == StatusPendingNotFinished))
            .Select(ws => new
            {
                ws.WeightSheetId,
                ws.As400Id,
                ws.RowUid,
                ws.CreationDate,
                ws.StatusId,
                ws.LotId,
                LotDescription = ws.LotId != null
                    ? _ctx.Lots.Where(l => l.LotId == ws.LotId).Select(l => l.LotDescription).FirstOrDefault()
                    : null,
                Variety = ws.LotId != null
                    ? _ctx.Lots
                        .Where(l => l.LotId == ws.LotId && l.ItemId != null)
                        .Select(l => _ctx.Items
                            .Where(i => i.ItemId == l.ItemId)
                            .Select(i => i.Description).FirstOrDefault())
                        .FirstOrDefault()
                    : null,
                LoadCount = _ctx.InventoryTransactionDetails
                    .Count(d => d.RefId == ws.RowUid && d.RefType == "WeightSheet" && !d.IsVoided),
            })
            .ToListAsync(ct);

        var filtered = candidates
            .Where(c => c.LoadCount < WeightSheetMaxLoads)
            .OrderByDescending(c => c.CreationDate)
            .ToList();

        // Source lot info — returned alongside so the client can compare and
        // raise a warning when moving across lots.
        var sourceLotId = detail.LotId;
        var sourceLotDesc = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LotId == sourceLotId)
            .Select(l => l.LotDescription)
            .FirstOrDefaultAsync(ct);
        var sourceVariety = await _ctx.Lots
            .AsNoTracking()
            .Where(l => l.LotId == sourceLotId && l.ItemId != null)
            .Select(l => _ctx.Items
                .Where(i => i.ItemId == l.ItemId)
                .Select(i => i.Description).FirstOrDefault())
            .FirstOrDefaultAsync(ct);

        return Ok(new
        {
            SourceLotId    = sourceLotId,
            SourceLotDesc  = sourceLotDesc,
            SourceVariety  = sourceVariety,
            Candidates     = filtered,
        });
    }

    // POST /api/GrowerDelivery/{transactionId}/move
    // Moves a load to a different weight sheet. Audited as MOVE_LOAD.
    [HttpPost("{transactionId:long}/move")]
    public async Task<IActionResult> MoveLoad(
        long transactionId,
        [FromBody] MoveLoadDto dto,
        CancellationToken ct)
    {
        if (dto is null)
            return BadRequest(new { message = "Request body is required." });
        if (dto.TargetWeightSheetUid == Guid.Empty)
            return BadRequest(new { message = "TargetWeightSheetUid is required." });
        if (dto.Pin <= 0)
            return BadRequest(new { message = "PIN is required." });

        // ── PIN + privilege validation ──────────────────────────────────────
        var pinUser = await _ctx.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Pin == dto.Pin && u.IsActive, ct);

        if (pinUser is null)
            return Unauthorized(new { message = "Invalid or inactive PIN." });

        var hasPrivilege = await _ctx.UserPrivileges
            .AsNoTracking()
            .AnyAsync(p => p.UserId == pinUser.UserId && p.PrivilegeId == PrivilegeIdMoveLoads, ct);

        if (!hasPrivilege)
            return Unauthorized(new { message = "User does not have the Move Loads privilege." });

        // ── Load source detail + transaction ────────────────────────────────
        var detail = await _ctx.InventoryTransactionDetails
            .FirstOrDefaultAsync(d => d.TransactionId == transactionId, ct);
        if (detail is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });
        if (detail.IsVoided)
            return BadRequest(new { message = "Cannot move a voided load." });
        if (detail.RefType != "WeightSheet" || !detail.RefId.HasValue)
            return BadRequest(new { message = "Load is not currently attached to a weight sheet." });

        var txn = await _ctx.InventoryTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId, ct);
        if (txn is null)
            return NotFound(new { message = $"Transaction {transactionId} not found." });

        var sourceUid = detail.RefId.Value;
        if (sourceUid == dto.TargetWeightSheetUid)
            return BadRequest(new { message = "Load is already on that weight sheet." });

        // ── Validate destination ───────────────────────────────────────────
        var target = await _ctx.WeightSheets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.RowUid == dto.TargetWeightSheetUid, ct);

        if (target is null)
            return NotFound(new { message = "Destination weight sheet not found." });
        if (target.ServerId != _systemInfo.ServerId || target.LocationId != txn.LocationId)
            return BadRequest(new { message = "Destination weight sheet is on a different server/location." });
        if (target.StatusId != StatusOpen && target.StatusId != StatusPendingNotFinished)
            return BadRequest(new { message = "Destination weight sheet is not open." });
        // Note: cross-lot moves are intentionally allowed. The client warns the
        // operator when the destination lot/variety differs; the move itself
        // is recorded in MOVE_LOAD audit so a different-lot move is traceable.

        var targetLoadCount = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .CountAsync(d => d.RefId == target.RowUid && d.RefType == "WeightSheet" && !d.IsVoided, ct);

        if (targetLoadCount >= WeightSheetMaxLoads)
            return BadRequest(new { message = $"Destination weight sheet is full ({WeightSheetMaxLoads} loads max)." });

        // Also resolve source IDs for the audit row before mutating.
        var sourceWs = await _ctx.WeightSheets
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.RowUid == sourceUid, ct);

        // ── Perform move ────────────────────────────────────────────────────
        // Re-stamp the detail to fit the new parent's WS type:
        //   * Delivery target → TxnType='RECEIVE', Direction=+1, LotId=target.LotId
        //   * Transfer target → TxnType='TRANSFER_IN'/'TRANSFER_OUT' based on the
        //     load's role at this location, LotId=NULL.
        detail.RefId     = target.RowUid;
        detail.UpdatedAt = DateTime.UtcNow;
        var targetType = (target.WeightSheetType ?? "").ToLowerInvariant();
        if (targetType == "transfer")
        {
            short newDirection = (target.DestinationLocationId == target.LocationId) ? (short)1 : (short)-1;
            detail.LotId     = null;
            detail.Direction = newDirection;
            detail.TxnType   = newDirection == 1 ? "TRANSFER_IN" : "TRANSFER_OUT";
        }
        else if (targetType == "delivery")
        {
            if (target.LotId.HasValue)
                detail.LotId = target.LotId.Value;
            detail.TxnType   = "RECEIVE";
            detail.Direction = 1;
        }

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to move load TransactionId={TxnId}", transactionId);
            return StatusCode(500, new { message = "Database error while moving load." });
        }

        // ── Audit ───────────────────────────────────────────────────────────
        // Audit payload labels match the operator-facing terminology in the
        // AuditTrail viewer:
        //   As400Id — the AS400 / Agvantage id operators recognize
        //   LoadId  — the transaction id of the load that moved
        // The acting user is already captured in AuditTrail.UserName (the
        // "User" column in the viewer), so it isn't repeated in the payload.
        string oldJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            ["As400Id"] = sourceWs?.As400Id,
            ["LoadId"]  = transactionId,
        });
        string newJson = JsonSerializer.Serialize(new Dictionary<string, object>
        {
            ["As400Id"] = target.As400Id,
            ["LoadId"]  = transactionId,
        });
        string keyJson   = JsonSerializer.Serialize(new { TransactionId = transactionId });
        string tableName = "Inventory.InventoryTransactionDetails";
        string action    = "MOVE_LOAD";
        int locationId   = txn.LocationId;

        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
            VALUES ({locationId}, {pinUser.UserName}, {tableName}, {action}, {keyJson}, {oldJson}, {newJson})",
            ct);

        // Recompute statuses on both sheets — moving may flip pending↔finished.
        await RecomputeWeightSheetStatusAsync(sourceUid, ct);
        await RecomputeWeightSheetStatusAsync(target.RowUid, ct);

        // Notify both sheets so source-WS view loses the row and target-WS
        // view gains it. Same locationId; only the WS id differs.
        if (sourceWs?.WeightSheetId is long fromId)
            await _notifier.NotifyAsync(locationId, fromId, "load-moved-out", ct);
        await _notifier.NotifyAsync(locationId, target.WeightSheetId, "load-moved-in", ct);

        return Ok(new
        {
            id = transactionId,
            fromWeightSheetId = sourceWs?.WeightSheetId,
            toWeightSheetId   = target.WeightSheetId,
            toWeightSheetUid  = target.RowUid,
        });
    }

    // ────────────────────────────────────────────────────────────────────────
    // Manual Pending / Re-open transitions on a weight sheet.
    //   POST /WeightSheet/{id}/pending
    //     - Validates every non-voided load has outbound weight + a bin/container
    //     - Returns 400 with `incompleteLoads` if any are missing those
    //     - If any load is missing protein, returns 200 with
    //       { requiresProteinAck = true, missingProteinLoads = [...] }
    //       unless the body says { acceptMissingProtein: true }
    //     - On success transitions to PendingFinished and writes a SET_PENDING audit row
    //   POST /WeightSheet/{id}/reopen
    //     - Allowed when current status is Pending* and load count < cap
    //     - Transitions to Open and writes a REOPEN audit row
    // ────────────────────────────────────────────────────────────────────────

    public class SetPendingDto
    {
        public bool AcceptMissingProtein { get; set; }
    }

    [HttpPost("WeightSheet/{id:long}/pending")]
    public async Task<IActionResult> SetWeightSheetPending(
        long id,
        [FromBody] SetPendingDto dto,
        CancellationToken ct)
    {
        dto ??= new SetPendingDto();

        var ws = await _ctx.WeightSheets
            .FirstOrDefaultAsync(w => w.WeightSheetId == id && w.ServerId == _systemInfo.ServerId, ct);
        if (ws is null)
            return NotFound(new { message = $"Weight sheet {id} not found." });
        if (ws.StatusId == StatusClosed)
            return BadRequest(new { message = "This weight sheet is closed." });
        if (ws.StatusId == StatusPendingFinished)
            return Ok(new { weightSheetId = ws.WeightSheetId, statusId = (int)ws.StatusId, alreadyPending = true });

        // Pull the non-voided loads and check completeness.
        var loads = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .Where(d => d.RefId == ws.RowUid && d.RefType == "WeightSheet" && !d.IsVoided)
            .Select(d => new
            {
                d.TransactionId,
                d.StartQty,
                d.EndQty,
                d.DirectQty,
                HasContainer = _ctx.InventoryTransactionDetailToContainers
                    .Any(c => c.TransactionId == d.TransactionId),
                ProteinValue = _ctx.TransactionAttributes
                    .Where(a => a.TransactionId == d.TransactionId
                                && a.AttributeType.Code == "PROTEIN")
                    .Select(a => (decimal?)a.DecimalValue)
                    .FirstOrDefault(),
            })
            .ToListAsync(ct);

        // "Complete enough to set Pending" = a recorded final weight + a bin.
        // Direct loads need DirectQty + container; truck loads need EndQty + container.
        var incomplete = loads
            .Where(l =>
            {
                bool hasFinalWeight = l.DirectQty.HasValue
                    || (l.StartQty.HasValue && l.EndQty.HasValue);
                return !hasFinalWeight || !l.HasContainer;
            })
            .Select(l => new { l.TransactionId })
            .ToList();

        if (incomplete.Count > 0)
        {
            return BadRequest(new
            {
                message = "Some loads are not complete.",
                incompleteLoads = incomplete,
            });
        }

        var missingProtein = loads
            .Where(l => !(l.ProteinValue.HasValue && l.ProteinValue.Value > 0))
            .Select(l => new { l.TransactionId })
            .ToList();

        if (missingProtein.Count > 0 && !dto.AcceptMissingProtein)
        {
            return Ok(new
            {
                requiresProteinAck = true,
                missingProteinLoads = missingProtein,
            });
        }

        var oldStatus = (int)ws.StatusId;
        ws.StatusId = StatusPendingFinished;
        ws.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to set WS {WsId} to Pending", id);
            return StatusCode(500, new { message = "Database error while updating weight sheet status." });
        }

        // Audit
        string oldJson   = JsonSerializer.Serialize(new { StatusId = oldStatus });
        string newJson   = JsonSerializer.Serialize(new { StatusId = (int)ws.StatusId });
        string keyJson   = JsonSerializer.Serialize(new { WeightSheetId = ws.WeightSheetId });
        string tableName = "warehouse.WeightSheets";
        string action    = "SET_PENDING";
        // Prefer the Remote Admin cookie name (operator-recognizable on Remote
        // deployments) and fall back to the Microsoft Identity UPN, then "system".
        string userName  = ResolveAuditUserName();

        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
            VALUES ({ws.LocationId}, {userName}, {tableName}, {action}, {keyJson}, {oldJson}, {newJson})",
            ct);

        await _notifier.NotifyAsync(ws.LocationId, ws.WeightSheetId, "ws-finished", ct);

        return Ok(new
        {
            weightSheetId = ws.WeightSheetId,
            statusId      = (int)ws.StatusId,
            acceptedMissingProtein = dto.AcceptMissingProtein && missingProtein.Count > 0,
        });
    }

    // POST /api/GrowerDelivery/WeightSheet/{id}/finish-and-close-lot
    //   Finishes this WS and closes its lot in one step. The lot can only be
    //   closed when EVERY non-voided load on the lot (across every WS that
    //   uses it) is complete. Returns 400 with `incompleteLoads` listing
    //   {WeightSheetId, As400Id, TransactionId, Reason} when any are
    //   missing outbound weight or a bin. When loads have weight + bin but
    //   no protein, returns 200 with `requiresProteinAck = true` until the
    //   client retries with AcceptMissingProtein = true.
    [HttpPost("WeightSheet/{id:long}/finish-and-close-lot")]
    public async Task<IActionResult> FinishAndCloseLot(
        long id,
        [FromBody] SetPendingDto dto,
        CancellationToken ct)
    {
        dto ??= new SetPendingDto();

        var ws = await _ctx.WeightSheets
            .FirstOrDefaultAsync(w => w.WeightSheetId == id && w.ServerId == _systemInfo.ServerId, ct);
        if (ws is null)
            return NotFound(new { message = $"Weight sheet {id} not found." });
        if (ws.StatusId == StatusClosed)
            return BadRequest(new { message = "This weight sheet is closed." });
        if (!ws.LotId.HasValue)
            return BadRequest(new { message = "This weight sheet has no lot to close." });

        var lotId = ws.LotId.Value;
        var lot = await _ctx.Lots.FirstOrDefaultAsync(l => l.LotId == lotId, ct);
        if (lot is null)
            return BadRequest(new { message = "Lot not found." });

        // Pull every non-voided load on the lot (across all WSs of that lot)
        // and check completeness. Surface WS info on each row so the client
        // can guide the operator to the affected sheet.
        var lotLoads = await (
            from d in _ctx.InventoryTransactionDetails.AsNoTracking()
            where d.LotId == lotId && d.RefType == "WeightSheet" && !d.IsVoided
            join w in _ctx.WeightSheets.AsNoTracking() on d.RefId equals w.RowUid
            select new
            {
                d.TransactionId,
                d.StartQty,
                d.EndQty,
                d.DirectQty,
                w.WeightSheetId,
                w.As400Id,
                HasContainer = _ctx.InventoryTransactionDetailToContainers
                    .Any(c => c.TransactionId == d.TransactionId),
                ProteinValue = _ctx.TransactionAttributes
                    .Where(a => a.TransactionId == d.TransactionId
                                && a.AttributeType.Code == "PROTEIN")
                    .Select(a => (decimal?)a.DecimalValue)
                    .FirstOrDefault(),
            }
        ).ToListAsync(ct);

        if (lotLoads.Count == 0)
            return BadRequest(new { message = "Lot has no loads to finish." });

        var incomplete = lotLoads
            .Select(l =>
            {
                bool hasFinalWeight = l.DirectQty.HasValue
                    || (l.StartQty.HasValue && l.EndQty.HasValue);
                string reason = !hasFinalWeight
                    ? "missing outbound weight"
                    : (!l.HasContainer ? "missing bin" : null);
                return (l, reason);
            })
            .Where(x => x.reason != null)
            .Select(x => new
            {
                x.l.WeightSheetId,
                x.l.As400Id,
                x.l.TransactionId,
                Reason = x.reason,
            })
            .ToList();

        if (incomplete.Count > 0)
        {
            return BadRequest(new
            {
                message = "Lot cannot be closed — some loads are not complete.",
                incompleteLoads = incomplete,
            });
        }

        var missingProtein = lotLoads
            .Where(l => !(l.ProteinValue.HasValue && l.ProteinValue.Value > 0))
            .Select(l => new { l.WeightSheetId, l.As400Id, l.TransactionId })
            .ToList();

        if (missingProtein.Count > 0 && !dto.AcceptMissingProtein)
        {
            return Ok(new
            {
                requiresProteinAck = true,
                missingProteinLoads = missingProtein,
            });
        }

        // Finish this WS (PendingFinished) and close the lot.
        var oldWsStatus = (int)ws.StatusId;
        ws.StatusId  = StatusPendingFinished;
        ws.UpdatedAt = DateTime.UtcNow;

        bool wasOpen = lot.IsOpen;
        lot.IsOpen    = false;
        lot.UpdatedAt = DateTime.UtcNow;

        try { await _ctx.SaveChangesAsync(ct); }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to finish-and-close-lot for WS={WsId}, LotId={LotId}", id, lotId);
            return StatusCode(500, new { message = "Database error while finishing & closing lot." });
        }

        // Audit: WS status change + lot close.
        string userName = ResolveAuditUserName();
        int locId = ws.LocationId;
        string keyJsonWs = JsonSerializer.Serialize(new { WeightSheetId = ws.WeightSheetId });
        string oldJsonWs = JsonSerializer.Serialize(new { StatusId = oldWsStatus });
        string newJsonWs = JsonSerializer.Serialize(new { StatusId = (int)ws.StatusId });
        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
            VALUES ({locId}, {userName}, {"warehouse.WeightSheets"}, {"SET_PENDING"}, {keyJsonWs}, {oldJsonWs}, {newJsonWs})",
            ct);

        if (wasOpen)
        {
            string keyJsonLot = JsonSerializer.Serialize(new { LotId = lotId });
            string oldJsonLot = JsonSerializer.Serialize(new { IsOpen = true });
            string newJsonLot = JsonSerializer.Serialize(new { IsOpen = false });
            await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
                VALUES ({locId}, {userName}, {"Inventory.Lots"}, {"CLOSE"}, {keyJsonLot}, {oldJsonLot}, {newJsonLot})",
                ct);
        }

        // Notify every WS at this location — closing the lot affects the
        // closed-lot guard on every WS that uses it. Cheapest: broadcast each
        // WS that shares the lot.
        var lotWsIds = await _ctx.WeightSheets
            .AsNoTracking()
            .Where(w => w.LotId == lotId && w.LocationId == ws.LocationId)
            .Select(w => w.WeightSheetId)
            .ToListAsync(ct);
        foreach (var wsAffected in lotWsIds)
            await _notifier.NotifyAsync(ws.LocationId, wsAffected, "lot-closed", ct);

        return Ok(new
        {
            weightSheetId          = ws.WeightSheetId,
            statusId               = (int)ws.StatusId,
            lotId                  = lotId,
            lotClosed              = !lot.IsOpen,
            acceptedMissingProtein = dto.AcceptMissingProtein && missingProtein.Count > 0,
        });
    }

    [HttpPost("WeightSheet/{id:long}/reopen")]
    public async Task<IActionResult> ReopenWeightSheet(long id, CancellationToken ct)
    {
        var ws = await _ctx.WeightSheets
            .FirstOrDefaultAsync(w => w.WeightSheetId == id && w.ServerId == _systemInfo.ServerId, ct);
        if (ws is null)
            return NotFound(new { message = $"Weight sheet {id} not found." });

        if (ws.StatusId != StatusPendingNotFinished && ws.StatusId != StatusPendingFinished)
            return BadRequest(new { message = "Only finished weight sheets can be re-opened." });

        var loadCount = await _ctx.InventoryTransactionDetails
            .AsNoTracking()
            .CountAsync(d => d.RefId == ws.RowUid && d.RefType == "WeightSheet" && !d.IsVoided, ct);

        if (loadCount >= WeightSheetMaxLoads)
            return BadRequest(new { message = $"This weight sheet already has {WeightSheetMaxLoads} loads — cannot re-open." });

        var oldStatus = (int)ws.StatusId;
        ws.StatusId = StatusOpen;
        ws.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to re-open WS {WsId}", id);
            return StatusCode(500, new { message = "Database error while re-opening weight sheet." });
        }

        string oldJson   = JsonSerializer.Serialize(new { StatusId = oldStatus });
        string newJson   = JsonSerializer.Serialize(new { StatusId = (int)ws.StatusId });
        string keyJson   = JsonSerializer.Serialize(new { WeightSheetId = ws.WeightSheetId });
        string tableName = "warehouse.WeightSheets";
        string action    = "REOPEN";
        // Prefer the Remote Admin cookie name (operator-recognizable on Remote
        // deployments) and fall back to the Microsoft Identity UPN, then "system".
        string userName  = ResolveAuditUserName();

        await _ctx.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO [system].[AuditTrail] (LocationId, UserName, TableName, Action, KeyJson, OldJson, NewJson)
            VALUES ({ws.LocationId}, {userName}, {tableName}, {action}, {keyJson}, {oldJson}, {newJson})",
            ct);

        await _notifier.NotifyAsync(ws.LocationId, ws.WeightSheetId, "ws-reopened", ct);

        return Ok(new { weightSheetId = ws.WeightSheetId, statusId = (int)ws.StatusId });
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
