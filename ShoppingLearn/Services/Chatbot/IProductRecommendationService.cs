using ShoppingLearn.Models;
using ShoppingLearn.Models.AIRecommendation;

namespace ShoppingLearn.Services.Chatbot
{
	public interface IProductRecommendationService
	{
		/// <summary>
		/// Xử lý message từ user và trả về response với product recommendations
		/// </summary>
		Task<AIChatResponse> ProcessMessageAsync(string userId, AIChatRequest request);

		/// <summary>
		/// Lấy lịch sử conversation của user
		/// </summary>
		Task<List<ConversationViewModel>> GetUserConversationsAsync(string userId);

		/// <summary>
		/// Lấy messages của một conversation
		/// </summary>
		Task<List<ChatMessageViewModel>> GetConversationMessagesAsync(Guid conversationId);

		/// <summary>
		/// Xóa conversation
		/// </summary>
		Task<bool> DeleteConversationAsync(Guid conversationId, string userId);

		/// <summary>
		/// Tạo conversation mới
		/// </summary>
		Task<Guid> CreateNewConversationAsync(string userId, string firstMessage);
	}
}
