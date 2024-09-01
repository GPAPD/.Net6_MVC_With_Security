using ABC_Company.Models;
using Domain.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace ABC_Company.Controllers
{
	public class LogInController : BaseController
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LogInController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
		{
            LogInModel model = new LogInModel();
            model.ErrorMsgList = new List<string>();

            return View(model);
		}

		public async Task<IActionResult> AuthUser(LogInModel content) 
		{
            content.ErrorMsgList = new List<string>();

            try {
                var user = await _userManager.FindByNameAsync(content.UserName);

                if (user != null && await _userManager.CheckPasswordAsync(user, content.Password))
                {
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = content.RememberMe,
                        ExpiresUtc = content.RememberMe ? DateTime.UtcNow.AddDays(14) : (DateTime?)null

                    };

                    await _signInManager.SignInAsync(user, authProperties);
                    return RedirectToAction("Index", "Home");
                }
                else 
                {
                    content.ErrorMsgList.Add("Invalid username or password");
                
                }
            } 
            catch(Exception e) 
            {
                //return RedirectToAction("Index", "Home");
            }
            
            return View("Index",content);
		}
		public async Task<IActionResult> LogOut()
		{
			// Sign out the user
			await _signInManager.SignOutAsync();

			// Optionally clear any other authentication-related cookies or session data
			// HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			// Redirect to the home page or a login page
			return RedirectToAction("Index", "Home");
		}

		public IActionResult Register ()
        {
            var model = new RegisterModel();
            model.ErrorMsgList = new List<string>();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterNewUser(RegisterModel model)
        {
            //string[] roleNames = { "Admin", "User", "Manager" };
            //foreach (var roleName in roleNames)
            //{
            //    // Check if the role already exists
            //    var roleExists = await _roleManager.RoleExistsAsync(roleName);

            //    if (!roleExists)
            //    {
            //        // Create the role if it doesn't exist
            //        await _roleManager.CreateAsync(new IdentityRole(roleName));
            //    }
            //}
            //if (model.Password.Length < 8) 
            //{
            //    return View("Register", model);
            //}
            //if (model.Password.Length < 8)
            //{
            //    return View("Register", model);
            //}

            model.ErrorMsgList = new List<string>();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName    
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //var user = await _userManager.FindByIdAsync(userId);

                    if (user == null)
                    {
                        return NotFound(); // User not found
                    }

                    var statues = await _userManager.AddToRoleAsync(user, "User");

                    if (statues.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home"); // Redirect to the home page after successful registration
                }
                else 
                {
                    foreach (var error in result.Errors)
                    {
                        //ModelState.AddModelError(string.Empty, error.Description);

                        model.ErrorMsgList.Add(error.Description);
                       
                    }
                    return View("Register", model);
                }
            }

            return View(model); // If model is invalid, return the form with validation messages
        }
    }
}

