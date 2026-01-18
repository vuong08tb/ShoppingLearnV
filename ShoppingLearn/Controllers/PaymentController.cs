using Microsoft.AspNetCore.Mvc;
using ShoppingLearn.Models.Order;
using ShoppingLearn.Models.Vnpay;
using ShoppingLearn.Services.Momo;
using ShoppingLearn.Services.Vnpay;

namespace ShoppingLearn.Controllers
{
    public class PaymentController : Controller
    {
		private IMomoService _momoService;
		private readonly IVnPayService _vnPayService;

		public PaymentController(IMomoService momoService, IVnPayService vnPayService)
		{

			_momoService = momoService;
			_vnPayService = vnPayService;

		}
		[HttpPost]
		public async Task<IActionResult> CreatePaymentMomo(OrderInfoModel model)
		{
			var response = await _momoService.CreatePaymentMomo(model);
			return Redirect(response.PayUrl);
		}
		[HttpPost]
		public IActionResult CreatePaymentUrlVnpay(PaymentInformationModel model)
		{
			var recipientName = Request.Form["RecipientName"].ToString();
			var recipientPhone = Request.Form["RecipientPhone"].ToString();
			var recipientAddress = Request.Form["RecipientAddress"].ToString();
			if (!string.IsNullOrEmpty(recipientName)) HttpContext.Session.SetString("RecipientName", recipientName);
			if (!string.IsNullOrEmpty(recipientPhone)) HttpContext.Session.SetString("RecipientPhone", recipientPhone);
			if (!string.IsNullOrEmpty(recipientAddress)) HttpContext.Session.SetString("RecipientAddress", recipientAddress);
			Console.WriteLine("===== VNPay PAYMENT PROCESS =====");
			Console.WriteLine("request method: " + Request.Method);
			Console.WriteLine("recipient:" + recipientName + ", " + recipientPhone + ", " + recipientAddress);
	
			var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

			return Redirect(url);
		}

		[HttpGet]
		public IActionResult PaymentCallBack()
		{
			var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
			return View(response);
		}
		

	}
}
