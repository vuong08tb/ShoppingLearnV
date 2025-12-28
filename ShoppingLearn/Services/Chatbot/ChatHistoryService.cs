using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Models;
using ShoppingLearn.Models.AIRecommendation;
using ShoppingLearn.Repository;
using System.Text.Json;

namespace ShoppingLearn.Services.Chatbot
{
	public class ChatHistoryService : IChatHistoryService
	{
		private readonly DataContext _context;

		public ChatHistoryService(DataContext context)
		{
			_context = context;
		}

		public async Task<ChatConversation> CreateConversationAsync(string userId, string title)
		{
			var conversation = new ChatConversation
			{
				Id = Guid.NewGuid(),
				UserId = userId,
				Title = title,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				IsDeleted = false
			};

			_context.ChatConversations.Add(conversation);
			await _context.SaveChangesAsync();

			return conversation;
		}

		public async Task<ChatConversation?> GetConversationAsync(Guid conversationId)
		{
			return await _context.ChatConversations
				.Include(c => c.Messages)
				.FirstOrDefaultAsync(c => c.Id == conversationId && !c.IsDeleted);
		}

		public async Task<List<ConversationViewModel>> GetUserConversationsAsync(string userId)
		{
			var conversations = await _context.ChatConversations
				.Where(c => c.UserId == userId && !c.IsDeleted)
				.OrderByDescending(c => c.UpdatedAt)
				.Select(c => new ConversationViewModel
				{
					Id = c.Id,
					Title = c.Title,
					CreatedAt = c.CreatedAt,
					UpdatedAt = c.UpdatedAt,
					IsActive = false
				})
				.ToListAsync();

			return conversations;
		}

		public async Task<bool> DeleteConversationAsync(Guid conversationId, string userId)
		{
			var conversation = await _context.ChatConversations
				.FirstOrDefaultAsync(c => c.Id == conversationId && c.UserId == userId);

			if (conversation == null)
				return false;

			conversation.IsDeleted = true;
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task UpdateConversationTitleAsync(Guid conversationId, string newTitle)
		{
			var conversation = await _context.ChatConversations
				.FirstOrDefaultAsync(c => c.Id == conversationId);

			if (conversation != null)
			{
				conversation.Title = newTitle;
				conversation.UpdatedAt = DateTime.Now;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<ChatMessage> AddMessageAsync(Guid conversationId, string role, string content, string? productRecommendations = null)
		{
			var message = new ChatMessage
			{
				Id = Guid.NewGuid(),
				ConversationId = conversationId,
				Role = role,
				Content = content,
				ProductRecommendations = productRecommendations,
				CreatedAt = DateTime.Now
			};

			_context.ChatMessages.Add(message);

			// Update conversation UpdatedAt
			var conversation = await _context.ChatConversations.FindAsync(conversationId);
			if (conversation != null)
			{
				conversation.UpdatedAt = DateTime.Now;
			}

			await _context.SaveChangesAsync();

			return message;
		}

		public async Task<List<ChatMessageViewModel>> GetConversationMessagesAsync(Guid conversationId)
		{
			var messages = await _context.ChatMessages
				.Where(m => m.ConversationId == conversationId)
				.OrderBy(m => m.CreatedAt)
				.ToListAsync();

			var viewModels = messages.Select(m => new ChatMessageViewModel
			{
				Id = m.Id,
				Role = m.Role,
				Content = m.Content,
				CreatedAt = m.CreatedAt,
				Products = string.IsNullOrEmpty(m.ProductRecommendations)
					? null
					: JsonSerializer.Deserialize<List<ProductRecommendationViewModel>>(m.ProductRecommendations)
			}).ToList();

			return viewModels;
		}
	}
}
