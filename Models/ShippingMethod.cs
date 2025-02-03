namespace ConsoleShoppen.Models
{
    public class ShippingMethod
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}