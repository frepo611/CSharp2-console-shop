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
            var productGroups = Products.GroupBy(product => product.Name)
                                        .Select(group => new { Id = group.First().Id, Name = group.Key, Count = group.Count(), TotalPrice = group.Sum(p => p.Price) });

            foreach (var group in productGroups)
            {
                result.Add($"{group.Id,-2}. {group.Name,-20} {group.Count} {group.TotalPrice:c}");
            }

            if (result.Count == 0) result.Add("Tom varukorg");
            result.Add(new string('-', result.Max(s => s.Length)));
            result.Add($"{productGroups.Select(g => g.TotalPrice).Sum():c}");
            return result;
        }
        public List<string> AppendTotalPrice(List<string> products)
        {
            var result = new List<string>();

            return result;
        }
    }
}