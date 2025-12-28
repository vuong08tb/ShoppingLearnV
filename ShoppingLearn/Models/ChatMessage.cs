using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingLearn.Models
{
	public class ChatMessage
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public Guid ConversationId { get; set; }

		[ForeignKey("ConversationId")]
		public ChatConversation Conversation { get; set; }

		[Required]
		[MaxLength(20)]
		public string Role { get; set; } // "user" or "assistant"

		[Required]
		public string Content { get; set; }

		// JSON array of product IDs if this is a recommendation
		public string? ProductRecommendations { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}
