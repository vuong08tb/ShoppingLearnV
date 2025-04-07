using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Models;
using ShoppingLearn.Repository;

namespace ShoppingLearn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
	{
		private readonly DataContext _datacontext;
		public CategoryController(DataContext context)
		{
			_datacontext = context;
			
		}
		public async Task<IActionResult> Index(int pg = 1)
		{
            List<CategoryModel> category = _datacontext.Categories.ToList(); // 33 item
            const int pageSize = 10; //10 items/trang

            if (pg < 1) //page < 1;
            {
                pg = 1; //page ==1
            }
            int recsCount = category.Count(); //33 items;

            var pager = new Paginate(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize; //(3 - 1) * 10; 

            //category.Skip(20).Take(10).ToList()

            var data = category.Skip(recSkip).Take(pager.PageSize).ToList();

            ViewBag.Pager = pager;

            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                // code them du lieu
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _datacontext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(category);
                }

                _datacontext.Add(category);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
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
            return View(category);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _datacontext.Categories.FindAsync(Id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                // code them du lieu
                category.Slug = category.Name.Replace(" ", "-");
                var slug = await _datacontext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Danh mục đã có trong database");
                    return View(category);
                }

                _datacontext.Update(category);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Cập nhật danh mục thành công";
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
            return View(category);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _datacontext.Categories.FindAsync(Id);
           
            _datacontext.Categories.Remove(category);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "Danh mục đã được xóa thành công";
            return RedirectToAction("Index");
        }
    }
}
