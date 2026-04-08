namespace GrainManagement.Services
{
    public class AppReadiness
    {
        private volatile bool _isReady;
        public bool IsReady => _isReady;
        public void MarkReady() => _isReady = true;
    }
}
