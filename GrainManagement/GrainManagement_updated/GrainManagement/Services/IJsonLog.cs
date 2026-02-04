using GrainManagement.Models.DTO;

namespace GrainManagement.Services
{
    public interface IJsonLog
    {
        Task LogInfo(List<LogDTO> messages,string message, string eventDescription);
        Task LogWarning(List<LogDTO> messages, string message, string eventDescription);
        Task LogError(List<LogDTO> messages, Exception ex, string eventDescription);
    }
}
