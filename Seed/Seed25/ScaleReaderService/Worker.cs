using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using ScaleReaderService.Services;

namespace ScaleReaderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ScalePoller _poller;

        public Worker(ScalePoller poller, ILogger<Worker> logger)
        {
            _poller = poller;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ScaleReader Worker starting pollers.");
            await _poller.RunAsync(stoppingToken);
            _logger.LogInformation("ScaleReader Worker stopping.");
        }
    }
}
