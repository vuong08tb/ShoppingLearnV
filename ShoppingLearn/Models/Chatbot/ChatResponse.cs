namespace ShoppingLearn.Models.Chatbot
{
    /// <summary>
    /// Model cho response từ chatbot trả về client
    /// </summary>
    public class ChatResponse
    {
        /// <summary>
        /// Nội dung câu trả lời từ chatbot
        /// </summary>
        public string Reply { get; set; }

        /// <summary>
        /// Các nguồn tham khảo được sử dụng để trả lời
        /// </summary>
        public List<string> Sources { get; set; } = new List<string>();

        /// <summary>
        /// Trạng thái thành công hay thất bại
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Thông báo lỗi (nếu có)
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Session ID
        /// </summary>
        public string SessionId { get; set; }
    }
}
