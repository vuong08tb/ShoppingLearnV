using Microsoft.AspNetCore.Identity;

namespace ShoppingLearn.Resquest
{
    public class CreatUserRequest : IdentityUser
    {
        public string Occupation { get; set; }
        public string RoleId { get; set; }
        public string Token { get; set; }

        // AI Recommendation Preferences (optional)
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PreferredStyle { get; set; }
        public string? PreferredColors { get; set; }
        public string? SizePreference { get; set; }
        public string? PriceRange { get; set; }
        public string? Interests { get; set; }
    }
}
