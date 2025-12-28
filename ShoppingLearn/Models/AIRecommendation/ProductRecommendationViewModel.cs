namespace ShoppingLearn.Models.AIRecommendation
{
	public class ProductRecommendationViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Image { get; set; }
		public decimal Price { get; set; }
		public string Reason { get; set; } // Lý do gợi ý
		public string Slug { get; set; }
	}
}
