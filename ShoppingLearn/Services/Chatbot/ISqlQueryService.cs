using ShoppingLearn.Models.Chatbot;
using ShoppingLearn.Models;

namespace ShoppingLearn.Services.Chatbot
{
    /// <summary>
    /// Interface cho SQL Query Service
    /// </summary>
    public interface ISqlQueryService
    {
        /// <summary>
        /// Phát hiện intent từ câu hỏi của user
        /// </summary>
        /// <param name="message">Message từ user</param>
        /// <returns>QueryIntent chứa thông tin về intent</returns>
        QueryIntent DetectIntent(string message);

        /// <summary>
        /// Truy vấn database dựa trên intent
        /// </summary>
        /// <param name="intent">Intent đã được detect</param>
        /// <returns>Danh sách sản phẩm</returns>
        Task<List<ProductSearchResult>> QueryProductsAsync(QueryIntent intent);

        /// <summary>
        /// Lấy thông tin chi tiết một sản phẩm theo ID
        /// </summary>
        /// <param name="productId">ID của sản phẩm</param>
        /// <returns>Thông tin sản phẩm</returns>
        Task<ProductSearchResult> GetProductByIdAsync(int productId);

        /// <summary>
        /// Lấy danh sách categories
        /// </summary>
        /// <returns>Danh sách tên categories</returns>
        Task<List<string>> GetCategoriesAsync();

        /// <summary>
        /// Format kết quả products thành text để gửi cho Gemini
        /// </summary>
        /// <param name="products">Danh sách sản phẩm</param>
        /// <returns>Chuỗi text đã format</returns>
        string FormatProductResults(List<ProductSearchResult> products);

        // AI Recommendation methods
        /// <summary>
        /// Tìm kiếm sản phẩm phù hợp với user preferences
        /// </summary>
        /// <param name="user">Thông tin user với preferences</param>
        /// <param name="keywords">Keywords từ câu hỏi</param>
        /// <param name="maxResults">Số lượng sản phẩm tối đa</param>
        /// <returns>Danh sách sản phẩm gợi ý</returns>
        Task<List<ProductSearchResult>> GetRecommendedProductsAsync(AppUserModel user, List<string> keywords, int maxResults = 5);

        /// <summary>
        /// Lấy sản phẩm theo category và filters
        /// </summary>
        Task<List<ProductSearchResult>> GetProductsByCategoryAsync(string category, decimal? minPrice = null, decimal? maxPrice = null, int maxResults = 10);

        /// <summary>
        /// Lấy sản phẩm theo style
        /// </summary>
        Task<List<ProductSearchResult>> GetProductsByStyleAsync(string style, int maxResults = 10);
    }

    /// <summary>
    /// Model chứa thông tin về intent của user
    /// </summary>
    public class QueryIntent
    {
        public bool NeedDatabaseQuery { get; set; }
        public QueryType QueryType { get; set; }
        public List<string> Keywords { get; set; } = new List<string>();
    }

    /// <summary>
    /// Enum các loại query
    /// </summary>
    public enum QueryType
    {
        General,    // Câu hỏi chung
        Price,      // Hỏi về giá
        Stock,      // Hỏi về tồn kho
        Category,   // Hỏi về danh mục
        Search      // Tìm kiếm sản phẩm
    }
}
