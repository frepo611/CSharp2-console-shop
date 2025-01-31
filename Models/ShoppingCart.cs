namespace consoleshoppen.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public List<ShoppingCartItem> Items { get; set; } = new();
        public List<string> ToList()
        {
            var result = new List<string>();
            foreach (var item in Items)
            {
                result.Add($"{item.Product.Id,2}. {item.Product.Name,-20} {item.Quantity} {item.Product.Price * item.Quantity:c}");
            }

            if (result.Count == 0) result.Add("Tom varukorg");
            result.Add(new string('-', result.Max(s => s.Length)));
            result.Add($"{Items.Sum(i => i.Product.Price * i.Quantity):c}");
            return result;
        }
    }
}