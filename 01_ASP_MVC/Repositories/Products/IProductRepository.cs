using _01_ASP_MVC.Models;

namespace _01_ASP_MVC.Repositories.Models
{
    public interface IProductRepository
    {
        Task<bool> CreateAsync(Product model);
        Task<bool> UpdateAsync(Product model);
        Task<bool> DeleteAsync(string id);
        Task<List<Product>> GetAllByCategoryAsync(string categoryId);
        Task<Product?> FindByIdAsync(string id);
        Task<List<Product>> GetAllAsync();
    }
}
