using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Models;
using ShoppingLearn.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShoppingLearn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _datacontext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _datacontext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brand).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name", product.BrandId);
            if (ModelState.IsValid)
            {
                // code them du lieu
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await _datacontext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(product);
                }

                if (product.ImageUpLoad != null)
                {
                    string upLoadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpLoad.FileName;
                    string filePath = Path.Combine(upLoadDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpLoad.CopyToAsync(fs);
                    fs.Close();
                    product.Image = imageName;
                }

                _datacontext.Add(product);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công ";
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
            return View(product);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _datacontext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id,ProductModel product)
        {
            var existed_product = _datacontext.Products.Find(product.Id); //tìm sp theo id product
            ViewBag.Categories = new SelectList(_datacontext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_datacontext.Brands, "Id", "Name", product.BrandId);
            if (ModelState.IsValid)
            {
                // code them du lieu
                product.Slug = product.Name.Replace(" ", "-");
                var slug = await _datacontext.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Sản phẩm đã có trong database");
                    return View(product);
                }

                if (product.ImageUpLoad != null)
                {
                   //upload new image 
                    string upLoadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpLoad.FileName;
                    string filePath = Path.Combine(upLoadDir, imageName);

                    // delete old picture
                    string oldfilePath = Path.Combine(upLoadDir, existed_product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldfilePath))
                        {
                            System.IO.File.Delete(oldfilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "An error occurred while deleting the product image.");
                    }

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpLoad.CopyToAsync(fs);
                    fs.Close();
                    existed_product.Image = imageName;

                }
                // Update other product properties
                existed_product.Name = product.Name;
                existed_product.Description = product.Description;
                existed_product.Price = product.Price;
                existed_product.CategoryId = product.CategoryId;
                existed_product.BrandId = product.BrandId;
                // ... other properties

                _datacontext.Update(existed_product);
                await _datacontext.SaveChangesAsync();
                TempData["success"] = "Cập nhật sản phẩm thành công";
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



            return View(product);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _datacontext.Products.FindAsync(Id);
            if (!string.Equals(product.Image, "noname.jpg"))
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath,"media/products");
                string oldfilePath = Path.Combine(uploadsDir, product.Image);
                if (System.IO.File.Exists(oldfilePath))
                {
                    System.IO.File.Delete(oldfilePath);
                }
            }
            _datacontext.Products.Remove(product);
            await _datacontext.SaveChangesAsync();
            TempData["success"] = "sản phẩm đã được xóa thành công";
            return RedirectToAction("Index");
        }
        // add more quan tity to products
        [Route("AddQuantity")]
        [HttpGet]
        public async Task<IActionResult> AddQuantity(int Id)
        {
            var productbyquantity = await _datacontext.ProductQuantities.Where(pq => pq.ProductId == Id).ToListAsync();
            ViewBag.ProductByQuantity = productbyquantity;
            ViewBag.Id = Id;
            return View();
        }

        [Route("StoreProductQuantity")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StoreProductQuantity(ProductQuantityModel productQuantityModel)
        {
            // Get the product to update
            var product = _datacontext.Products.Find(productQuantityModel.ProductId);

            if (product == null)
            {
                return NotFound(); // Handle product not found scenario
            }
            product.Quantity += productQuantityModel.Quantity;

            productQuantityModel.Quantity = productQuantityModel.Quantity;
            productQuantityModel.ProductId = productQuantityModel.ProductId;
            productQuantityModel.DateCreated = DateTime.Now;


            _datacontext.Add(productQuantityModel);
            _datacontext.SaveChangesAsync();
            TempData["success"] = "Thêm số lượng sản phẩm thành công";
            return RedirectToAction("AddQuantity", "Product", new { Id = productQuantityModel.ProductId });
        }
    }
}
