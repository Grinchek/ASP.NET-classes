using Microsoft.EntityFrameworkCore;
using _01_ASP_MVC.Models;
using _01_ASP_MVC.Data;
using _01_ASP_MVC.Repositories.Models;
using System;

namespace _01_ASP_MVC.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDBContext _context;

        public ProductRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Product model)
        {
            await _context.Products.AddAsync(model);
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

            _context.Products.Remove(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            return products;
        }

        public async Task<Product?> FindByIdAsync(string id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<bool> UpdateAsync(Product model)
        {
            _context.Products.Update(model);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public async Task<List<Product>> GetAllByCategoryAsync(string categoryId)
        {
            return await _context.Products
                .Where(p => p.CategiryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();
        }

    }
}
