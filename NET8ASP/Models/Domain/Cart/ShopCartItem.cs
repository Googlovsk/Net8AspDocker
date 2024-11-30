namespace NET8ASP.Models.Domain.Cart
{
    public class ShopCartItem
    {
        public int Id { get; set; }
        public Product Product {  get; set; }
        public int Price { get; set; }
        public int ShopCartId { get; set; }
    }
}
