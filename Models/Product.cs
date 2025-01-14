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
        // Navigation properties
        public required virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public required virtual ICollection<Supplier> Suppliers { get; set; }
        public required virtual ICollection<Size> Sizes { get; set; }
        public virtual ICollection<Color>? Colors { get; set; }

    }
}