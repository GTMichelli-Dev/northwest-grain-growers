using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace ScaleReaderService.Models;

public class ScaleBrandDefinition
{
    [JsonPropertyName("brand")]
    public string Brand { get; set; } = "";

    [JsonPropertyName("protocol")]
    public string Protocol { get; set; } = "";

    [JsonPropertyName("connectionType")]
    public string ConnectionType { get; set; } = "TCP";

    [JsonPropertyName("defaultPort")]
    public int DefaultPort { get; set; } = 10001;

    [JsonPropertyName("requestCommand")]
    public string RequestCommand { get; set; } = "W\r\n";

    [JsonPropertyName("encoding")]
    public string Encoding { get; set; } = "ascii";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = "";

    /// <summary>
    /// Loads brand definitions from a remote URL first, falling back to local file.
    /// </summary>
    public static async Task<List<ScaleBrandDefinition>> LoadBrandsAsync(
        string? remoteUrl, string localPath, string? token, ILogger logger)
    {
        // Try local first (fast)
        List<ScaleBrandDefinition>? brands = null;
        if (File.Exists(localPath))
        {
            try
            {
                var json = await File.ReadAllTextAsync(localPath);
                brands = JsonSerializer.Deserialize<List<ScaleBrandDefinition>>(json);
                logger.LogInformation("Loaded {Count} scale brands from local: {Path}", brands?.Count ?? 0, localPath);
            }
            catch (Exception ex)
            {
                logger.LogWarning("Failed to load local scale brands: {Msg}", ex.Message);
            }
        }

        // Try remote to get latest (if configured)
        if (!string.IsNullOrWhiteSpace(remoteUrl))
        {
            try
            {
                using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                if (!string.IsNullOrWhiteSpace(token))
                    http.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var json = await http.GetStringAsync(remoteUrl);
                var remote = JsonSerializer.Deserialize<List<ScaleBrandDefinition>>(json);
                if (remote != null && remote.Count > 0)
                {
                    brands = remote;
                    logger.LogInformation("Loaded {Count} scale brands from remote: {Url}", brands.Count, remoteUrl);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("Could not load remote scale brands from {Url}. Using local. Error: {Msg}", remoteUrl, ex.Message);
            }
        }

        return brands ?? new List<ScaleBrandDefinition>();
    }
}
