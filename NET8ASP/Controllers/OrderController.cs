using Microsoft.AspNetCore.Mvc;
using NET8ASP.Data.AppDbContext;
using NET8ASP.Models.Domain;
using NET8ASP.Models.Domain.Cart;

namespace NET8ASP.Controllers
{
    public class OrderController : Controller
    {
        AppDbContext db;
        ShopCart shopCart;
        public OrderController(AppDbContext context, ShopCart cart)
        {
            db = context;
            shopCart = cart;
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            shopCart.ShopCartItems = shopCart.GetShopItems();
            if (shopCart.ShopCartItems.Count == 0)
            {
                ModelState.AddModelError("", "KYS");
            }
            if (ModelState.IsValid)
            {
                CreateOrder(order);
                return RedirectToAction("Complete");
            }
            return View(order);
        }
        public IActionResult Complete()
        {
            ViewBag.Message = "Заказ обработан";
            return View();
        }

        private void CreateOrder(Order order)
        {
            order.OrderTime = DateTime.Now;
            db.Orders.Add(order);
            db.SaveChanges();

            var items = shopCart.ShopCartItems;
            foreach (var item in items)
            {
                var orderDetail = new OrderDetail
                {
                    Id = item.Product.Id,
                    OrderId = order.Id,
                    Price = item.Product.Price,
                };
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
            }
        }
    }
}
