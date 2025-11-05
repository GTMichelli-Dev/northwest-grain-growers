using System.Collections.Generic;
using System.Threading.Tasks;
using Agvantage_Transfer.DTOModels;

namespace Agvantage_Transfer.Sync;

public interface ICarrierSyncService
{
    Task UpsertAsync(IEnumerable<AgvantageCarrierDTO> dtoList);
}
