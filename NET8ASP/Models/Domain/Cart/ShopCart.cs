using Microsoft.EntityFrameworkCore;
using NET8ASP.Data.AppDbContext;

namespace NET8ASP.Models.Domain.Cart
{
    public class ShopCart
    {
        private AppDbContext db;

        public ShopCart(AppDbContext context)
        {
            db = context;
        }
        public int ShopCartId { get; set; }
        public ICollection<ShopCartItem> ShopCartItems { get; set; }


        public static ShopCart GetCart(IServiceProvider service)
        {
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = service.GetService<AppDbContext>();

            string shopCartId = session.GetString("Id") ??Guid.NewGuid().ToString();
            session.SetString("Id", shopCartId);
            return new ShopCart(context)
            {
                ShopCartId = int.Parse(shopCartId)
            };
        }
        public void AddToCart(Product product)
        {
            db.ShopCartItems.Add(new ShopCartItem
            {
                ShopCartId = ShopCartId,
                Product = product,
                Price = product.Price,
            });
            db.SaveChanges();
        }
        public ICollection<ShopCartItem> GetShopItems()
        {
            return db.ShopCartItems.Where(p => p.ShopCartId == ShopCartId).Include(p => p.Product).ToList();
        }
    }
}
