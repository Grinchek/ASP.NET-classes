using _01_ASP_MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace _01_ASP_MVC.ViewModels
{
    public class CreateCategoryVM
    {
        public string? Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
    }
}
