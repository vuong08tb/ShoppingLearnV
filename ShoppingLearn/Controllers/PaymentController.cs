using Microsoft.AspNetCore.Mvc;
using ShoppingLearn.Models.Order;
using ShoppingLearn.Services.Momo;

namespace ShoppingLearn.Controllers
{
    public class PaymentController : Controller
    {
		private IMomoService _momoService;
		public PaymentController(IMomoService momoService)
		{
			_momoService = momoService;
		}
		[HttpPost]
		public async Task<IActionResult> CreatePaymentMomo(OrderInfoModel model)
		{
			var response = await _momoService.CreatePaymentMomo(model);
			return Redirect(response.PayUrl);
		}
		[HttpGet]
		public IActionResult PaymentCallBack()
		{
			var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
			return View(response);
		}
	}
}
