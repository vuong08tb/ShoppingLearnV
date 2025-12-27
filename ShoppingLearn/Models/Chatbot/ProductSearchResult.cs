namespace ShoppingLearn.Models.Chatbot
{
    /// <summary>
    /// Model cho kết quả tìm kiếm sản phẩm từ database
    /// </summary>
    public class ProductSearchResult
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
