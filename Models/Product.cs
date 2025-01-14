namespace Models
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
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int SupplierId { get; set; }
        // Navigation properties
        public required virtual ICollection<ProductCategory> ProductCategories { get; set; }

    }
}