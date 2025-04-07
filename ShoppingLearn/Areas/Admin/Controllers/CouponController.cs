using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Models;
using ShoppingLearn.Repository;

namespace ShoppingLearn.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Route("Admin/Coupon")]
    [Authorize(Roles = "Admin")]
	public class CouponController : Controller
	{
		
		private readonly DataContext _datacontext;
		public CouponController(DataContext context)
		{
			_datacontext = context;

		}
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var coupon_list = await _datacontext.Coupons.ToListAsync();
            ViewBag.Coupons = coupon_list;
            return View();
        }
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CouponModel coupon)
        {


            if (ModelState.IsValid)
            {

                _datacontext.Add(coupon);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Thêm coupon thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            return View();
        }
    }
}
