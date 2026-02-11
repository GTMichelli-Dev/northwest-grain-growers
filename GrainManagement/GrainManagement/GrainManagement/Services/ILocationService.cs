using GrainManagement.Dtos;
namespace GrainManagement.Services
{
    public interface ILocationService
    {
        Task<List<LocationDto>> GetAllAsync();

        Task<List<DistrictDto>> GetLocationDistricts();
    }
}
