using System.Text;
using Newtonsoft.Json;

namespace ShoppingLearn.Services.Chatbot
{
    /// <summary>
    /// Service tích hợp Google Gemini API để xử lý chat
    /// </summary>
    public class GeminiService : IGeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private const string API_ENDPOINT = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

        public GeminiService(IConfiguration configuration)
        {
            _apiKey = configuration["Gemini:ApiKey"];
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Gửi message đến Gemini và nhận response
        /// </summary>
        public async Task<string> SendMessageAsync(string userMessage, string systemPrompt = null, List<string> context = null)
        {
            try
            {
                // Xây dựng prompt hoàn chỉnh
                var fullPrompt = BuildPrompt(userMessage, systemPrompt, context);

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = fullPrompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.7,
                        maxOutputTokens = 1000,
                        topP = 0.95,
                        topK = 40
                    }
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var url = $"{API_ENDPOINT}?key={_apiKey}";
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Gemini API error: {response.StatusCode} - {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<GeminiResponse>(responseContent);

                return result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? "Xin lỗi, tôi không thể trả lời câu hỏi này.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GeminiService: {ex.Message}");
                return "Xin lỗi, đã có lỗi xảy ra khi xử lý yêu cầu của bạn. Vui lòng thử lại sau.";
            }
        }

        /// <summary>
        /// Xây dựng prompt đầy đủ từ system prompt, context và user message
        /// </summary>
        private string BuildPrompt(string userMessage, string systemPrompt, List<string> context)
        {
            var promptBuilder = new StringBuilder();

            // Thêm system prompt
            if (!string.IsNullOrEmpty(systemPrompt))
            {
                promptBuilder.AppendLine(systemPrompt);
                promptBuilder.AppendLine();
            }

            // Thêm context từ RAG
            if (context != null && context.Any())
            {
                promptBuilder.AppendLine("Thông tin tham khảo:");
                foreach (var ctx in context)
                {
                    promptBuilder.AppendLine($"- {ctx}");
                }
                promptBuilder.AppendLine();
            }

            // Thêm câu hỏi của user
            promptBuilder.AppendLine($"Câu hỏi: {userMessage}");

            return promptBuilder.ToString();
        }

        // Response models cho Gemini API
        private class GeminiResponse
        {
            public List<Candidate> Candidates { get; set; }
        }

        private class Candidate
        {
            public ContentPart Content { get; set; }
        }

        private class ContentPart
        {
            public List<TextPart> Parts { get; set; }
        }

        private class TextPart
        {
            public string Text { get; set; }
        }
    }
}
