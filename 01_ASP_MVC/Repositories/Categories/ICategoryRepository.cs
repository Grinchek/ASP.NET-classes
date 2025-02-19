using _01_ASP_MVC.Models;

namespace _01_ASP_MVC.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<bool> CreateAsync(Category model);
        Task<bool> UpdateAsync(Category model);
        Task<bool> DeleteAsync(string id);
        Task<List<Category>> GetAllAsync();
        Task<Category?> FindByIdAsync(string id);
    }
}
