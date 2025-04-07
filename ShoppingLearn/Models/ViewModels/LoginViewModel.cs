using System.ComponentModel.DataAnnotations;

namespace ShoppingLearn.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Nhập user name")]
		public string Username { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Nhập Password")]

		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
