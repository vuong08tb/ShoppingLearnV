using ShoppingLearn.Models.Vnpay;

namespace ShoppingLearn.Services.Vnpay
{
	public interface IVnPayService
	{
		string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
		PaymentResponseModel PaymentExecute(IQueryCollection collections);

	}
}
