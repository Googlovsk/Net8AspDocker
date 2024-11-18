using Microsoft.AspNetCore.Mvc;
using NET8ASP.Data.AppDbContext;

namespace NET8ASP.Controllers
{
    public class ShopCartController : Controller
    {
        AppDbContext db;
        
        public ShopCartController()
        {
            
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
