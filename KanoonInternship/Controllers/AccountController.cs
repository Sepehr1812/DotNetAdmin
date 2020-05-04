using KanoonInternship.Models;
using KanoonInternship.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KanoonInternship.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;

        public AccountController(UserManager<ApplicationUser> UserManager,
            SignInManager<ApplicationUser> SignInManager)
        {
            this.UserManager = UserManager;
            this.SignInManager = SignInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    ActiveState = 0,
                    IsAdmin = false,
                    IsBanned = false,
                    BanUntil = new DateTime(1970, 1, 1),
                };
                var Result = await UserManager.CreateAsync(User, model.Password);

                if (Result.Succeeded)
                {
                    await SignInManager.SignInAsync(User, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var Error in Result.Errors)
                    ModelState.AddModelError("", Error.Description);
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await UserManager.FindByNameAsync(model.UserName);
                if (User == null)
                {
                    ModelState.AddModelError("", "There is no Username like it. Please Register First.");
                }
                else
                {
                    if (User.ActiveState != 1) // check activation state
                    {
                        if (User.ActiveState == 0)
                            ModelState.AddModelError("", "Your account is not active yet. Call an Admin.");
                        else
                            ModelState.AddModelError("", "Your registration request has been rejected. Sorry.");

                        return View();
                    }
                    if (!await UserManager.CheckPasswordAsync(User, model.Password))
                    {
                        ModelState.AddModelError("", "Password is incorrect.");
                        return View(model);
                    }
                    else // user exists and password is correct
                    {
                        // Remember me part will add here - see: https://code-maze.com/authentication-aspnet-core-identity/
                        await SignInManager.SignInAsync(User, isPersistent: false);

                        // check if the user really banned
                        if (User.IsBanned && User.BanUntil < DateTime.Today)
                        {
                            User.IsBanned = false;
                            await UserManager.UpdateAsync(User);
                        }

                        // go to home page
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}