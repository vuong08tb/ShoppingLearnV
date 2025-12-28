using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingLearn.Models.AIRecommendation;
using ShoppingLearn.Services.Chatbot;
using System.Security.Claims;

namespace ShoppingLearn.Controllers
{
	[Authorize] // Chỉ user đã login mới truy cập được
	public class AIRecommendationController : Controller
	{
		private readonly IProductRecommendationService _recommendationService;
		private readonly ILogger<AIRecommendationController> _logger;

		public AIRecommendationController(
			IProductRecommendationService recommendationService,
			ILogger<AIRecommendationController> logger)
		{
			_recommendationService = recommendationService;
			_logger = logger;
		}

		/// <summary>
		/// Trang chính AI Recommendation Chat
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> Index(Guid? conversationId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			// Lấy danh sách conversations
			var conversations = await _recommendationService.GetUserConversationsAsync(userId);

			// Nếu có conversationId, đánh dấu active
			if (conversationId.HasValue)
			{
				var activeConv = conversations.FirstOrDefault(c => c.Id == conversationId.Value);
				if (activeConv != null)
				{
					activeConv.IsActive = true;
				}
			}

			ViewBag.Conversations = conversations;
			ViewBag.ActiveConversationId = conversationId;

			return View();
		}

		/// <summary>
		/// API: Gửi message và nhận recommendation
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> SendMessage([FromBody] AIChatRequest request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

				if (string.IsNullOrEmpty(userId))
				{
					return Unauthorized(new { success = false, errorMessage = "Vui lòng đăng nhập." });
				}

				if (string.IsNullOrWhiteSpace(request.Message))
				{
					return BadRequest(new { success = false, errorMessage = "Tin nhắn không được để trống." });
				}

				var response = await _recommendationService.ProcessMessageAsync(userId, request);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in SendMessage: {ex.Message}");
				return StatusCode(500, new AIChatResponse
				{
					Success = false,
					ErrorMessage = "Đã có lỗi xảy ra. Vui lòng thử lại sau."
				});
			}
		}

		/// <summary>
		/// API: Lấy lịch sử messages của một conversation
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetMessages(Guid conversationId)
		{
			try
			{
				var messages = await _recommendationService.GetConversationMessagesAsync(conversationId);
				return Ok(new { success = true, messages });
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in GetMessages: {ex.Message}");
				return StatusCode(500, new { success = false, errorMessage = "Không thể tải tin nhắn." });
			}
		}

		/// <summary>
		/// API: Lấy danh sách conversations
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetConversations()
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var conversations = await _recommendationService.GetUserConversationsAsync(userId);
				return Ok(new { success = true, conversations });
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in GetConversations: {ex.Message}");
				return StatusCode(500, new { success = false, errorMessage = "Không thể tải danh sách hội thoại." });
			}
		}

		/// <summary>
		/// API: Tạo conversation mới
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> CreateConversation([FromBody] CreateConversationRequest request)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var conversationId = await _recommendationService.CreateNewConversationAsync(userId, request.FirstMessage);
				return Ok(new { success = true, conversationId });
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in CreateConversation: {ex.Message}");
				return StatusCode(500, new { success = false, errorMessage = "Không thể tạo hội thoại mới." });
			}
		}

		/// <summary>
		/// API: Xóa conversation
		/// </summary>
		[HttpDelete]
		public async Task<IActionResult> DeleteConversation(Guid conversationId)
		{
			try
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				var success = await _recommendationService.DeleteConversationAsync(conversationId, userId);

				if (success)
				{
					return Ok(new { success = true, message = "Đã xóa hội thoại." });
				}
				else
				{
					return NotFound(new { success = false, errorMessage = "Không tìm thấy hội thoại." });
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error in DeleteConversation: {ex.Message}");
				return StatusCode(500, new { success = false, errorMessage = "Không thể xóa hội thoại." });
			}
		}
	}

	// Request models
	public class CreateConversationRequest
	{
		public string FirstMessage { get; set; }
	}
}
