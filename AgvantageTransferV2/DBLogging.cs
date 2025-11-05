using Agvantage_Transfer.AtModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Agvantage_Transfer
{
    internal class DBLogging : IDBLogging
    {
        private readonly ILogger<AgvantageTransfer> _logger;
        private readonly AtDbContext _atDbContext;

        public DBLogging(ILogger<AgvantageTransfer> logger, AtDbContext dbContext)
        {
            _logger = logger;
            _atDbContext = dbContext;
        }

        public async Task ClearLog()
        {
            var allLogs = await _atDbContext.AgvantageTransferLogs.ToListAsync();
            _atDbContext.AgvantageTransferLogs.RemoveRange(allLogs);
            await _atDbContext.SaveChangesAsync();
        }

        public async Task LogAsync(string message, bool isError = false)
        {
            try
            {
                var logEntry = new AgvantageTransferLog
                {
                    Message = message,
                    Error = isError,
                    TaskTime = DateTime.Now
                };
                await _atDbContext.AgvantageTransferLogs.AddAsync(logEntry);
                await _atDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to log message to database: {Message}", message);
            }
        }
    }

    public interface IDBLogging
    {
        Task ClearLog();
        Task LogAsync(string message, bool isError = false);
    }
}