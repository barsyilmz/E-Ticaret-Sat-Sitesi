using Etrade.Entity.Models.Identity;
using Etrade.Entity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Etrade.Core.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users.ToList());
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync($"{id}");
            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded)
                return RedirectToAction("Index");
            return NotFound("idye sahip kullanıcı bulunamadı");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RoleAssign(int id)
        {
            var user = await _userManager.FindByIdAsync($"{id}");
            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleAssigns = new List<RoleAssignViewModel>();
            foreach (var role in roles)
            {
                roleAssigns.Add(new RoleAssignViewModel()
                {
                    HasAssign =userRoles.Contains(role.Name),
                    Id = role.Id,
                    Name = role.Name,
                    
                });
            }
            ViewBag.Username = user.UserName;
            return View(roleAssigns);
        }
        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> models, int id)
        {
            var user = await _userManager.FindByIdAsync($"{id}");
            foreach(var item in models)
            {
                if (item.HasAssign)
                    await _userManager.AddToRoleAsync(user, item.Name);
                else 
                    await _userManager.RemoveFromRoleAsync(user,item.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
