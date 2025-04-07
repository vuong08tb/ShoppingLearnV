using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Models;
using ShoppingLearn.Repository;

namespace ShoppingLearn.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _datacontext;
		public BrandController(DataContext context)
		{
			_datacontext = context;
		}
		public async Task<IActionResult> Index(string Slug = "")
		{
			BrandModel brand = _datacontext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
			if (brand == null) return RedirectToAction("Index");
			var productByBrand = _datacontext.Products.Where(p => p.BrandId == brand.Id);
			return View(await productByBrand.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}
