namespace ShoppingLearn.Services.Chatbot
{
    /// <summary>
    /// Interface cho Gemini AI Service
    /// </summary>
    public interface IGeminiService
    {
        /// <summary>
        /// Gửi message đến Gemini và nhận response
        /// </summary>
        /// <param name="userMessage">Tin nhắn từ user</param>
        /// <param name="systemPrompt">System prompt (optional)</param>
        /// <param name="context">Context từ RAG hoặc database (optional)</param>
        /// <returns>Response từ Gemini</returns>
        Task<string> SendMessageAsync(string userMessage, string systemPrompt = null, List<string> context = null);
    }
}
