using GrainManagement.Models;
using Microsoft.EntityFrameworkCore;
using GrainManagement.Dtos;

namespace GrainManagement.Services
{
    public class sqlLocationService : ILocationService
    {
        private readonly dbContext _ctx;

        public sqlLocationService(dbContext ctx)
        {
            _ctx = ctx;
        }

        public async  Task<List<LocationDto>> GetAllAsync()
        {

            var data = await _ctx.Locations
          .AsNoTracking()
          .Include(l => l.District)
          .Where(l => l.UseForWarehouse == true)
          .Where(l => l.IsActive == true)
          .OrderBy(l => l.Name)
          .Select(l => new
          {
              LocationId = l.LocationId,
              Name = l.Name,
              District = l.District.Name
          })
          .ToListAsync();

            return data.Select(l => new LocationDto
            {
                LocationId = l.LocationId,
                Name = l.Name,
                District = l.District
            }).ToList();
        }

        public async Task<List<DistrictDto>> GetLocationDistricts()
        {
            var data = await _ctx.LocationDistricts
               .Select(p => new DistrictDto { DistrictId = p.DistrictId, Name = p.Name })
               .OrderBy(p => p.Name)
               .ToListAsync();

            return data;
        }

}
}
