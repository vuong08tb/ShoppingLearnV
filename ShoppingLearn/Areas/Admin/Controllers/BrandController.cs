using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Models; 
using ShoppingLearn.Repository;

namespace ShoppingLearn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly DataContext _datacontext;
        public BrandController(DataContext context)
        {
            _datacontext = context;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _datacontext.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                // code them du lieu
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _datacontext.Categories.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã có trong database");
                    return View(brand);
                }

                _datacontext.Add(brand);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model có vài thứ đang bị lỗi ";
                List<string> errorrs = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errorrs.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errorrs);
                return BadRequest(errorMessage);
            }
            return View(brand);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            BrandModel brand = await _datacontext.Brands.FindAsync(Id);
            return View(brand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BrandModel brand)
        {
            if (ModelState.IsValid)
            {
                // code them du lieu
                brand.Slug = brand.Name.Replace(" ", "-");
                var slug = await _datacontext.Categories.FirstOrDefaultAsync(p => p.Slug == brand.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Thương hiệu đã có trong database");
                    return View(brand);
                }

                _datacontext.Update(brand);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thương hiệu thành công";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["error"] = "Model có vài thứ đang bị lỗi ";
                List<string> errorrs = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errorrs.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errorrs);
                return BadRequest(errorMessage);
            }
            return View(brand);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            BrandModel brand = await _datacontext.Brands.FindAsync(Id);

            _datacontext.Brands.Remove(brand);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "Thương hiệu đã được xóa thành công";
            return RedirectToAction("Index");
        }

    }
}
