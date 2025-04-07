using System.ComponentModel.DataAnnotations;

namespace ShoppingLearn.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage ="Nhập user name")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Nhập Email"),EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage ="Nhập Password")]	
		
		public string Password { get; set; }

	}
}
