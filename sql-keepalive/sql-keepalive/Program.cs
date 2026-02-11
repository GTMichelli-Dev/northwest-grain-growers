using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var cs = config.GetConnectionString("AzureSql")
                 ?? throw new InvalidOperationException("Missing ConnectionStrings:AzureSql");

        var query = config["KeepAlive:Query"] ?? "SELECT 1";
        var delayMs = config.GetValue("Retry:DelayMilliseconds", 5000);
        var connectTo = config.GetValue("Retry:ConnectTimeoutSeconds", 10);
        var commandTo = config.GetValue("Retry:CommandTimeoutSeconds", 10);

        var csb = new SqlConnectionStringBuilder(cs) { ConnectTimeout = connectTo };

        var attempt = 0;

        while (true)
        {
            attempt++;

            try
            {
                await using var conn = new SqlConnection(csb.ConnectionString);
                await conn.OpenAsync();

                await using var cmd = conn.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = commandTo;

                await cmd.ExecuteScalarAsync();

                Console.WriteLine($"OK {DateTimeOffset.Now:u} (attempt {attempt})");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"RETRY {DateTimeOffset.Now:u} (attempt {attempt}) -> {ex.GetType().Name}: {ex.Message}");
            }

            // Always wait, whether it succeeded or failed
            await Task.Delay(delayMs);
        }
    }
}
