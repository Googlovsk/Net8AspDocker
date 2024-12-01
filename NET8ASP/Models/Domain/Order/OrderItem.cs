namespace NET8ASP.Models.Domain.Order
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }

        public int OrderId { get; set; }
        //public int ArciviedOrderId { get; set; }
        public Order Order { get; set; }
        //public ArchivedOrder ArchivedOrder { get; set; }
    }
}