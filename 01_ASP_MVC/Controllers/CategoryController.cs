using _01_ASP_MVC.Data;
using _01_ASP_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _01_ASP_MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDBContext _context;
        public CategoryController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.AsEnumerable();
            return View(categories);
        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            model.Id = Guid.NewGuid().ToString();

            _context.Categories.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
