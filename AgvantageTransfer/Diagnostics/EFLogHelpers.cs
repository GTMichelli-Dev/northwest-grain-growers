// Diagnostics/EFLogHelpers.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Agvantage_Transfer.Logging; // for ITransferLogger

namespace Agvantage_Transfer.Diagnostics
{
    public static class EFLogHelpers
    {
        /// <summary>
        /// Logs details from a DbUpdateException (entity names, states, current values) using your DI logger.
        /// </summary>
        public static async Task LogDbUpdateExceptionAsync(
            ITransferLogger log, string message, DbUpdateException ex)
        {
            try
            {
                var entries = ex.Entries?.Select(e =>
                {
                    var values = new Dictionary<string, string>();
                    if (e.CurrentValues != null)
                    {
                        foreach (var p in e.CurrentValues.Properties)
                        {
                            var v = e.CurrentValues[p];
                            values[p.Name] = v?.ToString() ?? "<null>";
                        }
                    }
                    return new
                    {
                        Entity = e.Entity?.GetType().Name ?? "<unknown>",
                        State = e.State.ToString(),
                        Values = values
                    };
                }).ToList();

                await log.ErrorAsync($"{message}: {ex.Message}","System");

                if (entries != null && entries.Count > 0)
                {
                    foreach (var entry in entries)
                    {
                        var kvPairs = (entry.Values ?? new Dictionary<string, string>())
                            .Select(kv => $"{kv.Key}={kv.Value}");
                        await log.ErrorAsync(
                            $"Entity={entry.Entity}, State={entry.State}, Values={string.Join(", ", kvPairs)}", "System");
                    }
                }
            }
            catch
            {
                // best-effort: never throw from logger
                await log.ErrorAsync($"{message} (additional entry logging failed): {ex.Message}", "System");
            }
        }

        /// <summary>
        /// Same thing, but for plain Microsoft ILogger (if you ever need it without ITransferLogger).
        /// </summary>
        public static void LogDbUpdateException(
            ILogger logger, string message, DbUpdateException ex)
        {
            try
            {
                logger.LogError(ex, "{Message}: {Error}", message, ex.Message);

                var entries = ex.Entries?.Select(e =>
                {
                    var values = new Dictionary<string, string>();
                    if (e.CurrentValues != null)
                    {
                        foreach (var p in e.CurrentValues.Properties)
                        {
                            var v = e.CurrentValues[p];
                            values[p.Name] = v?.ToString() ?? "<null>";
                        }
                    }
                    return new
                    {
                        Entity = e.Entity?.GetType().Name ?? "<unknown>",
                        State = e.State.ToString(),
                        Values = values
                    };
                }).ToList();

                if (entries != null && entries.Count > 0)
                {
                    foreach (var entry in entries)
                    {
                        var kvPairs = (entry.Values ?? new Dictionary<string, string>())
                            .Select(kv => $"{kv.Key}={kv.Value}");
                        logger.LogError("Entity={Entity}, State={State}, Values={Values}",
                            entry.Entity, entry.State, string.Join(", ", kvPairs));
                    }
                }
            }
            catch
            {
                logger.LogError("{Message} (additional entry logging failed): {Error}", message, ex.Message);
            }
        }
    }
}
