using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Repository;

namespace ShoppingLearn.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Route("Admin/Order")]
    [Authorize]
	public class OrderController : Controller
	{
		
		private readonly DataContext _datacontext;
		public OrderController(DataContext context)
		{
			_datacontext = context;

		}
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
		{
			return View(await _datacontext.Orders.OrderByDescending(p => p.Id).ToListAsync());
		}
        [HttpGet]
        [Route("ViewOrder")]
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
			
			var DetailsOrder = await _datacontext.OrderDetails.Include(od =>od.Product).Where(od=>od.OrderCode==ordercode).ToListAsync();
			// ;ấy shingpp cost
			var Order = _datacontext.Orders.Where(o => o.Order_code == ordercode).First();
			ViewBag.ShippingCost = Order.ShippingCost;	
			ViewBag.Status = Order.Status;

            return View(DetailsOrder);
        }
        [HttpPost]
        [Route("UpdateOrder")]
		public async Task<IActionResult> UpdateOrder(string ordercode, int status)
		{
			var order = await _datacontext.Orders.FirstOrDefaultAsync(o => o.Order_code == ordercode);
			if (order == null)
			{
				return NotFound();
			}
			order.Status = status;
			try
			{
				await _datacontext.SaveChangesAsync();
				return Ok(new { success = true, message = " Order status update successfully"} );
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while updating the order status");
			}
		}
        [HttpGet]
        [Route("Delete")]
		public async Task<IActionResult> Delete(string ordercode)
		{
            var order = await _datacontext.Orders.FirstOrDefaultAsync(o => o.Order_code == ordercode);
            if (order == null)
            {
                return NotFound();
            }
            try
            {
				// delete order 
				_datacontext.Orders.Remove(order);
                await _datacontext.SaveChangesAsync();
               return RedirectToAction("Index");	
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the order status");
            }
        }
    }
}
