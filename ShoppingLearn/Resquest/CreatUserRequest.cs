using Microsoft.AspNetCore.Identity;

namespace ShoppingLearn.Resquest
{
    public class CreatUserRequest : IdentityUser
    {
        public string Occupation { get; set; }
        public string RoleId { get; set; }
        public string Token { get; set; }
    }
}
