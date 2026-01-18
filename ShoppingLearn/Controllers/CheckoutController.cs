using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ShoppingLearn.Areas.Admin.Repository;
using ShoppingLearn.Models;
using ShoppingLearn.Repository;
using ShoppingLearn.Services.Momo;
using ShoppingLearn.Services.Vnpay;
using System;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace ShoppingLearn.Controllers
{
	public class CheckoutController : Controller
	{
		private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        private readonly IMomoService _moMoService;
		private readonly IVnPayService _vnPayService;
		private static readonly HttpClient client = new HttpClient();
        public CheckoutController(IEmailSender emailSender,DataContext context,IMomoService momoService, IVnPayService vnPayService)
		{
			_dataContext = context;
			_emailSender = emailSender;
			_moMoService = momoService;
			_vnPayService = vnPayService;
		}
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Checkout(string PaymentMethod,string PaymentId)
		{
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			if(userEmail == null)
			{
				return RedirectToAction("Login","Account");
			}
			else
			{
				Console.WriteLine("===== CHECKOUT PROCESS =====");

				Console.WriteLine("request method: " + Request.Method);

				// Try to read recipient info from session first (set by payment step).
				string recipientName = HttpContext.Session.GetString("RecipientName");
				string recipientPhone = HttpContext.Session.GetString("RecipientPhone");
				string recipientAddress = HttpContext.Session.GetString("RecipientAddress");

				Console.WriteLine("---- RECIPIENT FROM SESSION ----");
				Console.WriteLine($"RecipientName(session) = {recipientName}");
				Console.WriteLine($"RecipientPhone(session) = {recipientPhone}");
				Console.WriteLine($"RecipientAddress(session) = {recipientAddress}");

				// Fallback to form data if any value is missing
				if (string.IsNullOrEmpty(recipientName) || string.IsNullOrEmpty(recipientPhone) || string.IsNullOrEmpty(recipientAddress))
				{
					Console.WriteLine("---- FORM DATA (fallback) ----");
					foreach (var key in Request.Form.Keys)
					{
						var value = Request.Form[key].ToString();
						Console.WriteLine($"{key} = {value}");

						if (key == "RecipientName" && string.IsNullOrEmpty(recipientName))
							recipientName = value;

						if (key == "RecipientPhone" && string.IsNullOrEmpty(recipientPhone))
							recipientPhone = value;

						if (key == "RecipientAddress" && string.IsNullOrEmpty(recipientAddress))
							recipientAddress = value;
					}
				}

				var ordercode = Guid.NewGuid().ToString();
				var orderItem = new OrderModel();
				orderItem.Order_code = ordercode;
				orderItem.Receiver = recipientName;
				orderItem.PhoneReceiver = recipientPhone;
				orderItem.ShippingAddress = recipientAddress;
				// nhận shipping từ cookie
				var shippingPriceCookie = Request.Cookies["ShippingPrice"];
				decimal shippingPrice = 0;

				if (shippingPriceCookie != null)
				{
					var shippingPriceJson = shippingPriceCookie;
					shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
				}
				else
				{
					shippingPrice = 0;
				} 
				
				// nhận coupon từ cookie
				var coupon_code = Request.Cookies["CouponTitle"];
				orderItem.ShippingCost = shippingPrice;
				orderItem.CouponCode = coupon_code;
				orderItem.UserName = userEmail;
				if(PaymentMethod == "VnPay")
				{
					orderItem.PaymentMethod = "VnPay" + " " + PaymentId;
				}
				else {
					if(PaymentMethod == "Momo"){
						orderItem.PaymentMethod = "Momo" + " " + PaymentId;
					} else {
					orderItem.PaymentMethod = "COD";
					}
				}
				// orderItem.PaymentMethod = PaymentMethod + " " + PaymentId;
				orderItem.Status = 1;
				orderItem.CreatedDate = DateTime.Now;
				_dataContext.Add(orderItem);
				_dataContext.SaveChanges();
				List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart")
				 ?? new List<CartItemModel>();
				foreach (var cart in cartItems)
				{
					var orderdetails = new OrderDetails();
					orderdetails.UserName = userEmail;
					orderdetails.OrderCode = ordercode;
					orderdetails.ProductId =cart.ProductId;
					orderdetails.Price = cart.Price;
					orderdetails.Quantity = cart.Quantity;
					// update product quantity
					var product = await _dataContext.Products.Where(p => p.Id == cart.ProductId).FirstAsync();
					product.Quantity -= cart.Quantity;
					product.Sold += cart.Quantity;
					_dataContext.Update(product);

					// 
					_dataContext.Add(orderdetails);
					_dataContext.SaveChanges();
				}
				HttpContext.Session.Remove("Cart");
				// Send mail order  when success

				var receiver =userEmail;
                var subject = "Đặt hàng thành công";
				var message = "Đặt hàng thành công, trải nghiệm dịch vụ nhé.";

				await _emailSender.SendEmailAsync(receiver, subject, message);
				TempData["Success"] = "Đơn hàng tạo thành công, vui lòng chờ duyệt đơn";
				return RedirectToAction("History", "Account");
			}
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> PaymentCallBack(MomoInfoModel model)
		{
			var response = _moMoService.PaymentExecuteAsync(HttpContext.Request.Query);
			var requestQuery = HttpContext.Request.Query;
			if (requestQuery["resultCode"] != 0) // giao dịch không thành công thì lưu
			{
				var newMomoInsert = new MomoInfoModel
				{
					OrderId = requestQuery["orderId"],
					FullName = User.FindFirstValue(ClaimTypes.Email),
					Amount = decimal.Parse(requestQuery["Amount"]),
					OrderInfo = requestQuery["orderInfo"],
					DatePaid = DateTime.Now,
				};
				_dataContext.Add(newMomoInsert);
				await _dataContext.SaveChangesAsync();
				// 
				var PaymentMethod = "Momo";
				await Checkout(PaymentMethod, requestQuery["orderId"]);
			}
            else
			{
				TempData["success"] = " Giao dịch Momo không thành công";
				return RedirectToAction("Index", "Cart");
			}
			
			return View(response);
		}
		[HttpGet]
		public async  Task<IActionResult> PaymentCallbackVnpay()
		{
			var response = _vnPayService.PaymentExecute(Request.Query);
			if (response.VnPayResponseCode == "00") // giao dịch  thành công thì lưu
			{
				var newVnpayInsert = new VnpayModel
				{
					OrderId = response.OrderId,
					PaymentMethod = response.PaymentMethod,
					OrderDescription = response.OrderDescription,
					TransactionId = response.TransactionId,
					PaymentId = response.PaymentId,
					DateCreated = DateTime.Now
				};
				_dataContext.Add(newVnpayInsert);
				await _dataContext.SaveChangesAsync();
				// 
				var PaymentMethod =response.PaymentMethod;
				var PaymentId = response.PaymentId;
				 await Checkout(PaymentMethod,PaymentId);
			}
			else
			{
				TempData["success"] = " Giao dịch Vnpay không thành công";
				return RedirectToAction("Index", "Cart");
			}
			return View(response);
		}
	}
}
