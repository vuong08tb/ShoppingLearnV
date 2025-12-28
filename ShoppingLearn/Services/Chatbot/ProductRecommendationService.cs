using Microsoft.AspNetCore.Identity;
using ShoppingLearn.Models;
using ShoppingLearn.Models.AIRecommendation;
using ShoppingLearn.Models.Chatbot;
using System.Text.Json;

namespace ShoppingLearn.Services.Chatbot
{
	public class ProductRecommendationService : IProductRecommendationService
	{
		private readonly IGeminiService _geminiService;
		private readonly IChromaService _chromaService;
		private readonly ISqlQueryService _sqlQueryService;
		private readonly IChatHistoryService _chatHistoryService;
		private readonly UserManager<AppUserModel> _userManager;
		private readonly IConfiguration _configuration;
		private readonly ILogger<ProductRecommendationService> _logger;

		public ProductRecommendationService(
			IGeminiService geminiService,
			IChromaService chromaService,
			ISqlQueryService sqlQueryService,
			IChatHistoryService chatHistoryService,
			UserManager<AppUserModel> userManager,
			IConfiguration configuration,
			ILogger<ProductRecommendationService> logger)
		{
			_geminiService = geminiService;
			_chromaService = chromaService;
			_sqlQueryService = sqlQueryService;
			_chatHistoryService = chatHistoryService;
			_userManager = userManager;
			_configuration = configuration;
			_logger = logger;
		}

		public async Task<AIChatResponse> ProcessMessageAsync(string userId, AIChatRequest request)
		{
			try
			{
				// Lấy thông tin user với preferences
				var user = await _userManager.FindByIdAsync(userId);
				if (user == null)
				{
					return new AIChatResponse
					{
						Success = false,
						ErrorMessage = "Không tìm thấy thông tin người dùng."
					};
				}

				// Lấy hoặc tạo conversation
				Guid conversationId;
				if (request.ConversationId.HasValue && request.ConversationId.Value != Guid.Empty)
				{
					conversationId = request.ConversationId.Value;
				}
				else
				{
					// Tạo conversation mới
					var title = request.Message.Length > 50
						? request.Message.Substring(0, 50) + "..."
						: request.Message;
					var conversation = await _chatHistoryService.CreateConversationAsync(userId, title);
					conversationId = conversation.Id;
				}

				// Lưu user message
				await _chatHistoryService.AddMessageAsync(conversationId, "user", request.Message);

				// Detect intent
				var intent = _sqlQueryService.DetectIntent(request.Message);

				// Kiểm tra xem có phải là product recommendation request không
				bool isRecommendationRequest = IsRecommendationRequest(request.Message);

				List<ProductRecommendationViewModel>? recommendedProducts = null;
				string aiResponse;

				if (isRecommendationRequest || intent.NeedDatabaseQuery)
				{
					// Lấy sản phẩm từ database dựa trên user preferences
					var maxProducts = _configuration.GetValue<int>("AIRecommendation:MaxProducts", 5);
					var products = await _sqlQueryService.GetRecommendedProductsAsync(user, intent.Keywords, maxProducts);

					if (products.Any())
					{
						// Convert sang ProductRecommendationViewModel
						recommendedProducts = products.Select(p => new ProductRecommendationViewModel
						{
							Id = p.ProductId,
							Name = p.ProductName,
							Image = p.ImageUrl ?? "/images/default-product.jpg",
							Price = p.Price,
							Slug = GenerateSlug(p.ProductName, p.ProductId),
							Reason = "" // Sẽ được AI điền
						}).ToList();

						// Lấy context từ RAG
						var ragContext = await _chromaService.SearchAsync(request.Message, 3);

						// Build system prompt với user preferences
						var systemPrompt = BuildSystemPrompt(user);

						// Build context với product info
						var productContext = _sqlQueryService.FormatProductResults(products);
						var fullContext = new List<string>();
						fullContext.AddRange(ragContext);
						fullContext.Add(productContext);

						// Gọi Gemini AI để tạo response và lý do gợi ý
						var prompt = $@"{request.Message}

Dựa trên thông tin sản phẩm trên, hãy:
1. Giải thích tại sao các sản phẩm này phù hợp với khách hàng
2. Đưa ra lời khuyên về cách phối đồ hoặc sử dụng
3. Đề cập đến ưu điểm của từng sản phẩm

QUAN TRỌNG: Trả về response dưới dạng JSON với format:
{{
  ""reply"": ""<lời giải thích chung>"",
  ""productReasons"": [
    {{
      ""productId"": <id>,
      ""reason"": ""<lý do gợi ý cụ thể cho sản phẩm này>""
    }}
  ]
}}";

						aiResponse = await _geminiService.SendMessageAsync(prompt, systemPrompt, fullContext);

						// Parse AI response để lấy reasons cho từng sản phẩm
						try
						{
							var aiJson = JsonDocument.Parse(aiResponse);
							var reply = aiJson.RootElement.GetProperty("reply").GetString();
							var productReasons = aiJson.RootElement.GetProperty("productReasons").EnumerateArray();

							// Cập nhật reasons cho products
							foreach (var pr in productReasons)
							{
								var productId = pr.GetProperty("productId").GetInt32();
								var reason = pr.GetProperty("reason").GetString();

								var product = recommendedProducts.FirstOrDefault(p => p.Id == productId);
								if (product != null)
								{
									product.Reason = reason ?? "Sản phẩm chất lượng, phù hợp với phong cách của bạn.";
								}
							}

							aiResponse = reply ?? aiResponse;
						}
						catch
						{
							// Nếu AI không trả JSON, giữ nguyên response và dùng default reasons
							foreach (var product in recommendedProducts)
							{
								product.Reason = "Sản phẩm chất lượng, phù hợp với phong cách của bạn.";
							}
						}
					}
					else
					{
						// Không tìm thấy sản phẩm phù hợp
						var ragContext = await _chromaService.SearchAsync(request.Message, 3);
						var systemPrompt = BuildSystemPrompt(user);
						aiResponse = await _geminiService.SendMessageAsync(
							request.Message + "\n\nKhông tìm thấy sản phẩm phù hợp. Hãy xin lỗi và gợi ý thử tìm kiếm với từ khóa khác.",
							systemPrompt,
							ragContext
						);
					}
				}
				else
				{
					// Câu hỏi chung, không cần recommendation
					var ragContext = await _chromaService.SearchAsync(request.Message, 3);
					var systemPrompt = BuildSystemPrompt(user);
					aiResponse = await _geminiService.SendMessageAsync(request.Message, systemPrompt, ragContext);
				}

				// Lưu assistant message với product recommendations
				string? productJson = null;
				if (recommendedProducts != null && recommendedProducts.Any())
				{
					productJson = JsonSerializer.Serialize(recommendedProducts);
				}

				await _chatHistoryService.AddMessageAsync(conversationId, "assistant", aiResponse, productJson);

				return new AIChatResponse
				{
					Success = true,
					Reply = aiResponse,
					ConversationId = conversationId,
					Products = recommendedProducts
				};
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error processing recommendation message: {ex.Message}");
				return new AIChatResponse
				{
					Success = false,
					ErrorMessage = "Đã có lỗi xảy ra khi xử lý yêu cầu của bạn. Vui lòng thử lại sau."
				};
			}
		}

		public async Task<List<ConversationViewModel>> GetUserConversationsAsync(string userId)
		{
			return await _chatHistoryService.GetUserConversationsAsync(userId);
		}

		public async Task<List<ChatMessageViewModel>> GetConversationMessagesAsync(Guid conversationId)
		{
			return await _chatHistoryService.GetConversationMessagesAsync(conversationId);
		}

		public async Task<bool> DeleteConversationAsync(Guid conversationId, string userId)
		{
			return await _chatHistoryService.DeleteConversationAsync(conversationId, userId);
		}

		public async Task<Guid> CreateNewConversationAsync(string userId, string firstMessage)
		{
			var title = firstMessage.Length > 50
				? firstMessage.Substring(0, 50) + "..."
				: firstMessage;
			var conversation = await _chatHistoryService.CreateConversationAsync(userId, title);
			return conversation.Id;
		}

		// Helper methods
		private bool IsRecommendationRequest(string message)
		{
			var recommendKeywords = new[]
			{
				"gợi ý", "tư vấn", "nên mua", "nên chọn", "phù hợp",
				"recommend", "suggest", "advice", "tìm", "mua gì",
				"đẹp", "hợp", "outfit", "phối đồ"
			};

			var messageLower = message.ToLower();
			return recommendKeywords.Any(k => messageLower.Contains(k));
		}

		private string BuildSystemPrompt(AppUserModel user)
		{
			var age = user.DateOfBirth.HasValue
				? DateTime.Now.Year - user.DateOfBirth.Value.Year
				: 0;

			return $@"Bạn là chuyên gia tư vấn thời trang của ShoppingLearn, một cửa hàng thời trang trực tuyến.

THÔNG TIN KHÁCH HÀNG:
- Tên: {user.UserName}
- Giới tính: {user.Gender ?? "Chưa xác định"}
- Tuổi: {(age > 0 ? age.ToString() : "Chưa xác định")}
- Phong cách ưa thích: {user.PreferredStyle ?? "Chưa xác định"}
- Màu sắc yêu thích: {user.PreferredColors ?? "Chưa xác định"}
- Size thường mặc: {user.SizePreference ?? "Chưa xác định"}
- Khoảng giá ưa thích: {user.PriceRange ?? "Chưa xác định"}
- Sở thích: {user.Interests ?? "Chưa xác định"}

NHIỆM VỤ CỦA BẠN:
1. Tư vấn sản phẩm dựa trên thông tin cá nhân của khách hàng
2. Giải thích rõ ràng tại sao sản phẩm phù hợp với khách hàng
3. Đưa ra lời khuyên về phối đồ và cách sử dụng
4. Luôn thân thiện, chuyên nghiệp và tận tâm
5. Sử dụng tiếng Việt tự nhiên, dễ hiểu

LƯU Ý:
- Nếu khách hàng hỏi về sản phẩm, ưu tiên gợi ý sản phẩm phù hợp với preferences
- Nếu không tìm thấy sản phẩm, gợi ý thử từ khóa khác hoặc mở rộng tiêu chí
- Luôn đề cập đến lợi ích và ưu điểm của sản phẩm
- Khuyến khích khách hàng thử nhiều phong cách mới";
		}

		private string GenerateSlug(string name, int id)
		{
			// Simple slug generation
			var slug = name.ToLower()
				.Replace(" ", "-")
				.Replace("á", "a").Replace("à", "a").Replace("ả", "a").Replace("ã", "a").Replace("ạ", "a")
				.Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("ặ", "a")
				.Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ậ", "a")
				.Replace("đ", "d")
				.Replace("é", "e").Replace("è", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("ẹ", "e")
				.Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e")
				.Replace("í", "i").Replace("ì", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("ị", "i")
				.Replace("ó", "o").Replace("ò", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ọ", "o")
				.Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ộ", "o")
				.Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("ợ", "o")
				.Replace("ú", "u").Replace("ù", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ụ", "u")
				.Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ử", "u").Replace("ữ", "u").Replace("ự", "u")
				.Replace("ý", "y").Replace("ỳ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("ỵ", "y");

			return $"{slug}-{id}";
		}
	}
}
