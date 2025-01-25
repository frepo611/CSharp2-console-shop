namespace consoleshoppen.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; } = new();
        public List<string> ToList()
        {
            var result = new List<string>();
            foreach (var product in Products)
            {
                result.AddRange(product.ToList());
            }
            return result;
        }
    }
}