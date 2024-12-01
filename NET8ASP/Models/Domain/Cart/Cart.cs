namespace NET8ASP.Models.Domain.Cart
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public virtual User User { get; set; }
 
        public void AddItem(Product product, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (existingItem == null)
            {
                Items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }
        public int GetTotalPrice()
        {
            return Items.Sum(i => i.Product.Price * i.Quantity);
        }
    }
}
