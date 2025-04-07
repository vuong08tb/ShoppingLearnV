using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingLearn.Repository;

namespace ShoppingLearn.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/Dashboard")]
	//[Authorize]
	public class DashboardController : Controller
	{

		private readonly DataContext _datacontext;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public DashboardController(DataContext context, IWebHostEnvironment webHostEnvironment)
		{
			_datacontext = context;
			_webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			var count_product = _datacontext.Products.Count();
			var count_order = _datacontext.Orders.Count();
			var count_category = _datacontext.Categories.Count();
			var count_user = _datacontext.Users.Count();
			ViewBag.CountProduct = count_product;
			ViewBag.CountOrder = count_order;
			ViewBag.CountCategory = count_category;
			ViewBag.CountUser = count_user;
			return View();
		}
		[HttpPost]
		[Route("GetChartData")]
		public async Task<IActionResult> GetChartData()
		{
			var data = _datacontext.Statisticals
				.Select(x => new
				{
					date = x.DateCreated.ToString("yyyy-MM-dd"),
					sold = x.Sold,
					quantity = x.Quantity,
					revenue = x.Revenue,
					profit = x.Profit,


				})
				.ToList();
			return Json(data);
		}
		[HttpPost]
		[Route("GetChartDataBySelect")]
		public IActionResult GetChartDataBySelect(DateTime startDate, DateTime endDate)
		{
			var data = _datacontext.Statisticals
				.Where(x => x.DateCreated >= startDate && x.DateCreated <= endDate)
				.Select(x => new
				{
					date = x.DateCreated.ToString("yyyy-MM-dd"),
					sold = x.Sold,
					quantity = x.Quantity,
					revenue = x.Revenue,
					profit = x.Profit,


				})
				.ToList();
			return Json(data);
		}

	}
}
