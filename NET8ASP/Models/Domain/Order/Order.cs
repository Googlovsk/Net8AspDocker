namespace NET8ASP.Models.Domain.Order
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Новый";
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
