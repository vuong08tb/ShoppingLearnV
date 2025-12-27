namespace ShoppingLearn.Models.Chatbot
{
    /// <summary>
    /// Model cho request từ client gửi lên chatbot
    /// </summary>
    public class ChatRequest
    {
        /// <summary>
        /// Nội dung tin nhắn từ user
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Session ID để theo dõi lịch sử chat
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// User ID (optional) - nếu user đã đăng nhập
        /// </summary>
        public string UserId { get; set; }
    }
}
