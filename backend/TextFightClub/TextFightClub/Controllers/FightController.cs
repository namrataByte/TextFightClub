using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TextFightClub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FightController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "your_api_key"; // replace with your Gemini API key

        // ✅ Using gemini-1.5-flash (higher quota, faster)
        private readonly string _endpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

        public FightController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("battle")]
        public async Task<IActionResult> Battle([FromBody] FightRequest request)
        {
            var conversation = new List<BotMessage>();
            string lastBot1 = "";
            string lastBot2 = "";

            for (int i = 0; i < 3; i++)
            {
                // Bot1 speaks
                var bot1Prompt = i == 0
                    ? $"You are Bot1. Start the roast battle about {request.Subject}."
                    : $"You are Bot1. Continue roasting after Bot2 said: \"{lastBot2}\"";

                lastBot1 = await GetGeminiResponse(bot1Prompt);
                lastBot1 = string.IsNullOrWhiteSpace(lastBot1) ? "Bot1 has nothing to say..." : lastBot1.Trim();

                conversation.Add(new BotMessage
                {
                    Bot = "Bot1",
                    Text = lastBot1
                });

                // Bot2 responds
                var bot2Prompt = $"You are Bot2. Respond with an even harsher roast back to Bot1. Bot1 just said: \"{lastBot1}\"";
                lastBot2 = await GetGeminiResponse(bot2Prompt);
                lastBot2 = string.IsNullOrWhiteSpace(lastBot2) ? "Bot2 has nothing to say..." : lastBot2.Trim();

                conversation.Add(new BotMessage
                {
                    Bot = "Bot2",
                    Text = lastBot2
                });
            }

            return Ok(new
            {
                subject = request.Subject,
                conversation
            });
        }

        private async Task<string> GetGeminiResponse(string prompt)
        {
            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_endpoint}?key={_apiKey}", content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Gemini API Error: {response.StatusCode}");
                return $"Error: {response.StatusCode}";
            }

            var json = await response.Content.ReadAsStringAsync();
            return ParseGeminiJson(json);
        }

        private string ParseGeminiJson(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("candidates", out var candidates) &&
                    candidates.GetArrayLength() > 0)
                {
                    var candidate = candidates[0];

                    if (candidate.TryGetProperty("content", out var contentEl) &&
                        contentEl.TryGetProperty("parts", out var parts) &&
                        parts.GetArrayLength() > 0)
                    {
                        var text = parts[0].GetProperty("text").GetString();
                        return string.IsNullOrWhiteSpace(text) ? "Bot has nothing to say..." : text.Trim();
                    }
                }

                return "Bot has nothing to say...";
            }
            catch
            {
                return "Failed to parse response";
            }
        }
    }

    public class FightRequest
    {
        public string Subject { get; set; }
    }

    public class BotMessage
    {
        public string Bot { get; set; }
        public string Text { get; set; }
    }
}
