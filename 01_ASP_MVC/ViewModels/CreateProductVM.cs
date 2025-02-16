using _01_ASP_MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _01_ASP_MVC.ViewModels
{
    public class CreateProductVM
    {
        public Product Product { get; set; } = new();
        public IEnumerable<SelectListItem> Categories { get; set; } = [];
        public IFormFile? File { get; set; }
    }
}