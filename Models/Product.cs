
namespace consoleshoppen.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsFeatured { get; set; }
        // Foreign keys
        public int SupplierId { get; set; }
        // Navigation properties
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        //public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();

        // Method to update stock based on ProductVariants
        //public void UpdateStock()
        //{
        //    Stock = ProductVariants?.Sum(pv => pv.Quantity) ?? 0;
        //}
        public override string ToString()
        {
            return $"{Name} {Price}";
        }

        internal List<string> ToList()
        {
            var result = new List<string>();
            result.AddRange([$"{Name} {Price}",$"{Description}"]);
            return result;
        }
    }
}