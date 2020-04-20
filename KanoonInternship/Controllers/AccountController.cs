using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KanoonInternship.Models;
using KanoonInternship.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register(RegisterViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var User = new ApplicationUser
                {
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    UserName = Model.UserName,
                    ActiveState = 0,
                    IsAdmin = false,
                    IsBanned = false,
                    BanUntil = new DateTime(1970, 1, 1),
                };
                var Result = await UserManager.CreateAsync(User, Model.Password);

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
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var User = await UserManager.FindByNameAsync(Model.UserName);
                if (User == null)
                {
                    // show "There is no such user name
                    return RedirectToAction("Register", "Account");
                }
                else
                {
                    if (!await UserManager.CheckPasswordAsync(User, Model.Password))
                    {
                        // show incorrect password message
                        return View(Model);
                    }
                    else // user exists and password is correct
                    {
                        // TODO: determain if the user is rejected or waiting

                        // Remember me part will add here - see: https://code-maze.com/authentication-aspnet-core-identity/
                        await SignInManager.SignInAsync(User, isPersistent: false);

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