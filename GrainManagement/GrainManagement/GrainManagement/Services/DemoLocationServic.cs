
using GrainManagement.Dtos;

namespace GrainManagement.Services
{

    public class DemoLocationService : ILocationService
    {
        public Task<List<LocationDto>> GetAllAsync()
        {
            var data = new List<LocationDto>
        {
            CreateLocation(2, 5, "Clyde", true, true, true),
            CreateLocation(3, 5, "Dixie", true, true, true),
            CreateLocation(4, 5, "Dry Creek", true, true, true),
            CreateLocation(5, 5, "Ennis", false, true, true),
            CreateLocation(6, 5, "Eureka", true, true, true),
            CreateLocation(7, 5, "Baker Langdon", true, true, true),
            CreateLocation(8, 5, "Miller", true, true, true),
            CreateLocation(9, 5, "Paddock", true, true, true),
            CreateLocation(11, 4, "Port Kelley", true, true, true),
            CreateLocation(12, 5, "Reser", true, true, true),
            CreateLocation(13, 5, "Rulo", true, true, true),
            CreateLocation(14, 5, "Sapolil", true, false, true),
            CreateLocation(15, 5, "Sheffler", true, false, true),
            CreateLocation(16, 5, "Smith Springs", true, true, true),
            CreateLocation(17, 4, "Spofford", true, true, true),
            CreateLocation(18, 5, "Spring Valley", true, true, true),
            CreateLocation(19, 5, "Tracy", true, true, true),
            CreateLocation(20, 5, "Valley Grove", true, true, true),
            CreateLocation(21, 5, "Walla Walla", false, true, true),
            CreateLocation(22, 4, "Wallula", true, true, true),
            CreateLocation(24, 5, "Seed Plant", true, true, true),
            CreateLocation(25, 1, "Dayton Seed Plant", true, true, true),
            CreateLocation(26, 3, "Lancaster Seed Plant", true, true, false),
            CreateLocation(27, 3, "Garfield", true, true, true),
            CreateLocation(31, 5, "Bolles", true, true, true),
            CreateLocation(32, 5, "Coppei", true, true, true),
            CreateLocation(33, 5, "Harsha", true, true, true),
            CreateLocation(34, 5, "Jensen Corner", true, true, true),
            CreateLocation(35, 5, "Mckay", true, true, true),
            CreateLocation(36, 5, "Menoken", true, true, true),
            CreateLocation(37, 5, "Prescott", true, true, true),
            CreateLocation(38, 5, "Waitsburg", true, true, true)
        };

            return Task.FromResult(data);
        }

        private static LocationDto CreateLocation(
            int locationId,
            int districtId,
            string name,
            bool isActive,
            bool useForSeed,
            bool useForWarehouse)
        {
            return new LocationDto
            {
                LocationId = locationId,
                DistrictId = districtId,
                District = $"District {districtId}",
                Code = name.Replace(" ", "").ToUpperInvariant(),
                Name = name,
                IsActive = isActive,
                UseForSeed = useForSeed,
                UseForWarehouse = useForWarehouse
            };
        }


        private static DistrictDto CreateDistrict(int districtId, string name)
        {
            return new DistrictDto
            {
                DistrictId = districtId,
                Name = name
            };
        }

        public async Task<List<DistrictDto>> GetLocationDistricts()
        {
            var data = new List<DistrictDto>
            {
                CreateDistrict(1, "East"),
                CreateDistrict(2, "West"),
                CreateDistrict(3, "North"),
                CreateDistrict(4, "South"),
                CreateDistrict(5, "Central")
            };

            return await Task.FromResult(data);
        }
    }

    }
