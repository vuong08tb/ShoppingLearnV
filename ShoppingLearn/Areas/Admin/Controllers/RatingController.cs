using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Repository;

namespace ShoppingLearn.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class RatingController : Controller
	{
		private readonly DataContext _datacontext;
		public RatingController(DataContext datacontext)
		{
			_datacontext = datacontext;
		}
		public async Task<IActionResult> Index()
		{
			// Lấy tất cả comment kèm sản phẩm
			var ratingList = await _datacontext.Ratings
			 .Include(r => r.Product)          // để hiển thị tên sản phẩm
			 .OrderByDescending(r => r.Id)    // mới nhất lên đầu
			 .ToListAsync();

			return View(ratingList);
		}
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var rating = await _datacontext.Ratings.FindAsync(id);
			if (rating == null)
			{
				return NotFound();
			}

			_datacontext.Ratings.Remove(rating);
			await _datacontext.SaveChangesAsync();
			TempData["success"] = "Thương hiệu đã được xóa thành công";
			return RedirectToAction("Index");
		}

	}
}
