using NET8ASP.Models.Domain;

namespace NET8ASP.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public string PageName { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public int? CurrentCategory {  get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
