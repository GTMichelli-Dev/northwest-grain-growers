using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

class Program
{
    public class AccessCredentials
    {
        [JsonPropertyName("access_key")]
        public string AccessKey { get; set; } = "F71F8BBC8CB190E0F28F8F889D53739C";

        [JsonPropertyName("access_secret")]
        public string AccessSecret { get; set; } = "AG5q7ghqmfxhu2c8avvnnsgg6upsffnsqqvz8dstpts60wi5s5m67tpp7m6y5kgo";
    }

    static async Task Main(string[] args)
    {
        var response = await GetBearerToken();
        Console.WriteLine(response);
        Console.ReadLine();
    }

    public static async Task<string> GetBearerToken()
    {
        HttpClient _httpClient = new HttpClient();
        var credentials = new AccessCredentials();
        var jsonCredentials = JsonSerializer.Serialize(credentials);
        var requestContent = new StringContent(jsonCredentials, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://api.agconnex.com/api/v2/access_tokens", requestContent);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            return token ?? "";
        }

        return $"\r\nERROR - {(int)response.StatusCode}\r\n{response.ReasonPhrase}";
    }
}
