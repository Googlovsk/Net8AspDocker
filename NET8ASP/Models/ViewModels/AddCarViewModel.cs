using NET8ASP.Models.Domain;

namespace NET8ASP.Models.ViewModels
{
    public class AddCarViewModel
    {
        public Car Car { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
