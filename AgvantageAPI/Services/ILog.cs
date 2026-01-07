using AgvantageAPI.DTO;

namespace AgvantageAPI.Services
{
    public interface ILog
    {
        Task LogInfo(List<LogDTO> messages,string message, string eventDescription);
        Task LogWarning(List<LogDTO> messages, string message, string eventDescription);
        Task LogError(List<LogDTO> messages, Exception ex, string eventDescription);
    }
}
