namespace ShoppingLearn.Models.Chatbot
{
    /// <summary>
    /// Model lưu lịch sử chat
    /// </summary>
    public class ChatMessage
    {
        public string Role { get; set; } // "user" hoặc "assistant"
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
