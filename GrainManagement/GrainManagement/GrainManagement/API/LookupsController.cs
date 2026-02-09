using GrainManagement.Models;
using GrainManagement.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrainManagement.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupsController : ControllerBase
    {
      
        private readonly dbContext _ctx;
        private readonly ILogger<LookupsController> _logger;
       


        public LookupsController(dbContext ctx, ILogger<LookupsController> logger, IServerInfoProvider serverInfoProvider)
        {
            _ctx = ctx;
            _logger = logger;
            
        }

        [HttpGet("ActiveWarehouseLocations")]
        public async Task<IActionResult> ActiveWarehouseLocations(CancellationToken ct)
        {
            //try
            //{
            //    var server = await _serverInfoProvider.GetAsync(ct);
            //    return Ok(server);
            //}
            //catch
            //{
            //    return BadRequest(new { message = "Server Not Configured." });
            //}
            return BadRequest(new { message = "Server Not Configured." });
        }

    }
}
