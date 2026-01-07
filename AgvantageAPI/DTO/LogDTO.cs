using System.Security.Permissions;

namespace AgvantageAPI.DTO
{
    public class LogDTO
    {
        public string EventDescription { get; set; }=string.Empty;
        public string  MessageType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
