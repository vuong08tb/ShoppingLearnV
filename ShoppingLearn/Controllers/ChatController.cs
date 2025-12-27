using Microsoft.AspNetCore.Mvc;
using ShoppingLearn.Models.Chatbot;
using ShoppingLearn.Services.Chatbot;

namespace ShoppingLearn.Controllers
{
    /// <summary>
    /// API Controller xử lý chatbot requests
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatbotService chatbotService, ILogger<ChatController> logger)
        {
            _chatbotService = chatbotService;
            _logger = logger;
        }

        /// <summary>
        /// POST /api/chat/message - Gửi message cho chatbot
        /// </summary>
        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                // Validate request
                if (request == null || string.IsNullOrWhiteSpace(request.Message))
                {
                    return BadRequest(new ChatResponse
                    {
                        Success = false,
                        ErrorMessage = "Tin nhắn không hợp lệ"
                    });
                }

                // Sanitize input
                request.Message = SanitizeInput(request.Message);

                // Check rate limit
                var sessionId = request.SessionId ?? GetClientIdentifier();
                request.SessionId = sessionId;

                if (!_chatbotService.CheckRateLimit(sessionId))
                {
                    return StatusCode(429, new ChatResponse
                    {
                        Success = false,
                        ErrorMessage = "Bạn đang gửi tin nhắn quá nhanh. Vui lòng đợi ít phút."
                    });
                }

                // Process message
                var response = await _chatbotService.ProcessMessageAsync(request);

                if (!response.Success)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in SendMessage: {ex.Message}");
                return StatusCode(500, new ChatResponse
                {
                    Success = false,
                    ErrorMessage = "Lỗi server",
                    Reply = "Xin lỗi, đã có lỗi xảy ra. Vui lòng thử lại sau."
                });
            }
        }

        /// <summary>
        /// GET /api/chat/history/{sessionId} - Lấy lịch sử chat
        /// </summary>
        [HttpGet("history/{sessionId}")]
        public IActionResult GetHistory(string sessionId)
        {
            try
            {
                var history = _chatbotService.GetChatHistory(sessionId);
                return Ok(new
                {
                    success = true,
                    sessionId = sessionId,
                    messages = history
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting history: {ex.Message}");
                return StatusCode(500, new { success = false, error = "Lỗi server" });
            }
        }

        /// <summary>
        /// DELETE /api/chat/history/{sessionId} - Xóa lịch sử chat
        /// </summary>
        [HttpDelete("history/{sessionId}")]
        public IActionResult ClearHistory(string sessionId)
        {
            try
            {
                _chatbotService.ClearChatHistory(sessionId);
                return Ok(new { success = true, message = "Đã xóa lịch sử chat" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error clearing history: {ex.Message}");
                return StatusCode(500, new { success = false, error = "Lỗi server" });
            }
        }

        /// <summary>
        /// GET /api/chat/health - Health check
        /// </summary>
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.Now,
                service = "Chatbot API"
            });
        }

        /// <summary>
        /// Sanitize input để tránh XSS
        /// </summary>
        private string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Remove potentially dangerous characters
            input = input.Replace("<", "&lt;")
                        .Replace(">", "&gt;")
                        .Replace("\"", "&quot;")
                        .Replace("'", "&#39;");

            // Trim và giới hạn length
            return input.Trim().Substring(0, Math.Min(input.Length, 500));
        }

        /// <summary>
        /// Lấy identifier của client (IP + UserAgent hash)
        /// </summary>
        private string GetClientIdentifier()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
            var identifier = $"{ip}_{userAgent}".GetHashCode().ToString();
            return identifier;
        }
    }
}
