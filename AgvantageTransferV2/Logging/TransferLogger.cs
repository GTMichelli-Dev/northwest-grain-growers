using Agvantage_Transfer.AtModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Agvantage_Transfer.Logging;

public sealed class TransferLogger : ITransferLogger
{
    private readonly ILogger<TransferLogger> _logger;
  
    private readonly IDbContextFactory<AtDbContext> _factory;

    public TransferLogger(ILogger<TransferLogger> logger, IDbContextFactory<AtDbContext> factory)
          => (_logger, _factory) = (logger, factory);


    private static string FilterPasswords(string message)
    {
        const string passwordFilter = "/PASSWORD=";
        if (string.IsNullOrWhiteSpace(message)) return message;
        if (!message.Contains(passwordFilter, StringComparison.OrdinalIgnoreCase)) return message;

        var idx = message.IndexOf(passwordFilter, StringComparison.OrdinalIgnoreCase);
        return message[..idx] + passwordFilter + "**********";
    }

    public static string UsefulMessage(Exception ex) =>
    ex.Message.IndexOf("see the inner exception", StringComparison.OrdinalIgnoreCase) >= 0
        ? (ex.InnerException?.Message ?? ex.GetBaseException().Message ?? ex.Message)
        : ex.Message;


    private async Task WriteAsync(string message,  bool isError ,string eventDescription)
    {
        
        message = FilterPasswords(message);
        if (isError) _logger.LogWarning("{Message}", message);
        else _logger.LogInformation("{Message}", message);

        await using var db = await _factory.CreateDbContextAsync();
        db.AgvantageTransferLogs.Add(new AgvantageTransferLog
        {
            Error = isError,
            Message = message,
            TaskTime = DateTime.Now,
            Event = eventDescription

        });
        await db.SaveChangesAsync();
    }

    public Task InfoAsync(string message, string eventDescription) => WriteAsync(message, isError: false,eventDescription );
    public Task WarnAsync(string message, string eventDescription)   => WriteAsync(message, isError: true, eventDescription);
    public Task ErrorAsync(string message, string eventDescription) => WriteAsync(message, isError: true, eventDescription);

    public async Task ErrorAsync(string message, Exception ex, string eventDescription)
    {
        _logger.LogError(ex, "{Message}", message);
        await WriteAsync($"{message} :: {ex.Message} ::{eventDescription}", isError: true, eventDescription);
    }
}
