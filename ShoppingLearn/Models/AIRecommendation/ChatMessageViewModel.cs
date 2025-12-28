namespace ShoppingLearn.Models.AIRecommendation
{
	public class ChatMessageViewModel
	{
		public Guid Id { get; set; }
		public string Role { get; set; } // "user" or "assistant"
		public string Content { get; set; }
		public List<ProductRecommendationViewModel>? Products { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
