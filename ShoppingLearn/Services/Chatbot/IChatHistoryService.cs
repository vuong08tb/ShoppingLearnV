using ShoppingLearn.Models;
using ShoppingLearn.Models.AIRecommendation;

namespace ShoppingLearn.Services.Chatbot
{
	public interface IChatHistoryService
	{
		// Conversation management
		Task<ChatConversation> CreateConversationAsync(string userId, string title);
		Task<ChatConversation?> GetConversationAsync(Guid conversationId);
		Task<List<ConversationViewModel>> GetUserConversationsAsync(string userId);
		Task<bool> DeleteConversationAsync(Guid conversationId, string userId);
		Task UpdateConversationTitleAsync(Guid conversationId, string newTitle);

		// Message management
		Task<ChatMessage> AddMessageAsync(Guid conversationId, string role, string content, string? productRecommendations = null);
		Task<List<ChatMessageViewModel>> GetConversationMessagesAsync(Guid conversationId);
	}
}
