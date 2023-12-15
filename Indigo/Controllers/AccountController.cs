using Indigo.Models;
using Indigo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Indigo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public async Task<IActionResult> Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            AppUser appUser = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
            };
            var resultEmail = await _userManager.FindByEmailAsync(registerVM.Email);
            if (resultEmail == null)
            {
                var result = await _userManager.CreateAsync(appUser, registerVM.Password);
                await _userManager.AddToRoleAsync(appUser, "Admin");

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(registerVM);
                }
                return RedirectToAction(nameof(Login));
            }
            else
            {
                ModelState.AddModelError("Email", "Please use another email.");
                return View();
            }
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "Username/Email or password is not valid!");
                    return View();
                }
            }
            var result = _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, true).Result;

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Please, try again after a while!");
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username/Email or password is not valid!");
            }
            await _signInManager.SignInAsync(user, loginVM.RememberMe);
            if (returnUrl != null)
            {
                return RedirectToAction(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
