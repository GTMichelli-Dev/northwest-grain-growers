using GrainManagement.DTO;

namespace GrainManagement.Services
{
    public class JsonLog: IJsonLog
    {
        public Task LogInfo(List<LogDTO> messages, string message, string eventDescription)
        {
            messages.Add(new LogDTO
            {
                MessageType = "Info",
                Message = message,
                EventDescription = eventDescription
            });
            return Task.CompletedTask;
        }

        public Task LogWarning(List<LogDTO> messages, string message, string eventDescription)
        {
            messages.Add(new LogDTO
            {
                MessageType = "Warning",
                Message = message,
                EventDescription = eventDescription
            });
            return Task.CompletedTask;
        }

        
        public Task LogError(List<LogDTO> messages, Exception ex, string eventDescription)
        {
            messages.Add(new LogDTO
            {
                MessageType = "Error",
                Message = UsefulMessage( ex),
                EventDescription = eventDescription
            });
            return Task.CompletedTask;
        }

        public static string UsefulMessage(Exception ex) =>
           ex.Message.IndexOf("see the inner exception", StringComparison.OrdinalIgnoreCase) >= 0
               ? (ex.InnerException?.Message ?? ex.GetBaseException().Message ?? ex.Message)
               : ex.Message;

    }
}
