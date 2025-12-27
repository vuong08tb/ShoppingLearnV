using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
		private readonly UserManager<AppUserModel> _userManager;
		public ProductController(DataContext context, UserManager<AppUserModel> userManager)
		{
			_datacontext = context;
			_userManager = userManager;
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
			// lấy sản phẩm liên quan
			var relatedProducts = await _datacontext.Products
			.Where(p =>p.CategoryId == productById.CategoryId && p.Id != productById.Id)
			.Take(4)
			.ToListAsync();
			ViewBag.RelatedProducts = relatedProducts;


			var ratingList = await _datacontext.Ratings
					.Where(r => r.ProductId == Id)
					.OrderByDescending(r => r.Id)
					.ToListAsync();

			ViewBag.IsUserLoggedIn = User.Identity.IsAuthenticated;

			var viewModel = new ProductDetailsViewModel
			{
				ProductDetails = productById,
				RatingDetails = new RatingModel { ProductId = productById.Id },
				RatingList = ratingList
			};

		


			return View(viewModel);
		}
		[HttpPost]
		[Authorize] 
		public async Task<IActionResult> AddRating(ProductDetailsViewModel model)
		{
			// Valid thủ công: star, nội dung
			if (model.RatingDetails.Comment == null) {
				return RedirectToAction("Details", new { Id = model.RatingDetails.ProductId });
			}

			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToAction("Login", "Account");

			var rating = new RatingModel
			{
				ProductId = model.RatingDetails.ProductId,
				Name = user.UserName,
				Email = user.Email,
				Star = model.RatingDetails.Star,
				Comment = model.RatingDetails.Comment
			};

			_datacontext.Ratings.Add(rating);
			await _datacontext.SaveChangesAsync();

			return RedirectToAction("Details", new { Id = model.RatingDetails.ProductId });
		}


	}
}
