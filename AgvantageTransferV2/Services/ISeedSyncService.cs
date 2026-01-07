using Agvantage_Transfer.DTOModels;
using Agvantage_Transfer.SeedModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agvantage_Transfer.Sync
{
    public interface ISeedSyncService
    {
        Task UpsertSeedItemsAsync(IEnumerable<AgvantageItemDTO> dtoList);
        Task UpsertSeedDepartmentsAsync(IEnumerable<AgvantageSeedDepartmentDTO> dtoList);
        Task UpsertSeedItemLocationAsync(IEnumerable<AgvantageItemLocationDTO> dtoList);
    }
}
