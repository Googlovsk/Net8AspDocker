using NET8ASP.Models.Domain.Order;

namespace NET8ASP.Models.ViewModels
{
    public class OrderViewModel
    {
        public ICollection<Order> Orders { get; set; }
    }
}
