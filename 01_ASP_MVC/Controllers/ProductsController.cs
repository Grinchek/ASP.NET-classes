using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _01_ASP_MVC.Data;
using _01_ASP_MVC.Models;
using Microsoft.Extensions.Hosting;
using _01_ASP_MVC.ViewModels;

namespace _01_ASP_MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(AppDBContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var appDBContext = _context.Products.Include(p => p.Category);
            return View(await appDBContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories = _context.Categories.AsEnumerable();

            var viewModel = new CreateProductVM
            {
                Product = new Product(),
                Categories = categories.Select(c =>
                new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                })
            };

            return View(viewModel);
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create([FromForm] CreateProductVM viewModel)
        {
            string? imagePath = null;

            if (viewModel.File != null)
            {
                imagePath = SaveImage(viewModel.File);
            }

            viewModel.Product.Image = imagePath;
            viewModel.Product.Id = Guid.NewGuid().ToString();
            _context.Products.Add(viewModel.Product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _context.Categories.AsEnumerable();

            var viewModel = new CreateProductVM
            {
                Product = product,
                Categories = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id
                })
            };

            return View(viewModel);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [FromForm] CreateProductVM viewModel)
        {
            if (id != viewModel.Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.FindAsync(id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    product.Name = viewModel.Product.Name;
                    product.Description = viewModel.Product.Description;
                    product.Price = viewModel.Product.Price;
                    product.Amount = viewModel.Product.Amount;
                    product.CategiryId = viewModel.Product.CategiryId;

                    if (viewModel.File != null)
                    {
                        product.Image = SaveImage(viewModel.File);
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(viewModel.Product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        private string? SaveImage(IFormFile file)
        {
            var types = file.ContentType.Split("/");
            if (types[0] != "image")
            {
                return null;
            }

            string fileName = $"{Guid.NewGuid()}.{types[1]}";
            string imagesPath = Path.Combine(_environment.WebRootPath, "images", "products");
            string filePath = Path.Combine(imagesPath, fileName);
            using (var stream = file.OpenReadStream())
            {
                using (var fileStream = System.IO.File.Create(filePath))
                {
                    stream.CopyTo(fileStream);
                }
            }

            return fileName;
        }
    }
}
