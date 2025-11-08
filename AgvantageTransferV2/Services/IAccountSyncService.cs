using System.Collections.Generic;
using System.Threading.Tasks;
using Agvantage_TransferV2.GmModels;

namespace Agvantage_TransferV2.Sync
{
    public interface IAccountSyncService
    {
        Task UpsertAsync(IEnumerable<Account> accountList);
    }
}
