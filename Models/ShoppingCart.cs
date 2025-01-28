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
                result.Add($"{product.Name,-20} {product.Price:c}");
            }
            if (result.Count == 0) result.Add("Tom varukorg");
            result.Add(new string('-', 26));
            result.Add($"{"Totalt:",-20} {Products.Sum(p => p.Price):c}");
            return result;
        }
    }
}