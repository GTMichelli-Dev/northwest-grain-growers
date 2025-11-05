using Agvantage_Transfer.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agvantage_Transfer.Sync
{
    public interface ICropSyncService
    {
        Task UpsertAsync(IEnumerable<AgvantageCropDTO> dtoList);
    }
}
