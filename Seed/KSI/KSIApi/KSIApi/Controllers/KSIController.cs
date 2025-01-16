using System.Net.Http;
using System.Text;
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
                AccessKey = _ksiOptions.AccessKey,
                AccessSecret = _ksiOptions.AccessSecret
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");

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
        public string AccessKey { get; set; } = string.Empty;
        public string AccessSecret { get; set; } = string.Empty;
    }
}
