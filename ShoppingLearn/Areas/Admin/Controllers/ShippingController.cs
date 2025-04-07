using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Models;
using ShoppingLearn.Repository;

namespace ShoppingLearn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ShippingController : Controller
	{
        private readonly DataContext _datacontext;
        public ShippingController(DataContext context)
        {
            _datacontext = context;

        }
        public async Task<IActionResult> Index()
		{
            var shippingList = await _datacontext.Shippings.ToListAsync();
            ViewBag.Shippings = shippingList;
            return View();
		}
        [HttpPost]
        [Route("StoreShipping")]

        public async Task<IActionResult> StoreShipping(ShippingModel shippingModel, string phuong, string quan, string tinh, decimal price)
        {

            shippingModel.City = tinh;
            shippingModel.District = quan;
            shippingModel.Ward = phuong;
            shippingModel.Price = price;

            try
            {

                var existingShipping = await _datacontext.Shippings
                    .AnyAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);

                if (existingShipping)
                {
                    return Ok(new { duplicate = true, message = "Dữ liệu trùng lặp." });
                }
                _datacontext.Shippings.Add(shippingModel);
                await _datacontext.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm shipping thành công" });
            }
            catch (Exception)
            {

                return StatusCode(500, "An error occurred while adding shipping.");
            }
        }
        public async Task<IActionResult> Delete(int Id)
        {
            ShippingModel shipping = await _datacontext.Shippings.FindAsync(Id);

            _datacontext.Shippings.Remove(shipping);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "Shipping đã được xóa thành công";
            return RedirectToAction("Index");
        }

    }
}
