using GrainManagement.Dtos;
using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
        private readonly dbContext _ctx;
        private readonly ILogger<LookupsController> _logger;

        public LookupsController(dbContext ctx, ILogger<LookupsController> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        // GET /api/Lookups/Accounts
        // Active producer accounts for the grower delivery selector.
        [HttpGet("ProducerAccounts")]
        public async Task<IActionResult> ProducerAccounts(CancellationToken ct)
        {
            var data = await _ctx.Accounts
                .AsNoTracking()
                .Where(a => a.IsProducer == true)
                .OrderBy(a => a.EntityName)
                .Select(a => new { AccountId = a.AccountId, Name = a.LookupName + " (" + a.As400AccountId + ")" })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/ProducerAccountsForItem?itemId=&locationId=
        // Returns producer accounts allowed to bring in the specified item at the given location.
        // If the item+location has ANY AccountItemFilter rows, only those accounts are allowed.
        // If no filter rows exist for the item+location, all producer accounts are allowed.
        [HttpGet("ProducerAccountsForItem")]
        public async Task<IActionResult> ProducerAccountsForItem(
            [FromQuery] long itemId,
            [FromQuery] int locationId,
            CancellationToken ct)
        {
            // Check if any filters exist for this item at this location
            var allowedAccountIds = await _ctx.AccountItemFilters
                .AsNoTracking()
                .Where(f => f.LocationId == locationId && f.ItemId == itemId)
                .Select(f => f.AccountId)
                .Distinct()
                .ToListAsync(ct);

            // If filters exist, only return those accounts; otherwise return all
            var query = _ctx.Accounts
                .AsNoTracking()
                .Where(a => a.IsProducer == true);

            if (allowedAccountIds.Count > 0)
                query = query.Where(a => allowedAccountIds.Contains(a.AccountId));

            var data = await query
                .OrderBy(a => a.EntityName)
                .Select(a => new { a.AccountId, Name = a.LookupName + " (" + a.As400AccountId + ")" })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/WarehouseItems
        // Active, visible grain products (CropId required), ordered by description descending.
        [HttpGet("WarehouseItems")]
        public async Task<IActionResult> WarehouseItems(CancellationToken ct)
        {
            var data = await _ctx.Items
                .AsNoTracking()
                .Where(I => I.IsActive == true && I.Product.IsHidden != true && I.ItemTraits.FirstOrDefault(x=>x.TraitId==32) != null)
                .OrderByDescending(p => p.Description)
                .Select(p => new
                {
                    ItemId = p.ItemId,
                    Name      = p.Description
                })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/Lots?productId=
        // Open lots, optionally filtered by product.
        [HttpGet("Lots")]
        public async Task<IActionResult> Lots([FromQuery] int? productId, CancellationToken ct)
        {
            var query = _ctx.Lots
                .AsNoTracking()
                .Where(l => l.IsOpen == true);

            if (productId.HasValue)
                query = query.Where(l => l.ProductId == productId.Value);

            var data = await query
                .OrderByDescending(l => l.LotId)
                .Select(l => new
                {
                    Id        = l.LotId,
                    LotNumber = l.LotDescription ?? l.LotId.ToString()
                })
                .ToListAsync(ct);

            return Ok(data);
        }

        // POST /api/Lookups/Lots
        // Create a new open lot. Uses admin connection for writes.
        [UseAdminConnection]
        [HttpPost("Lots")]
        public async Task<IActionResult> CreateLot([FromBody] CreateLotDto dto, CancellationToken ct)
        {
            if (dto is null)
                return BadRequest(new { message = "Request body is required." });
            if (dto.ProductId <= 0)
                return BadRequest(new { message = "ProductId is required." });
            if (string.IsNullOrWhiteSpace(dto.LotDescription))
                return BadRequest(new { message = "Lot description is required." });

            var lot = new Lot
            {
                ProductId      = dto.ProductId,
                LotDescription = dto.LotDescription.Trim(),
                SplitGroupId   = dto.SplitGroupId,
                IsOpen         = true,
                CreatedAt      = DateTime.UtcNow,
            };

            _ctx.Lots.Add(lot);

            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to create lot for ProductId={ProductId}", dto.ProductId);
                return StatusCode(500, new { message = "Database error while creating lot." });
            }

            return Ok(new { id = lot.LotId, lotNumber = lot.LotDescription });
        }

        // GET /api/Lookups/UnitOfMeasures
        // Active units of measure.
        [HttpGet("UnitOfMeasures")]
        public async Task<IActionResult> UnitOfMeasures(CancellationToken ct)
        {
            var data = await _ctx.UnitOfMeasures
                .AsNoTracking()
                .Where(u => u.IsActive == true)
                .OrderBy(u => u.Code)
                .Select(u => new
                {
                    UomId       = u.UomId,
                    Code        = u.Code,
                    Description = u.Description
                })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/ContainerBins?locationId={id}
        // Active containers for a location, with storage location and notes.
        [HttpGet("ContainerBins")]
        public async Task<IActionResult> ContainerBins(int locationId, CancellationToken ct)
        {
            var data = await _ctx.Containers
                .AsNoTracking()
                .Where(c => c.LocationId == locationId && c.IsActive == true && c.Destroyed != true)
                .OrderBy(c => c.ContainerLocation != null ? c.ContainerLocation.Description : "")
                .ThenBy(c => c.Description)
                .Select(c => new {
                    ContainerId           = c.ContainerId,
                    ContainerDescription  = c.Description,
                    LocationDescription   = c.ContainerLocation != null ? c.ContainerLocation.Description : null,
                    Notes                 = c.Notes
                })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/Containers
        // Active, undestroyed containers.
        [HttpGet("Containers")]
        public async Task<IActionResult> Containers(CancellationToken ct)
        {
            var data = await _ctx.Containers
                .AsNoTracking()
                .Where(c => c.IsActive == true && c.Destroyed != true)
                .OrderBy(c => c.Description)
                .Select(c => new { Id = c.ContainerId, Name = c.Description })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/SplitGroups
        // Active split groups enabled for receiving.
        [HttpGet("SplitGroups")]
        public async Task<IActionResult> SplitGroups(CancellationToken ct)
        {
            var data = await _ctx.SplitGroups
                .AsNoTracking()
                .Where(s => s.IsActive == true && s.UseForReceive == true)
                .OrderBy(s => s.SplitGroupDescription)
                .Select(s => new
                {
                    SplitGroupId = s.SplitGroupId,
                    Name         = s.SplitGroupDescription
                })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/Products
        // All active, non-hidden products — for admin use.
        [HttpGet("Products")]
        public async Task<IActionResult> Products(CancellationToken ct)
        {
            var data = await _ctx.Products
                .AsNoTracking()
                .Where(p => p.IsActive && !p.IsHidden)
                .OrderBy(p => p.Description)
                .Select(p => new { p.ProductId, p.Description })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/ItemsByProduct?productId=
        // Active items belonging to the specified product.
        [HttpGet("ItemsByProduct")]
        public async Task<IActionResult> ItemsByProduct([FromQuery] int productId, CancellationToken ct)
        {
            if (productId <= 0)
                return BadRequest(new { message = "productId is required." });

            var data = await _ctx.Items
                .AsNoTracking()
                .Where(i => i.ProductId == productId && i.IsActive)
                .OrderBy(i => i.Description)
                .Select(i => new { i.ItemId, i.Description })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/States
        // All states with Id, name, and abbreviation for admin editing.
        [HttpGet("States")]
        public async Task<IActionResult> States(CancellationToken ct)
        {
            var data = await _ctx.States
                .AsNoTracking()
                .OrderBy(s => s.State1)
                .Select(s => new
                {
                    s.Id,
                    StateName = s.State1,
                    StateAbv  = s.StateAbv
                })
                .ToListAsync(ct);

            return Ok(data);
        }

        // PATCH /api/Lookups/States/{id}
        // Update a state's abbreviation.
        [UseAdminConnection]
        [HttpPatch("States/{id}")]
        public async Task<IActionResult> UpdateState(int id, [FromBody] UpdateAbvDto dto, CancellationToken ct)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Abv))
                return BadRequest(new { message = "Abbreviation is required." });

            var state = await _ctx.States.FindAsync(new object[] { id }, ct);
            if (state is null)
                return NotFound(new { message = "State not found." });

            state.StateAbv = dto.Abv.Trim();

            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to update state abbreviation for Id={Id}", id);
                return StatusCode(500, new { message = "Database error while updating state." });
            }

            return Ok(new { state.Id, StateAbv = state.StateAbv });
        }

        // GET /api/Lookups/Counties?stateId=
        // Counties for a given state, with Id, name, and abbreviation.
        [HttpGet("Counties")]
        public async Task<IActionResult> Counties([FromQuery] int stateId, CancellationToken ct)
        {
            if (stateId <= 0)
                return BadRequest(new { message = "stateId is required." });

            var data = await _ctx.Counties
                .AsNoTracking()
                .Where(c => c.StateId == stateId)
                .OrderBy(c => c.County1)
                .Select(c => new
                {
                    c.Id,
                    CountyName = c.County1,
                    CountyAbv  = c.CountyAbv
                })
                .ToListAsync(ct);

            return Ok(data);
        }

        // PATCH /api/Lookups/Counties/{id}
        // Update a county's abbreviation.
        [UseAdminConnection]
        [HttpPatch("Counties/{id}")]
        public async Task<IActionResult> UpdateCounty(int id, [FromBody] UpdateAbvDto dto, CancellationToken ct)
        {
            if (dto is null || string.IsNullOrWhiteSpace(dto.Abv))
                return BadRequest(new { message = "Abbreviation is required." });

            var county = await _ctx.Counties.FindAsync(new object[] { id }, ct);
            if (county is null)
                return NotFound(new { message = "County not found." });

            county.CountyAbv = dto.Abv.Trim();

            try
            {
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Failed to update county abbreviation for Id={Id}", id);
                return StatusCode(500, new { message = "Database error while updating county." });
            }

            return Ok(new { county.Id, CountyAbv = county.CountyAbv });
        }

        // GET /api/Lookups/Haulers
        // Active haulers for the weight sheet hauler selector.
        [HttpGet("Haulers")]
        public async Task<IActionResult> Haulers(CancellationToken ct)
        {
            var data = await _ctx.Haulers
                .AsNoTracking()
                .Where(h => h.IsActive)
                .OrderBy(h => h.Description)
                .Select(h => new { h.Id, h.Description })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/HaulerRateForMiles?rateType=A&miles=10
        // Looks up the hauler rate tier that covers the given mileage for the specified rate type.
        [HttpGet("HaulerRateForMiles")]
        public async Task<IActionResult> HaulerRateForMiles(
            [FromQuery] string rateType,
            [FromQuery] decimal miles,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(rateType))
                return BadRequest(new { message = "rateType is required." });

            var rate = await _ctx.WeightSheetHaulerRates
                .AsNoTracking()
                .Where(r => r.RateType == rateType && r.MaxDistance >= miles)
                .OrderBy(r => r.MaxDistance)
                .Select(r => new { r.Rate, r.MaxDistance })
                .FirstOrDefaultAsync(ct);

            if (rate == null)
                return NotFound(new { message = "No rate tier covers the specified mileage." });

            return Ok(rate);
        }

        // GET /api/Lookups/QuantityMethods?locationId={id}
        // Active quantity methods configured for a location via LocationQuantityMethods.
        // Falls back to all active methods if the table is empty or not yet created.
        [HttpGet("QuantityMethods")]
        public async Task<IActionResult> QuantityMethods([FromQuery] int locationId, CancellationToken ct)
        {
            if (locationId <= 0)
                return BadRequest(new { message = "locationId is required." });

            try
            {
                var data = await _ctx.LocationQuantityMethods
                    .AsNoTracking()
                    .Where(lqm => lqm.LocationId == locationId && lqm.QuantityMethod.IsActive)
                    .OrderBy(lqm => lqm.QuantityMethod.Code)
                    .Select(lqm => new
                    {
                        lqm.QuantityMethod.QuantityMethodId,
                        lqm.QuantityMethod.Code,
                        lqm.QuantityMethod.Description
                    })
                    .ToListAsync(ct);

                if (data.Count > 0)
                    return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "LocationQuantityMethods query failed (table may not exist yet). Falling back to all active methods.");
            }

            // Fallback: return all active quantity methods
            var fallback = await _ctx.QuantityMethods
                .AsNoTracking()
                .Where(m => m.IsActive)
                .OrderBy(m => m.Code)
                .Select(m => new
                {
                    m.QuantityMethodId,
                    m.Code,
                    m.Description
                })
                .ToListAsync(ct);

            return Ok(fallback);
        }

        // GET /api/Lookups/QuantitySourceTypes
        // All active quantity source types.
        [HttpGet("QuantitySourceTypes")]
        public async Task<IActionResult> QuantitySourceTypes(CancellationToken ct)
        {
            var data = await _ctx.QuantitySourceTypes
                .AsNoTracking()
                .Where(q => q.IsActive)
                .OrderBy(q => q.Code)
                .Select(q => new
                {
                    q.QuantitySourceTypeId,
                    q.Code,
                    q.Description
                })
                .ToListAsync(ct);

            return Ok(data);
        }

        // GET /api/Lookups/StatesWithCounties
        // States with their counties from the database.
        [HttpGet("StatesWithCounties")]
        public async Task<IActionResult> StatesWithCounties(CancellationToken ct)
        {
            var data = await _ctx.States
                .AsNoTracking()
                .OrderBy(s => s.State1)
                .Select(s => new
                {
                    State     = s.StateAbv,
                    StateName = s.State1,
                    Counties  = s.Counties
                        .OrderBy(c => c.County1)
                        .Select(c => c.County1)
                        .ToList()
                })
                .ToListAsync(ct);

            return Ok(data);
        }
    }
}
