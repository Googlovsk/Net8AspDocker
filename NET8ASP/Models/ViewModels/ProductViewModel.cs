using Microsoft.AspNetCore.Mvc.Rendering;
using NET8ASP.Models.Domain;

namespace NET8ASP.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public ICollection<Product> Products {  get; set; } 
        public ICollection<Category> Categories { get; set; }
        public ICollection<Category> SubCategories { get; set; }
        public ICollection<Manufacturer> Manufacturers { get; set; }
        public int? SubCategoryId { get; set; }
        public SelectList SelectListCategories { get; set; }
    }
}
