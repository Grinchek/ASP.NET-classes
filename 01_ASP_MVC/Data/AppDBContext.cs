using _01_ASP_MVC.Models;
using Microsoft.EntityFrameworkCore;
namespace _01_ASP_MVC.Data
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions options) 
            :base(options){}

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
