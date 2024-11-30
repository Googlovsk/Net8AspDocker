using Microsoft.AspNetCore.Mvc;
using NET8ASP.Data.AppDbContext;
using NET8ASP.Models.Domain.Cart;

namespace NET8ASP.Controllers
{
    public class ShopCartController : Controller
    {
        AppDbContext db;
        ShopCart _shopCart;
        
        public ShopCartController(AppDbContext context, ShopCart shopCart)
        {
            db = context;
            _shopCart = shopCart;
        }
        public IActionResult Index()
        {
            _shopCart.ShopCartItems = _shopCart.GetShopItems();
            return View(_shopCart.ShopCartItems);
        }
        public RedirectToActionResult AddToCart(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                _shopCart.AddToCart(item);
            }
            return RedirectToAction("Index");
        }
    }
}
