using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "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"; // 🔐 Replace with your actual OpenRouter API key

    public ChatbotController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ChatRequest request)
    {
        var payload = new
        {
            model = "mistralai/mistral-7b-instruct", // 🧠 You can change this to another model if needed
            messages = new[]
            {
                new { role = "system", content = "You are a helpful AI health assistant. Do not diagnose. Give general advice and recommend seeing a doctor when needed." },
                new { role = "user", content = request.Message }
            }
        };

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://openrouter.ai/api/v1/chat/completions");
        httpRequest.Headers.Add("Authorization", $"Bearer {_apiKey}");
        httpRequest.Headers.Add("HTTP-Referer", "http://localhost"); // ✅ Required by OpenRouter
        httpRequest.Headers.Add("X-Title", "AIhealthchatbot");       // Optional, useful for tracking

        httpRequest.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(httpRequest);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return BadRequest(new { error = "Failed to get a valid response from OpenRouter.", details = error });
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(jsonResponse);

        var reply = doc.RootElement
                       .GetProperty("choices")[0]
                       .GetProperty("message")
                       .GetProperty("content")
                       .GetString();

        return Ok(new { reply });
    }

    public class ChatRequest
    {
        public string Message { get; set; }
    }
}
