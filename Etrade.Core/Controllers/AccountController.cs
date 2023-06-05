using Etrade.Entity.Models.Identity;
using Etrade.Entity.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Etrade.Core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult SignUp()
        {
            if (User.Identity.Name == null)
                return View();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            var user = new User(model.UserName)
            {
                Name = model.Name,
                SurName = model.SurName,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.Phone
            };
            var result = await _userManager.CreateAsync(user, user.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("SignIn");
            }
            return View(model);
        }

        public IActionResult SignIn()
        {
            if (User.Identity.Name == null)
                return View();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            User user;
            if (model.UserNameOrEmail.Contains("@"))
                user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
            else
                user = await _userManager.FindByNameAsync(model.UserNameOrEmail);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Profile(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync($"{id}");
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }

            }
            return RedirectToAction("Profile");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync($"{id}");
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {

            var result = await _userManager.UpdateAsync(model);
            if (result.Succeeded)
                return RedirectToAction("Profile");
            return View(model);

        }
    }
}
