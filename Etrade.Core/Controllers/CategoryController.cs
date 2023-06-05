using Etrade.DAL.Abstract;
using Etrade.DAL.Context;
using Etrade.Entity.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Etrade.Core.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryDAL categoryDAL;
        
        public CategoryController(EtradeDbContext db, ICategoryDAL categoryDAL)
        {
            this.categoryDAL = categoryDAL;
        }

        public IActionResult Index() => View(categoryDAL.GetAll());
        [Authorize(Roles = "Admin, Create")]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                categoryDAL.Add(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [Authorize(Roles = "Admin, Edit")]
        public IActionResult Edit(int id) => View(categoryDAL.Get(id));

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            var model = categoryDAL.Get(category.Id);
            model.Name = category.Name;
            model.Description = category.Description;

            if (ModelState.IsValid)
            {
                categoryDAL.Update(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Details(int id) => View(categoryDAL.Get(id));

        [Authorize(Roles = "Admin, Delete")]
        public IActionResult Delete(int id) => View(categoryDAL.Get(id));

        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(Category category)
        {
            categoryDAL.Delete(category);
            return RedirectToAction("Index");
        }
    }
}
