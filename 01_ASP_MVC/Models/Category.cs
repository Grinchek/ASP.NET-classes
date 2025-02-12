using System.ComponentModel.DataAnnotations;

namespace _01_ASP_MVC.Models
{
    public class Category
    {
        [Key]
        public string? Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        public List<Product> Products { get; set; }= [];
    }
}
