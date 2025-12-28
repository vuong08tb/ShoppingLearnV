using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingLearn.Models
{
	public class ChatConversation
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string UserId { get; set; }

		[ForeignKey("UserId")]
		public AppUserModel User { get; set; }

		[Required]
		[MaxLength(200)]
		public string Title { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public bool IsDeleted { get; set; }

		// Navigation property
		public ICollection<ChatMessage> Messages { get; set; }
	}
}
