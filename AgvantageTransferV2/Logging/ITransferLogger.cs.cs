using System.Threading.Tasks;

namespace Agvantage_TransferV2.Logging;

public interface ITransferLogger
{
    Task InfoAsync(string message,string eventDescription);
    Task WarnAsync(string message, string eventDescription);
    Task ErrorAsync(string message, string eventDescription);
    Task ErrorAsync(string message, Exception ex, string eventDescription);
}
