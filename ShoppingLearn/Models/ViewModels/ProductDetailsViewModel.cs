namespace ShoppingLearn.Models.ViewModels
{
	public class ProductDetailsViewModel
	{
		public ProductModel ProductDetails { get; set; }
		public RatingModel RatingDetails { get; set; } // nhập mới 
		public List<RatingModel> RatingList { get; set; } // danh sách đánh giá hiện có 
	}
}
