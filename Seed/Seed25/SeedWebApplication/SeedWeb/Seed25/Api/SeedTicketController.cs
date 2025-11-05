using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seed25.DTO;
using Seed25.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Seed25.Api
{


    public class TicketSummaryRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int LocationId { get; set; }
    }





    [Route("api/[controller]")]
    [ApiController]
    public class SeedTicketController : ControllerBase
    {



        private readonly Seed_DataContext _db;
        public SeedTicketController(Seed_DataContext db) => _db = db;







[HttpGet("GetTicketSummary")]
    public async Task<IActionResult> GetTicketSummary(
    DataSourceLoadOptions loadOptions,
    DateTime fromDate,
    DateTime toDate,
    int locationId = 0,
    CancellationToken ct = default)
    {
        var start = fromDate.Date;
        var endEx = toDate.Date.AddDays(1);

        var qry = _db.SeedTickets.AsNoTracking()
            .Where(t => t.TicketDate >= start && t.TicketDate < endEx);

        if (locationId > 0)
            qry = qry.Where(t => t.LocationId == locationId);

        // Project to DTO in IQueryable so search/sort/paging happen in SQL
        var projected = qry.Select(t => new SeedTicketSummaryDTO
        {
            UID = t.Uid,
            Ticket = t.Ticket ?? 0,
            TicketType = t.TicketType ?? Enumerations.TicketType.Unknown.ToString(),
            Status = t.Void ? Enumerations.TicketStatus.Voided.ToString()
                         : t.Complete ? Enumerations.TicketStatus.Finished.ToString()
                         : t.Pending ? Enumerations.TicketStatus.Pending.ToString()
                         : Enumerations.TicketStatus.Incomplete.ToString(),
            InvoiceDate = t.TicketDate,
            LocationId = t.LocationId,
            CustomerName = t.GrowerName ?? "",
            PO = t.Po ?? "",
            BOL = t.Bol ?? "",
            TruckId = t.TruckId ?? "",
            Comments = t.Comments ?? "",
            Location = _db.Locations.Where(x => x.Id == t.LocationId)
                               .Select(x => x.Description)
                               .FirstOrDefault() ?? "",

            Varieties = t.SeedTicketVarieties.Select(v => new VarietyDescriptionDTO
            {
                Id = v.VarietyId,
                Lot = v.Lot ?? string.Empty,
                Description = _db.Items.Where(i => i.Id == v.VarietyId)
                    .Select(i => i.Description)
                    .FirstOrDefault() ?? v.CustomName
            }).ToList()
        });

        // Let DevExtreme build SQL for search/filter/sort/paging over all rows
        var result = await DataSourceLoader.LoadAsync(projected, loadOptions, ct);

        // Add StrTotallbs formatting on the materialized result if you need it in the grid:
        // If you need StrTotallbs server-side, load IDs and post-process; otherwise keep as is.

        return new JsonResult(result);
    }






    // POST: api/SeedTicket/GetTicketSummary
    [HttpPost("GetTicketSummary")]
        public async Task<IEnumerable<SeedTicketSummaryDTO>> GetTicketSummary(
            [FromBody] TicketSummaryRequest request, CancellationToken ct = default)
        {
            if (request is null) return new List<SeedTicketSummaryDTO>();

            var start = request.FromDate.Date;                 // 00:00 local
            var endExclusive = request.ToDate.Date.AddDays(1); // next day 00:00

            var baseQry = _db.SeedTickets
                .AsNoTracking()
                .Where(t => t.TicketDate >= start && t.TicketDate < endExclusive);
            //    .Where(t => t.Ticket == 4017479);

            //if (request.LocationId > 0)
            //    baseQry = baseQry.Where(t => t.LocationId == request.LocationId);

            // First project to: { Ticket = DTO w/out StrTotallbs, AllDone, SumLbs }
            var rows = await baseQry
                .OrderByDescending(t => t.TicketDate)
                .Select(t => new
                {
                    Ticket = new SeedTicketSummaryDTO
                    {
                        UID = t.Uid,
                        Ticket = t.Ticket ?? 0,
                        TicketType = t.TicketType?? Enumerations.TicketType.Unknown.ToString(),
                        Status = t.Void
                            ? Enumerations.TicketStatus.Voided.ToString()
                            : t.Complete
                                ? Enumerations.TicketStatus.Finished.ToString()
                                : t.Pending
                                    ? Enumerations.TicketStatus.Pending.ToString()
                                    : Enumerations.TicketStatus.Incomplete.ToString(),
                        InvoiceDate = t.TicketDate,
                        LocationId = t.LocationId,
                        CustomerName = t.GrowerName??"",
                        PO = t.Po ?? "",
                        BOL = t.Bol ?? "",
                        TruckId = t.TruckId ?? "",
                        Comments = t.Comments ?? "",
                        Location = _db.Locations.Where(x => x.Id == t.LocationId)
                                           .Select(x => x.Description)
                                           .FirstOrDefault() ?? "",

                        // existing Varieties projection (fallback to CustomName when Item missing)
                        Varieties = t.SeedTicketVarieties.Select(v => new VarietyDescriptionDTO
                        {
                            Id = v.VarietyId,
                            Lot = v.Lot ?? string.Empty,
                            Description = _db.Items.Where(i => i.Id == v.VarietyId)
                                .Select(i => i.Description)
                                .FirstOrDefault() ?? v.CustomName
                        }).ToList()
                    },

                    // Are ALL weights complete (no nulls)?
                    AllDone = !_db.SeedTicketWeights.Any(w =>
                        w.SeedTicketUid == t.Uid &&
                        (w.StartingWeight == null || w.EndingWeight == null)),

                    // Sum of ABS(starting - ending) for complete rows; NULL -> 0
                    SumLbs = _db.SeedTicketWeights
                        .Where(w => w.SeedTicketUid == t.Uid &&
                                    w.StartingWeight != null && w.EndingWeight != null)
                        .Select(w => (double?)Math.Abs(
                            (double)w.StartingWeight - (double)w.EndingWeight))
                        .Sum() ?? 0d
                })
                .ToListAsync(ct);

            // Finish formatting on the client (EF can’t translate "#,##0")
            foreach (var r in rows)
                r.Ticket.StrTotallbs = r.AllDone ? $"{r.SumLbs:#,##0} lbs" : "Not Finished";
          
            return rows.Select(r => r.Ticket); 
        }




        // GET api/<SeedTicketController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SeedTicketController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SeedTicketController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SeedTicketController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
