namespace ShoppingLearn.Models.AIRecommendation
{
	public class ConversationViewModel
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public bool IsActive { get; set; } // Currently selected conversation
	}
}
