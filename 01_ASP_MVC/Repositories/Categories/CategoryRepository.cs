using _01_ASP_MVC.Data;
using _01_ASP_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace _01_ASP_MVC.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDBContext _context;

        public CategoryRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Category model)
        {
            await _context.Categories.AddAsync(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var model = await FindByIdAsync(id);
            if (model == null)
            {
                return false;
            }

            _context.Categories.Remove(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var category = await _context.Categories.ToListAsync();

            return category;
        }

        public async Task<Category?> FindByIdAsync(string id)
        {
            var product = await _context.Categories.FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<bool> UpdateAsync(Category model)
        {
            _context.Categories.Update(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

    }
}
