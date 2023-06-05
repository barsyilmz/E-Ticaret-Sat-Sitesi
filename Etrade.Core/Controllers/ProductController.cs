using Etrade.Core.Models.Helper;
using Etrade.DAL.Abstract;
using Etrade.DAL.Context;
using Etrade.Entity.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Etrade.Core.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly EtradeDbContext db;
        private readonly IProductDAL productDAL;
        private readonly ICategoryDAL categoryDAL;
        public ProductController(IProductDAL productDAL, ICategoryDAL categoryDAL, EtradeDbContext dbContext)
        {
            this.productDAL = productDAL;
            this.categoryDAL = categoryDAL;
            this.db = dbContext;
        }
        public IActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).ToList();
            return View(products);
        }
        [Authorize(Roles = "Admin, Create")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(categoryDAL.GetAll(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product, [Bind("Image")] IFormFile image)
        {            
            if (!ModelState.IsValid)
            {
                if (image != null) 
                    product.Image=FileHelper.Add(image);
                productDAL.Add(product);
                return RedirectToAction("Index");
            }
            return View();
        }
        [Authorize(Roles = "Admin, Edit")]
        public IActionResult Edit(int id)
        {
            ViewData["CategoryId"] = new SelectList(categoryDAL.GetAll(), "Id", "Name");
            var product = productDAL.Get(id);
            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(Product product, [Bind("Image")] IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                if (image!=null)
                    product.Image = FileHelper.Update(product.Image, image);
                productDAL.Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        public IActionResult Details(int id)
        {
            var product = productDAL.Get(id);
            product.Category = categoryDAL.Get(product.CategoryId);
            return View(product);
        }
        [Authorize(Roles = "Admin, Delete")]
        public IActionResult Delete(int id) 
        {
            var product = productDAL.Get(id);
            product.Category = categoryDAL.Get(product.CategoryId);
            return View(product);
        }
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            FileHelper.Delete(product.Image);
            productDAL.Delete(product);
            return RedirectToAction("Index");
        }
    }
}
