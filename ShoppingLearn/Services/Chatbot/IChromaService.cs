namespace ShoppingLearn.Services.Chatbot
{
    /// <summary>
    /// Interface cho Chroma Vector Database Service (RAG)
    /// </summary>
    public interface IChromaService
    {
        /// <summary>
        /// Khởi tạo collection trong ChromaDB
        /// </summary>
        Task InitializeCollectionAsync();

        /// <summary>
        /// Đọc và lưu tài liệu từ thư mục Knowledge vào ChromaDB
        /// </summary>
        /// <param name="knowledgePath">Đường dẫn đến thư mục Knowledge</param>
        Task IngestDocumentsAsync(string knowledgePath);

        /// <summary>
        /// Tìm kiếm tài liệu liên quan trong ChromaDB dựa trên query
        /// </summary>
        /// <param name="query">Câu query từ user</param>
        /// <param name="topK">Số lượng kết quả trả về</param>
        /// <returns>Danh sách các đoạn text liên quan</returns>
        Task<List<string>> SearchAsync(string query, int topK = 3);
    }
}
