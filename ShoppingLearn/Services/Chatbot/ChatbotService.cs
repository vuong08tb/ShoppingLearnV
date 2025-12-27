using ShoppingLearn.Models.Chatbot;
using System.Collections.Concurrent;

namespace ShoppingLearn.Services.Chatbot
{
    /// <summary>
    /// Service chính điều phối toàn bộ logic chatbot
    /// Kết hợp RAG, SQL Query và Gemini để trả lời user
    /// </summary>
    public class ChatbotService : IChatbotService
    {
        private readonly IGeminiService _geminiService;
        private readonly IChromaService _chromaService;
        private readonly ISqlQueryService _sqlQueryService;
        private readonly ILogger<ChatbotService> _logger;

        // Lưu lịch sử chat theo session (in-memory)
        private static readonly ConcurrentDictionary<string, List<ChatMessage>> _chatHistory
            = new ConcurrentDictionary<string, List<ChatMessage>>();

        // System prompt cho chatbot
        private const string SYSTEM_PROMPT = @"Bạn là trợ lý ảo thông minh chuyên về thời trang tại ShoppingLearn - cửa hàng thời trang trực tuyến.

NHIỆM VỤ CỦA BẠN:
- Tư vấn sản phẩm thời trang cho khách hàng
- Trả lời các câu hỏi về giá cả, tồn kho, chính sách đổi trả
- Giúp khách hàng tìm kiếm sản phẩm phù hợp
- Giải đáp thắc mắc về quy trình mua hàng, giao hàng

QUY TẮC:
1. CHỈ trả lời về sản phẩm thời trang, mua sắm và dịch vụ của shop
2. KHÔNG trả lời các câu hỏi ngoài phạm vi (chính trị, y tế, pháp luật...)
3. Luôn thân thiện, chuyên nghiệp, lịch sự
4. Trả lời ngắn gọn, súc tích, dễ hiểu
5. Nếu không biết câu trả lời, hãy thành thật nói 'Tôi không có thông tin này, vui lòng liên hệ hotline...'
6. Sử dụng thông tin được cung cấp để trả lời chính xác

PHONG CÁCH:
- Thân thiện, gần gũi
- Dùng emoji phù hợp (nhưng không lạm dụng)
- Gọi khách hàng là 'bạn' hoặc 'anh/chị'
- Kết thúc bằng câu hỏi mở để tương tác";

        public ChatbotService(
            IGeminiService geminiService,
            IChromaService chromaService,
            ISqlQueryService sqlQueryService,
            ILogger<ChatbotService> logger)
        {
            _geminiService = geminiService;
            _chromaService = chromaService;
            _sqlQueryService = sqlQueryService;
            _logger = logger;
        }

        /// <summary>
        /// Xử lý message từ user và trả về response
        /// </summary>
        public async Task<ChatResponse> ProcessMessageAsync(ChatRequest request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.Message))
                {
                    return new ChatResponse
                    {
                        Success = false,
                        ErrorMessage = "Tin nhắn không được để trống"
                    };
                }

                if (request.Message.Length > 500)
                {
                    return new ChatResponse
                    {
                        Success = false,
                        ErrorMessage = "Tin nhắn quá dài (tối đa 500 ký tự)"
                    };
                }

                // Tạo session ID nếu chưa có
                var sessionId = request.SessionId ?? Guid.NewGuid().ToString();

                // Lưu message của user vào history
                AddToHistory(sessionId, "user", request.Message);

                // Bước 1: Detect intent - có cần query database không?
                var intent = _sqlQueryService.DetectIntent(request.Message);

                // Bước 2: Lấy context từ database nếu cần
                var databaseContext = new List<string>();
                if (intent.NeedDatabaseQuery)
                {
                    var products = await _sqlQueryService.QueryProductsAsync(intent);
                    if (products.Any())
                    {
                        var productInfo = _sqlQueryService.FormatProductResults(products);
                        databaseContext.Add(productInfo);
                    }
                }

                // Bước 3: Tìm kiếm trong knowledge base (RAG)
                var knowledgeContext = await _chromaService.SearchAsync(request.Message, 3);

                // Bước 4: Kết hợp tất cả context
                var allContext = new List<string>();
                allContext.AddRange(databaseContext);
                allContext.AddRange(knowledgeContext);

                // Bước 5: Gửi đến Gemini để generate response
                var reply = await _geminiService.SendMessageAsync(
                    request.Message,
                    SYSTEM_PROMPT,
                    allContext
                );

                // Lưu response vào history
                AddToHistory(sessionId, "assistant", reply);

                return new ChatResponse
                {
                    Reply = reply,
                    Sources = allContext,
                    Success = true,
                    SessionId = sessionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
                return new ChatResponse
                {
                    Success = false,
                    ErrorMessage = "Đã có lỗi xảy ra, vui lòng thử lại sau",
                    Reply = "Xin lỗi, hệ thống đang gặp sự cố. Vui lòng thử lại sau ít phút."
                };
            }
        }

        /// <summary>
        /// Lấy lịch sử chat của một session
        /// </summary>
        public List<ChatMessage> GetChatHistory(string sessionId)
        {
            return _chatHistory.TryGetValue(sessionId, out var history)
                ? history
                : new List<ChatMessage>();
        }

        /// <summary>
        /// Xóa lịch sử chat của một session
        /// </summary>
        public void ClearChatHistory(string sessionId)
        {
            _chatHistory.TryRemove(sessionId, out _);
        }

        /// <summary>
        /// Thêm message vào lịch sử chat
        /// </summary>
        private void AddToHistory(string sessionId, string role, string content)
        {
            var message = new ChatMessage
            {
                Role = role,
                Content = content,
                Timestamp = DateTime.Now
            };

            _chatHistory.AddOrUpdate(
                sessionId,
                new List<ChatMessage> { message },
                (key, existing) =>
                {
                    existing.Add(message);
                    // Giữ tối đa 20 messages
                    if (existing.Count > 20)
                    {
                        existing.RemoveAt(0);
                    }
                    return existing;
                }
            );
        }

        /// <summary>
        /// Kiểm tra rate limit (20 messages/phút/session)
        /// </summary>
        public bool CheckRateLimit(string sessionId)
        {
            var history = GetChatHistory(sessionId);
            var oneMinuteAgo = DateTime.Now.AddMinutes(-1);
            var recentMessages = history.Count(m => m.Timestamp > oneMinuteAgo && m.Role == "user");

            return recentMessages < 20;
        }
    }
}
