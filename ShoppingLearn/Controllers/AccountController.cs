using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoppingLearn.Models.ViewModels;
using ShoppingLearn.Models;
using ShoppingLearn.Areas.Admin.Repository;
using ShoppingLearn.Repository;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ShoppingLearn.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManager;
		private SignInManager<AppUserModel> _signInManager;
        private readonly IEmailSender _emailSender;
		private readonly DataContext _dataContext;
		public AccountController(IEmailSender emailSender,SignInManager<AppUserModel> signInManager,
			UserManager<AppUserModel> userManager, DataContext context)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_emailSender = emailSender;
			_dataContext = context;
		}
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}
		// UpdateAccount
		[HttpGet]			
		public async Task<IActionResult> UpdateAccount()
		{
			if ((bool)!User.Identity?.IsAuthenticated)
			{
				// User is not logged in, redirect to login
				return RedirectToAction("Login", "Account"); // Replace "Account" with your controller name
			}
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userEmail = User.FindFirstValue(ClaimTypes.Email);
			// get user by user email 
			var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
			if(user == null)
			{
				return NotFound();
			}
			return View(user);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateInfoAccount(AppUserModel user)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userById = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);

			if (userById == null)
			{
				return NotFound();
			}

			// Update basic info
			userById.PhoneNumber = user.PhoneNumber;

			// Update AI recommendation preferences
			userById.Gender = user.Gender;
			userById.DateOfBirth = user.DateOfBirth;
			userById.PreferredStyle = user.PreferredStyle;
			userById.PreferredColors = user.PreferredColors;
			userById.SizePreference = user.SizePreference;
			userById.PriceRange = user.PriceRange;
			userById.Interests = user.Interests;

			_dataContext.Update(userById);
			await _dataContext.SaveChangesAsync();
			TempData["success"] = "Cập nhật thông tin tài khoản thành công!";

			return RedirectToAction("UpdateAccount", "Account");
		}

		// Change Password
		[HttpGet]
		public async Task<IActionResult> ChangePassword()
		{
			if ((bool)!User.Identity?.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
		{
			if ((bool)!User.Identity?.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null)
			{
				return NotFound();
			}

			// Validate new password and confirm password match
			if (newPassword != confirmPassword)
			{
				TempData["error"] = "Mật khẩu mới và xác nhận mật khẩu không khớp!";
				return View();
			}

			// Verify current password
			var passwordCheck = await _userManager.CheckPasswordAsync(user, currentPassword);
			if (!passwordCheck)
			{
				TempData["error"] = "Mật khẩu hiện tại không chính xác!";
				return View();
			}

			// Change password using UserManager
			var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

			if (result.Succeeded)
			{
				TempData["success"] = "Đổi mật khẩu thành công!";
				return RedirectToAction("UpdateAccount", "Account");
			}
			else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				TempData["error"] = "Đổi mật khẩu thất bại. Vui lòng kiểm tra lại!";
				return View();
			}
		}

		public async Task<IActionResult> NewPass(AppUserModel user, string token)
		{
			var checkuser = await _userManager.Users
				.Where(u => u.Email == user.Email)
				.Where(u => u.Token == user.Token).FirstOrDefaultAsync();

			if (checkuser != null)
			{
				ViewBag.Email = checkuser.Email;
				ViewBag.Token = token;
			}
			else
			{
				TempData["error"] = "Email not found or token is not right";
				return RedirectToAction("ForgetPass", "Account");
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> UpdateNewPassword(AppUserModel user, string token)
		{
			var checkuser = await _userManager.Users
				.Where(u => u.Email == user.Email)
				.Where(u => u.Token == user.Token).FirstOrDefaultAsync();

			if (checkuser != null)
			{
				//update user with new password and token
				string newtoken = Guid.NewGuid().ToString();
				// Hash the new password
				var passwordHasher = new PasswordHasher<AppUserModel>();
				var passwordHash = passwordHasher.HashPassword(checkuser, user.PasswordHash);

				checkuser.PasswordHash = passwordHash;


				checkuser.Token = newtoken;

				await _userManager.UpdateAsync(checkuser);
				TempData["success"] = "Password updated successfully.";
				return RedirectToAction("Login", "Account");
			}
			else
			{
				TempData["error"] = "Email not found or token is not right";
				return RedirectToAction("ForgetPass", "Account");
			}
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SendMailForgotPass(AppUserModel user)
		{
			var checkMail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

			if (checkMail == null)
			{
				TempData["error"] = "Email not found";
				return RedirectToAction("ForgetPass", "Account");
			}
			else
			{
				string token = Guid.NewGuid().ToString();
				//update token to user
				checkMail.Token = token;
				_dataContext.Update(checkMail);
				await _dataContext.SaveChangesAsync();
				var receiver = checkMail.Email;
				var subject = "Change password for user " + checkMail.Email;
				var message = "Click on link to change password " +
					"<a href='" + $"{Request.Scheme}://{Request.Host}/Account/NewPass?email=" + checkMail.Email + "&token=" + token + "'>";

				await _emailSender.SendEmailAsync(receiver, subject, message);
			}


			TempData["success"] = "An email has been sent to your registered email address with password reset instructions.";
			return RedirectToAction("ForgetPass", "Account");
		}
		public async Task<IActionResult> ForgetPass(string returnUrl)
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
				if (result.Succeeded)
				{
					return Redirect(loginVM.ReturnUrl ?? "/");
				}
				ModelState.AddModelError("", "UserName or Password bị sai");
			}
			return View(loginVM);
		}
		public IActionResult Create()
		{
			return View();
		}
		public async Task<IActionResult> History()
		{
			if ((bool)!User.Identity?.IsAuthenticated)
			{
				// User is not logged in, redirect to login
				return RedirectToAction("Login", "Account"); // Replace "Account" with your controller name
			}
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var userEmail = User.FindFirstValue(ClaimTypes.Email);

			var Orders = await _dataContext.Orders
				.Where(od => od.UserName == userEmail).OrderByDescending(od => od.Id).ToListAsync();
			ViewBag.UserEmail = userEmail;
			return View(Orders);
		}
        public async Task<IActionResult> CancelOrder(string ordercode)
        {
            if ((bool)!User.Identity?.IsAuthenticated)
            {
                // User is not logged in, redirect to login
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var order = await _dataContext.Orders.Where(o => o.Order_code == ordercode).FirstAsync();
                order.Status = 3;
                _dataContext.Update(order);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return BadRequest("An error occurred while canceling the order.");
            }


            return RedirectToAction("History", "Account");
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create(UserModel user)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email, RoleId = "3" };
		//		IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
		//		var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "User");
		//		if (result.Succeeded && addToRoleResult.Succeeded)
		//		{
		//			TempData["success"] = " Tạo user thành công";
		//			return Redirect("/account/login");

		//		}
		//		foreach (IdentityError error in result.Errors)
		//		{
		//			ModelState.AddModelError("", error.Description);
		//		}
		//	}

		//	return View(user);
		//}
		public async Task<IActionResult> Create(UserModel user)
		{
			if (ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel
				{
					UserName = user.Username,
					Email = user.Email
				};

				IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

				if (result.Succeeded)
				{
					var addToRoleResult = await _userManager.AddToRoleAsync(newUser, "User");
					if (addToRoleResult.Succeeded)
					{
						TempData["success"] = "Tạo user thành công";
						return Redirect("/account/login");
					}
					else
					{
						foreach (var error in addToRoleResult.Errors)
						{
							ModelState.AddModelError("", error.Description);
						}
					}
				}
				else
				{
					foreach (IdentityError error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}

			return View(user);
		}

		public async Task<IActionResult> Logout(string returnUrl = "/")
		{
			await _signInManager.SignOutAsync();
			return Redirect(returnUrl);
		}
	}
	
	
}
