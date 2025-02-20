using _01_ASP_MVC.Data;
using _01_ASP_MVC.Models;
using _01_ASP_MVC.Repositories.Models;
using _01_ASP_MVC.Repositories.Products;
using _01_ASP_MVC.Services.Image;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace _01_ASP_MVC.Controllers
{
        public class HomeController
       (IProductRepository productRepository, AppDBContext context, IImageService imageService)
       : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        public async Task<IActionResult> IndexAsync()
        {
            var products = await productRepository.GetAllAsync();
            ViewBag.Categories = await context.Categories.ToListAsync(); // Передаємо категорії у ViewBag
            return View(products);
        }

        public async Task<IActionResult> Filtered(string categoryId)
        {
            var products = await productRepository.GetAllByCategoryAsync(categoryId);
            ViewBag.Categories = await context.Categories.ToListAsync(); // Передаємо категорії у ViewBag
            return View("Index", products);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await productRepository.FindByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
