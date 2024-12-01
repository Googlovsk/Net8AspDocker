namespace NET8ASP.Models.Domain.Cart
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
