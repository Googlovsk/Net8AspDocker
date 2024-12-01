using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET8ASP.Data;
using NET8ASP.Data.AppDbContext;
using NET8ASP.Models.Domain.Cart;
using NET8ASP.Models.Domain.Order;
using NET8ASP.Models.ViewModels;

namespace NET8ASP.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext db;

        public CartController(AppDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                return NotFound();
            }
            var cart = GetCart();

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = quantity,
                    CartId = cart.Id
                });
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (cart.Items.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var order = new Order
            {
                UserId = int.Parse(userId),
                OrderDate = DateTime.Now,
                OrderItems = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Product = i.Product,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };
            
            db.Orders.Add(order);
            db.SaveChanges();

            ClearCart();

            return RedirectToAction("Payment", "Order", new {orderId = order.Id});
        }
        
        private Cart GetCart()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var cart = db.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                db.Carts.Add(cart);
                db.SaveChanges();
            }
            return cart;
        }
        private void ClearCart()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var cart = db.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart != null)
            {
                db.Carts.Remove(cart);
                db.SaveChanges();
            }
        }
    }
}
