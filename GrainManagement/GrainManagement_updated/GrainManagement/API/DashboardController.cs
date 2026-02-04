using GrainManagement.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading;
using static System.Collections.Specialized.BitVector32;

namespace GrainManagement.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private static int _callCount = 0;

        // GET /api/dashboard/location-totals?start=2025-12-17&end=2025-12-17
        [HttpGet("location-totals")]
        public ActionResult<IEnumerable<LocationTotalsDTO>> GetLocationTotals([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            var callNo = Interlocked.Increment(ref _callCount);

            // Seed changes every call so the fake data changes
            var rand = new Random(unchecked(Environment.TickCount * 31 + callNo * 997));

            // You can read start/end later for real filtering; for now just accept them.
            var locations = new (int id, string name)[]
            {
                      (2,"Clyde"),
                (3,"Dixie"),
                (4,"Dry Creek"),
                (5,"Ennis"),
                (6,"Eureka"),
                (9,"Paddock"),
                (11,"Port Kelley"),
                (12,"Reser"),
                (13,"Rulo"),
                (14,"Sapolil"),
                (15,"Sheffler"),
                (16,"Smith Springs"),
                (20,"Valley Grove"),
                (22,"Wallula"),
                (24,"Seed Plant"),
                (25,"Dayton Seed Plant"),
                (26,"Lancaster Seed Plant"),
                (27,"Garfield"),
                (31,"Bolles"),
                (32,"Coppei"),
                (33,"Harsha"),
                (34,"Jensen Corner"),
                (35,"Mckay"),
                (36,"Menoken"),
                (37,"Prescott"),
                (38,"Waitsburg"),
                (35,"Mckay"),
                (36,"Menoken"),
                (37,"Prescott"),
                (38,"Waitsburg"),
                (46,"Lower Whetstone"),
                (47,"Lyons Ferry"),
                (48,"Relief"),
                (49,"TURNER"),
                (50,"Athena"),
            };

            var result = new List<LocationTotalsDTO>();
            foreach (var (id, name) in locations)
            {
                // Base numbers + random + callNo drift
                var intake = (long)(200_000 + rand.Next(0, 150_000) + callNo * rand.Next(50, 500));
                var outbound = (long)(140_000 + rand.Next(0, 120_000) + callNo * rand.Next(30, 300));
                var xferTo = (long)(30_000 + rand.Next(0, 60_000));
                var xferFrom = (long)(25_000 + rand.Next(0, 55_000));

                result.Add(new LocationTotalsDTO
                {
                    LocationId = id,
                    LocationName = name,
                    IntakeLbs = intake,
                    OutboundLbs = outbound,
                    TransferedToLbs = xferTo,
                    TransferedFromLbs = xferFrom
                });
            }

            return Ok(result);
        }
    }
}
