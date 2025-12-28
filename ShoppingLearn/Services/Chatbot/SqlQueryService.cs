using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Repository;
using ShoppingLearn.Models.Chatbot;
using ShoppingLearn.Models;
using System.Text.RegularExpressions;

namespace ShoppingLearn.Services.Chatbot
{
    /// <summary>
    /// Service xử lý truy vấn SQL an toàn để lấy thông tin sản phẩm
    /// </summary>
    public class SqlQueryService : ISqlQueryService
    {
        private readonly DataContext _context;
        private readonly ILogger<SqlQueryService> _logger;

        public SqlQueryService(DataContext context, ILogger<SqlQueryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Phát hiện intent từ câu hỏi của user
        /// </summary>
        public QueryIntent DetectIntent(string message)
        {
            var messageLower = message.ToLower();

            // Keywords cho các intent khác nhau
            var priceKeywords = new[] { "giá", "bao nhiêu tiền", "giá bán", "giá cả", "đắt", "rẻ", "cost", "price" };
            var stockKeywords = new[] { "còn hàng", "tồn kho", "có sẵn", "available", "stock", "hết hàng" };
            var categoryKeywords = new[] { "loại", "danh mục", "category", "thể loại", "phân loại" };
            var searchKeywords = new[] { "tìm", "search", "có", "xem", "cho tôi", "liệt kê" };

            var intent = new QueryIntent
            {
                NeedDatabaseQuery = false,
                QueryType = QueryType.General
            };

            // Detect query type
            if (priceKeywords.Any(k => messageLower.Contains(k)))
            {
                intent.NeedDatabaseQuery = true;
                intent.QueryType = QueryType.Price;
            }
            else if (stockKeywords.Any(k => messageLower.Contains(k)))
            {
                intent.NeedDatabaseQuery = true;
                intent.QueryType = QueryType.Stock;
            }
            else if (categoryKeywords.Any(k => messageLower.Contains(k)))
            {
                intent.NeedDatabaseQuery = true;
                intent.QueryType = QueryType.Category;
            }
            else if (searchKeywords.Any(k => messageLower.Contains(k)))
            {
                intent.NeedDatabaseQuery = true;
                intent.QueryType = QueryType.Search;
            }

            // Extract product name or keywords
            intent.Keywords = ExtractKeywords(message);

            return intent;
        }

        /// <summary>
        /// Truy vấn database dựa trên intent
        /// </summary>
        public async Task<List<ProductSearchResult>> QueryProductsAsync(QueryIntent intent)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                // Filter theo keywords nếu có (OR logic - match bất kỳ keyword nào)
                if (intent.Keywords.Any())
                {
                    var keywordsLower = intent.Keywords.Select(k => k.ToLower()).ToList();
                    query = query.Where(p =>
                        keywordsLower.Any(k =>
                            p.Name.ToLower().Contains(k) ||
                            p.Description.ToLower().Contains(k)
                        )
                    );
                }

                // Filter theo query type
                switch (intent.QueryType)
                {
                    case QueryType.Stock:
                        query = query.Where(p => p.Quantity > 0);
                        break;
                    case QueryType.Price:
                        // Sắp xếp theo giá nếu hỏi về giá
                        query = query.OrderBy(p => p.Price);
                        break;
                }

                var products = await query
                    .Take(5) // Giới hạn 5 sản phẩm
                    .Select(p => new ProductSearchResult
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Category = p.CategoryId.ToString(),
                        Price = p.Price,
                        Stock = p.Quantity,
                        Description = p.Description,
                        ImageUrl = !string.IsNullOrEmpty(p.Image) ? $"/media/products/{p.Image}" : "/images/default-product.jpg"
                    })
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error querying products: {ex.Message}");
                return new List<ProductSearchResult>();
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết một sản phẩm theo ID
        /// </summary>
        public async Task<ProductSearchResult> GetProductByIdAsync(int productId)
        {
            try
            {
                var product = await _context.Products
                    .Where(p => p.Id == productId)
                    .Select(p => new ProductSearchResult
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Category = p.CategoryId.ToString(),
                        Price = p.Price,
                        Stock = p.Quantity,
                        Description = p.Description,
                        ImageUrl = !string.IsNullOrEmpty(p.Image) ? $"/media/products/{p.Image}" : "/images/default-product.jpg"
                    })
                    .FirstOrDefaultAsync();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting product: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách categories
        /// </summary>
        public async Task<List<string>> GetCategoriesAsync()
        {
            try
            {
                return await _context.Categories
                    .Select(c => c.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting categories: {ex.Message}");
                return new List<string>();
            }
        }

        /// <summary>
        /// Format kết quả products thành text để gửi cho Gemini
        /// </summary>
        public string FormatProductResults(List<ProductSearchResult> products)
        {
            if (!products.Any())
                return "Không tìm thấy sản phẩm phù hợp.";

            var result = new System.Text.StringBuilder();
            result.AppendLine("Thông tin sản phẩm:");

            foreach (var product in products)
            {
                result.AppendLine($"\n- {product.ProductName}");
                result.AppendLine($"  Giá: {product.Price:N0} VNĐ");
                result.AppendLine($"  Tồn kho: {product.Stock} sản phẩm");
                if (!string.IsNullOrEmpty(product.Description))
                {
                    var shortDesc = product.Description.Length > 100
                        ? product.Description.Substring(0, 100) + "..."
                        : product.Description;
                    result.AppendLine($"  Mô tả: {shortDesc}");
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Trích xuất keywords từ message
        /// </summary>
        private List<string> ExtractKeywords(string message)
        {
            // Loại bỏ stop words tiếng Việt
            var stopWords = new[] { "tôi", "bạn", "cho", "của", "và", "có", "là", "được", "này", "thế", "nào", "gì" };

            var words = Regex.Replace(message, @"[^\w\s]", " ")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(w => w.Length > 2 && !stopWords.Contains(w.ToLower()))
                .ToList();

            return words;
        }

        // AI Recommendation Methods
        /// <summary>
        /// Tìm kiếm sản phẩm phù hợp với user preferences
        /// </summary>
        public async Task<List<ProductSearchResult>> GetRecommendedProductsAsync(AppUserModel user, List<string> keywords, int maxResults = 5)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                // Filter theo keywords từ câu hỏi (OR logic - match bất kỳ keyword nào)
                if (keywords.Any())
                {
                    var keywordsLower = keywords.Select(k => k.ToLower()).ToList();
                    query = query.Where(p =>
                        keywordsLower.Any(k =>
                            p.Name.ToLower().Contains(k) ||
                            p.Description.ToLower().Contains(k)
                        )
                    );
                }

                // Filter theo price range preference
                if (!string.IsNullOrEmpty(user.PriceRange))
                {
                    query = user.PriceRange.ToLower() switch
                    {
                        "budget" => query.Where(p => p.Price < 500000),
                        "medium" => query.Where(p => p.Price >= 500000 && p.Price < 1500000),
                        "premium" => query.Where(p => p.Price >= 1500000),
                        _ => query
                    };
                }

                // Filter theo style (tìm trong Name hoặc Description)
                if (!string.IsNullOrEmpty(user.PreferredStyle))
                {
                    var style = user.PreferredStyle.ToLower();
                    query = query.Where(p =>
                        p.Name.ToLower().Contains(style) ||
                        p.Description.ToLower().Contains(style)
                    );
                }

                // Chỉ lấy sản phẩm còn hàng
                query = query.Where(p => p.Quantity > 0);

                var products = await query
                    .Take(maxResults)
                    .Select(p => new ProductSearchResult
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Category = p.CategoryId.ToString(),
                        Price = p.Price,
                        Stock = p.Quantity,
                        Description = p.Description,
                        ImageUrl = !string.IsNullOrEmpty(p.Image) ? $"/media/products/{p.Image}" : "/images/default-product.jpg"
                    })
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting recommended products: {ex.Message}");
                return new List<ProductSearchResult>();
            }
        }

        /// <summary>
        /// Lấy sản phẩm theo category và filters
        /// </summary>
        public async Task<List<ProductSearchResult>> GetProductsByCategoryAsync(string category, decimal? minPrice = null, decimal? maxPrice = null, int maxResults = 10)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                // Filter by category name
                if (!string.IsNullOrEmpty(category))
                {
                    var categoryLower = category.ToLower();
                    query = query.Where(p => p.Category.Name.ToLower().Contains(categoryLower));
                }

                // Filter by price range
                if (minPrice.HasValue)
                    query = query.Where(p => p.Price >= minPrice.Value);

                if (maxPrice.HasValue)
                    query = query.Where(p => p.Price <= maxPrice.Value);

                // Only available products
                query = query.Where(p => p.Quantity > 0);

                var products = await query
                    .Take(maxResults)
                    .Select(p => new ProductSearchResult
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Category = p.Category.Name,
                        Price = p.Price,
                        Stock = p.Quantity,
                        Description = p.Description,
                        ImageUrl = !string.IsNullOrEmpty(p.Image) ? $"/media/products/{p.Image}" : "/images/default-product.jpg"
                    })
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting products by category: {ex.Message}");
                return new List<ProductSearchResult>();
            }
        }

        /// <summary>
        /// Lấy sản phẩm theo style
        /// </summary>
        public async Task<List<ProductSearchResult>> GetProductsByStyleAsync(string style, int maxResults = 10)
        {
            try
            {
                var styleLower = style.ToLower();

                var products = await _context.Products
                    .Where(p => p.Quantity > 0 && (
                        p.Name.ToLower().Contains(styleLower) ||
                        p.Description.ToLower().Contains(styleLower)
                    ))
                    .Take(maxResults)
                    .Select(p => new ProductSearchResult
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        Category = p.Category.Name,
                        Price = p.Price,
                        Stock = p.Quantity,
                        Description = p.Description,
                        ImageUrl = !string.IsNullOrEmpty(p.Image) ? $"/media/products/{p.Image}" : "/images/default-product.jpg"
                    })
                    .ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting products by style: {ex.Message}");
                return new List<ProductSearchResult>();
            }
        }
    }
}
