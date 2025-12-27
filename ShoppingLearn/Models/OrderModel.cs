namespace ShoppingLearn.Models
{
	public class OrderModel
	{
		public int Id { get; set; }
		public string Order_code { get; set; }
		public decimal ShippingCost { get; set; }
		public string? PaymentMethod { get; set; }
		public string CouponCode { get; set; }
		public string UserName { get; set; }
		public DateTime CreatedDate { get; set; }
		public int Status { get; set; }
		public string ShippingAddress { get; set; }
		public string Receiver { get; set; }
		public string PhoneReceiver { get; set; }

    }
}
