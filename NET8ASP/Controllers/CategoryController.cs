using Microsoft.AspNetCore.Mvc;

namespace NET8ASP.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
