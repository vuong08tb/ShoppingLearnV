using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Models;
using ShoppingLearn.Models.ViewModels;
using ShoppingLearn.Repository;

namespace ShoppingLearn.Controllers
{
    public class ProductController : Controller
    {
		private readonly DataContext _datacontext;
		public ProductController(DataContext context)
		{
			_datacontext = context;
		}
		public IActionResult Index()
        {
            return View();

        }
        public async Task<IActionResult> Search(string searchTerm)
        {
			//var products = await _datacontext.Products
			//.Where(p =>p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToListAsync();
			//ViewBag.Keyword = searchTerm;
			//return View(products);
			if (string.IsNullOrWhiteSpace(searchTerm))
			{
				return RedirectToAction("Index","Home");
			}

			searchTerm = searchTerm.Trim().ToLower();

			var products = await _datacontext.Products
				.Where(p => EF.Functions.Like(p.Name.ToLower(), $"%{searchTerm}%") ||
							EF.Functions.Like(p.Description.ToLower(), $"%{searchTerm}%"))
				.OrderBy(p => p.Name.ToLower().StartsWith(searchTerm) ? 0 : 1) // Xếp ưu tiên gần đúng
				.ToListAsync();

			ViewBag.Keyword = searchTerm;
			return View(products);
		}
        public async Task<IActionResult>  Details(int Id)
        {
           if(Id == null) return RedirectToAction("Index");
			var productById = _datacontext.Products
				.Where(p => p.Id == Id).FirstOrDefault();
			var relatedProducts = await _datacontext.Products
			.Where(p =>p.CategoryId == productById.CategoryId && p.Id != productById.Id)
			.Take(4)
			.ToListAsync();
			
			ViewBag.RelatedProducts = relatedProducts;

			var viewModel = new ProductDetailsViewModel
			{
				ProductDetails = productById

			};


			return View(viewModel);
        }
	
    }
}
