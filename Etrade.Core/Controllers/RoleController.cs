using Etrade.Entity.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Etrade.Core.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        public IActionResult Create() => View();
        [HttpPost]
        [Authorize(Roles = "Admin, Create")]
        public async Task<IActionResult> Create(Role model)
        {
            var role = await _roleManager.FindByNameAsync(model.Name);
            if(role == null) 
            {
                var result = await _roleManager.CreateAsync(model);
                if (result.Succeeded)
                    return RedirectToAction("Index");
            }
            return View(role);
        }
        [Authorize(Roles = "Admin, Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleManager.FindByIdAsync($"{id}");
            return View(role); 
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Role model)
        {
            var role = await _roleManager.FindByNameAsync($"{model.Id}");
            role.Name=model.Name;
            role.NormalizedName= model.NormalizedName;
            var result = await _roleManager.UpdateAsync(model);
            if(result.Succeeded)
                return RedirectToAction("Index");
            return View(model);
        }
        [Authorize(Roles = "Admin, Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleManager.FindByIdAsync($"{id}");
            if( role != null )
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}