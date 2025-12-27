using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingLearn.Models;
using ShoppingLearn.Repository;
using ShoppingLearn.Resquest;
using System.Data;
using System.Numerics;

namespace ShoppingLearn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager, DataContext dataContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dataContext = dataContext;
        }
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var userWithRoles = await (from u in _dataContext.Users
                                       join ur in _dataContext.UserRoles on u.Id equals ur.UserId
                                       join r in _dataContext.Roles on ur.RoleId equals r.Id
                                       select new { User = u, RoleName = r.Name }).ToListAsync();

            foreach (var item in userWithRoles)
            {
                Console.WriteLine($"Username: {item.User.UserName}");
            }

            return View(userWithRoles);
        }
		[HttpGet]
		[Route("Create")]
		public async Task<IActionResult> Create()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(roles,"Id","Name");

			return View(new CreatUserRequest());
		}
        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user); // trả về List<string> (RoleName)
            var currentRole = roles.FirstOrDefault(r => r.Name == userRoles.FirstOrDefault());
 
            ViewBag.Roles = new SelectList(roles, "Id", "Name", currentRole.Id);

            UpdateUserRequest editUser = new UpdateUserRequest();
            editUser.Occupation = user.Occupation;
            editUser.UserName = user.UserName;
            editUser.NormalizedUserName = user.NormalizedUserName;
            editUser.Email = user.Email;
            editUser.NormalizedEmail = user.NormalizedEmail;
            editUser.EmailConfirmed = user.EmailConfirmed;
            editUser.PasswordHash = user.PasswordHash;
            editUser.SecurityStamp = user.SecurityStamp;
            editUser.ConcurrencyStamp = user.ConcurrencyStamp;
            editUser.PhoneNumber = user.PhoneNumber;
            editUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            editUser.TwoFactorEnabled = user.TwoFactorEnabled;
            editUser.LockoutEnabled = user.LockoutEnabled;
            editUser.LockoutEnd = user.LockoutEnd;
            editUser.AccessFailedCount = user.AccessFailedCount;
            editUser.Token = user.Token;

            return View(editUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id, UpdateUserRequest user)
        {
            var existingUser = await _userManager.FindByIdAsync(id); // lấy user dựa vào id 
            if (existingUser == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;

                // Sửa các thông tin chung của tài khoản ở bảng AspNetUsers
                var updateUserResult = await _userManager.UpdateAsync(existingUser);
                if (updateUserResult.Succeeded)
                {
                    // Lấy danh sách role của user hiện tại 
                    var currentRoles = await _userManager.GetRolesAsync(existingUser);

                    // Xóa role cũ của user
                    await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);

                    // Thêm role mới của user
                    var role = _roleManager.FindByIdAsync(user.RoleId);
                    await _userManager.AddToRoleAsync(existingUser, role.Result.Name);

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    AddIdentityErrors(updateUserResult);
                    return View(user);
                }
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            // // Model validation failed
            TempData["error"] = "Model validation failed";
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage).ToList());
            var errorMessage = string.Join("\n", errors);

            return View(existingUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(CreatUserRequest userRequest)
        {
            if(ModelState.IsValid)
			{
                AppUserModel newUser = new AppUserModel();
                newUser.Occupation = userRequest.Occupation;
                newUser.UserName = userRequest.UserName;
                newUser.NormalizedUserName = userRequest.NormalizedUserName;
                newUser.Email = userRequest.Email;
                newUser.NormalizedEmail = userRequest.NormalizedEmail;
                newUser.EmailConfirmed = userRequest.EmailConfirmed;
                newUser.PasswordHash = userRequest.PasswordHash;
                newUser.SecurityStamp = userRequest.SecurityStamp;
                newUser.ConcurrencyStamp = userRequest.ConcurrencyStamp;
                newUser.PhoneNumber = userRequest.PhoneNumber;
                newUser.PhoneNumberConfirmed = userRequest.PhoneNumberConfirmed;
                newUser.TwoFactorEnabled = userRequest.TwoFactorEnabled;
                newUser.LockoutEnabled = userRequest.LockoutEnabled;
                newUser.LockoutEnd = userRequest.LockoutEnd;
                newUser.AccessFailedCount = userRequest.AccessFailedCount;
                newUser.Token = userRequest.Token;

                var createUserResult = await _userManager.CreateAsync(newUser, newUser.PasswordHash); // tạo user 
                if(createUserResult.Succeeded)
                {
                    var createUser = await _userManager.FindByEmailAsync(newUser.Email); // tìm user dựa vào email
                    var userId = createUser.Id;
                    var role = _roleManager.FindByIdAsync(userRequest.RoleId); // lấy roleId
                    // gán quyền 
                    var addToRoleResult = await _userManager.AddToRoleAsync(createUser, role.Result.Name);
                    if(!addToRoleResult.Succeeded)
                    {
                        AddIdentityErrors(createUserResult);

                    }
                    return RedirectToAction("Index", "User");

                }
                else
                {
                    AddIdentityErrors(createUserResult);
                    return View(newUser);
                }

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

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(userRequest);
        }
        private void AddIdentityErrors(IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
          if(string.IsNullOrEmpty(id))
           {
                return NotFound();
           }
          var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                {
                return NotFound();
            }   
            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                return View("Error");
            }
            TempData["success"] = "Xóa User thành công";
            return RedirectToAction("Index");

        }

    }
}
