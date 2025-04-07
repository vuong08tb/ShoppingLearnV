using ShoppingLearn.Models.Momo;
using ShoppingLearn.Models.Order;

namespace ShoppingLearn.Services.Momo
{
    public interface IMomoService
    {
		Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model);
		MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
	}
}
