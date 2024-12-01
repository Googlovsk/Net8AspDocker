using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NET8ASP.Data.AppDbContext;
using NET8ASP.Models.Domain;
using NET8ASP.Models.Domain.Cart;
using NET8ASP.Models.Domain.Order;
using NET8ASP.Models.ViewModels;

namespace NET8ASP.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext db;

        public OrderController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Payment(int orderId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var order = db.Orders
                          .Where(o => o.Id == orderId && o.UserId == int.Parse(userId))
                          .Include(o => o.OrderItems)
                          .ThenInclude(oi => oi.Product)
                          .FirstOrDefault();

            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new OrderViewModel
            {
                Orders = new List<Order> { order }
            };

            return View(model);
        }

        [Authorize]
        public IActionResult OrderList()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }
            var model = new OrderViewModel
            {
                Orders = db.Orders
                  .Where(o => o.UserId == int.Parse(userId))
                  .Include(o => o.OrderItems)
                  .ThenInclude(oi => oi.Product)
                  .OrderByDescending(o => o.OrderDate)
                  .ToList()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult CompletePayment(int orderId)
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }
            order.Status = "Оплачен";
            db.SaveChanges();

            return RedirectToAction("OrderList");
        }
    }
}
