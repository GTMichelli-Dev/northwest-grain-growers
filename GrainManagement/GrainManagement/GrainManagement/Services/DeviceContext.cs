#nullable enable

namespace GrainManagement.Services
{


    public class DeviceContext : IDeviceContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeviceContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? DeviceId =>
            _httpContextAccessor.HttpContext?.Items["DeviceId"]?.ToString();
    }
}