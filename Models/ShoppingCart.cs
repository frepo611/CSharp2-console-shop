namespace consoleshoppen.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; } // Nullable CustomerId for temporary cart
        public List<Product> Products { get; set; } = new();
        // Navigation properties
        public Customer? Customer { get; set; }
        public List<string> ToList()
        {
            var result = new List<string>();
            foreach (var product in Products)
            {
                result.Add(product.ToString());
            }
            if (result.Count == 0) result.Add("Tom varukorg");
            return result;
        }
    }
}