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
                .OrderByDescending(l => l.Id)
                .Select(l => new
                {
                    Id        = l.Id,
                    LotNumber = l.LotDescription ?? l.Id.ToString()
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
                RowGuid        = Guid.NewGuid(),
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

            return Ok(new { id = lot.Id, lotNumber = lot.LotDescription });
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

        // GET /api/Lookups/Containers
        // Active, undestroyed containers.
        [HttpGet("Containers")]
        public async Task<IActionResult> Containers(CancellationToken ct)
        {
            var data = await _ctx.Containers
                .AsNoTracking()
                .Where(c => c.IsActive == true && c.Destroyed != true)
                .OrderBy(c => c.Description)
                .Select(c => new { Id = c.Id, Name = c.Description })
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
