namespace ShoppingLearn.Models.AIRecommendation
{
	public class AIChatResponse
	{
		public bool Success { get; set; }
		public string Reply { get; set; }
		public Guid ConversationId { get; set; }
		public List<ProductRecommendationViewModel>? Products { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
