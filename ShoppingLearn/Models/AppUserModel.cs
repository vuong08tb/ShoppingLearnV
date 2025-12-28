using Microsoft.AspNetCore.Identity;

namespace ShoppingLearn.Models
{
	public class AppUserModel :IdentityUser
	{
		public string Occupation {  get; set; }
		public string Token { get; set; }

		// AI Recommendation Preferences (optional fields)
		public string? Gender { get; set; } // Nam, Nữ, Khác
		public DateTime? DateOfBirth { get; set; }
		public string? PreferredStyle { get; set; } // Sporty, Casual, Formal, Street, Vintage, etc.
		public string? PreferredColors { get; set; } // JSON array or CSV: "Đỏ,Xanh,Đen"
		public string? SizePreference { get; set; } // S, M, L, XL, XXL
		public string? PriceRange { get; set; } // Budget, Medium, Premium
		public string? Interests { get; set; } // JSON array: "Thể thao,Công sở,Dạo phố"
	}
}
