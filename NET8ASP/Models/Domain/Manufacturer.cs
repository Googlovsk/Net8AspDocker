namespace NET8ASP.Models.Domain
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
