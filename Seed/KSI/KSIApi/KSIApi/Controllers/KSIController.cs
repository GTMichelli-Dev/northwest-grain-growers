using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace KSIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KSIController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly KSIOptions _ksiOptions;

        public KSIController(HttpClient httpClient, IOptions<KSIOptions> ksiOptions)
        {
            _httpClient = httpClient;
            _ksiOptions = ksiOptions.Value;
        }

     
        // Get a Token from KSI
        [HttpPost("GetBearerToken")]
        public async Task<IActionResult> GetBearerToken()
        {
            var credentials = new AccessCredentials
            {
                access_key = _ksiOptions.AccessKey,
                access_secret = _ksiOptions.AccessSecret
            };
            var jsonCredentials = JsonConvert.SerializeObject(credentials);
           
        
           
            var requestContent = new StringContent(jsonCredentials, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.agconnex.com/api/v2/access_tokens", requestContent);




            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return Ok(token);
            }

            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }

        
    }

    public class AccessCredentials
    {
        [JsonPropertyName("access_key")]
        public string access_key { get; set; } = string.Empty;

        [JsonPropertyName("access_secret")]
        public string access_secret { get; set; } = string.Empty;
    }
}
