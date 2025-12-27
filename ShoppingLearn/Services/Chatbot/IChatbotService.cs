using ShoppingLearn.Models.Chatbot;

namespace ShoppingLearn.Services.Chatbot
{
    /// <summary>
    /// Interface cho Chatbot Service chính
    /// </summary>
    public interface IChatbotService
    {
        /// <summary>
        /// Xử lý message từ user và trả về response
        /// </summary>
        /// <param name="request">ChatRequest từ client</param>
        /// <returns>ChatResponse chứa câu trả lời</returns>
        Task<ChatResponse> ProcessMessageAsync(ChatRequest request);

        /// <summary>
        /// Lấy lịch sử chat của một session
        /// </summary>
        /// <param name="sessionId">ID của session</param>
        /// <returns>Danh sách messages</returns>
        List<ChatMessage> GetChatHistory(string sessionId);

        /// <summary>
        /// Xóa lịch sử chat của một session
        /// </summary>
        /// <param name="sessionId">ID của session</param>
        void ClearChatHistory(string sessionId);

        /// <summary>
        /// Kiểm tra rate limit (20 messages/phút/session)
        /// </summary>
        /// <param name="sessionId">ID của session</param>
        /// <returns>True nếu còn được phép gửi, False nếu vượt limit</returns>
        bool CheckRateLimit(string sessionId);
    }
}
