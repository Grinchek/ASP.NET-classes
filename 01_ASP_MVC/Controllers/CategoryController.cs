using _01_ASP_MVC.Data;
using _01_ASP_MVC.Models;
using _01_ASP_MVC.Repositories.Categories;
using _01_ASP_MVC.Repositories.Models;
using _01_ASP_MVC.Repositories.Products;
using _01_ASP_MVC.Services.Image;
using _01_ASP_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace _01_ASP_MVC.Controllers
{
    public class CategoryController
       (ICategoryRepository categoryRepository, AppDBContext context)
       : Controller
    {
        public IActionResult Index()
        {
            var categories = context.Categories.AsEnumerable();
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

            context.Categories.Add(model);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await categoryRepository.FindByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Category model)
        {

            if (model.Id == null)
                return NotFound();

            var res = await categoryRepository.DeleteAsync(model.Id);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await categoryRepository.FindByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CreateCategoryVM
            {

            };

            return View(viewModel);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [FromForm] CreateCategoryVM viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                    var category = await categoryRepository.FindByIdAsync(id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    category.Name = viewModel.Name;



                    context.Update(category);
                    await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
    }
}
