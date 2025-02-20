using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _01_ASP_MVC.Data;
using _01_ASP_MVC.Models;
using _01_ASP_MVC.ViewModels;
using _01_ASP_MVC.Repositories.Models;
using _01_ASP_MVC.Services.Image;

namespace _01_ASP_MVC.Controllers
{
    public class ProductsController
       (IProductRepository productRepository, AppDBContext context, IImageService imageService)
       : Controller
    {

        // GET: Products
        public async Task<IActionResult> IndexAsync()
        {
            var products = await productRepository.GetAllAsync();

            return View(products);
        }
      

        // GET: Products/Details/5
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

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories = context.Categories.AsEnumerable();

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
        public async Task<IActionResult> Create([FromForm] CreateProductVM viewModel)
        {
            string? imagePath = null;

            if (viewModel.File != null)
            {
                imagePath = await imageService.SaveImageAsync(viewModel.File, Settings.ProductsImagesPath);
            }

            viewModel.Product.Image = imagePath;
            viewModel.Product.Id = Guid.NewGuid().ToString();
            context.Products.Add(viewModel.Product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
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

            var categories = context.Categories.AsEnumerable();

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
                    var product = await productRepository.FindByIdAsync(id);
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
                        product.Image = await imageService.SaveImageAsync(viewModel.File, Settings.ProductsImagesPath);
                    }

                    context.Update(product);
                    await context.SaveChangesAsync();
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

            var product = await productRepository.FindByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Product model)
        {
            string? imageName = model.Image;

            if (model.Id == null)
                return NotFound();

            var res = await productRepository.DeleteAsync(model.Id);

            if (res && imageName != null)
            {
                imageService.DeleteFile(Path.Combine(Settings.ProductsImagesPath, imageName));
            }

            return RedirectToAction("Index");
        }

        private bool ProductExists(string id)
        {
            return context.Products.Any(e => e.Id == id);
        }
    }
}