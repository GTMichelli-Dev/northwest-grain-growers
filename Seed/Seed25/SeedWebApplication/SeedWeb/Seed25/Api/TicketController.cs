using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Seed25.Models;

namespace Seed25.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly Seed_DataContext _context;

        public TicketController(Seed_DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            // Example: return all SampleOrder records
            var data = _context.Set<SampleOrder>().ToList();
            return Ok(data);
        }

       
        public class CreateTicketRequest
        {
            public string ScaleDescription { get; set; }
            public string Printer { get; set; }
        }




        [HttpPost("createNewTicket")]
        public IActionResult createNewTicket()
        {

            
        //    var allowed = _context.PcAllowedToPrints
        //.FirstOrDefault(p => p.PcAddress == request.ScaleDescription || p.PcAddress == request.Printer);

        //    if (allowed == null)
        //    {
        //        return BadRequest("PC not allowed to print for this scale or printer.");
        //    }

          

            return Ok(new { Success = true });
        }

        [HttpGet("tickets")]
        public IActionResult GetTickets(int? LocationId)
        {
            var query = _context.SeedTickets.AsQueryable();

            if (LocationId.HasValue)
            {
                query = query.Where(t => t.LocationId == LocationId.Value);
            }

            var tickets = query.ToList();
            return Ok(tickets);
        }

        [HttpGet("locations")]
        public IActionResult GetLocations()
        {
            var locations = _context.Locations
                .Where(x=> x.Id>0)
                .OrderBy(l => l.LocationCode)
                .Select(l => new LocationDTO
                {
                    Uid = l.Uid,
                    Id = l.Id,
                    LocationCode = l.LocationCode,
                    Description = l.Description,
                    PlcPcAddress = l.PlcPcAddress,
                    WebsiteAddress = l.WebsiteAddress,
                    TicketSeed = l.TicketSeed,
                    ReportPrinter = l.ReportPrinter,
                    TicketPrinter = l.TicketPrinter,

                    Address1 = l.Address1,
                    Address2 = l.Address2,
                    Address3 = l.Address3

                    
                })
                .ToList();

            return Ok(locations);
        }
    }
}