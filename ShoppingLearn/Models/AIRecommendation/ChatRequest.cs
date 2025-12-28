namespace ShoppingLearn.Models.AIRecommendation
{
	public class AIChatRequest
	{
		public string Message { get; set; }
		public Guid? ConversationId { get; set; } // null for new conversation
	}
}
