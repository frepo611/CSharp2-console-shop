using ConsoleShoppen.Classes;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Data
{
    public class ProductDataSeeder
    {
        private readonly ShopDbContext _context;

        public ProductDataSeeder(ShopDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            var categories = _context.ProductCategories.ToList();
            var suppliers = _context.Suppliers.ToList();

            if (!_context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Basic T-Shirt", Description = "Comfortable cotton t-shirt", Price = 19.99m, Stock = 200, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[0] } },
                    new Product { Name = "Graphic T-Shirt", Description = "Stylish graphic t-shirt", Price = 24.99m, Stock = 150, IsFeatured = true, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[0] } },
                    new Product { Name = "V-Neck T-Shirt", Description = "Classic v-neck t-shirt", Price = 22.99m, Stock = 180, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[0] } },
                    new Product { Name = "Long Sleeve T-Shirt", Description = "Warm long sleeve t-shirt", Price = 29.99m, Stock = 120, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[0] } },
                    new Product { Name = "Striped T-Shirt", Description = "Trendy striped t-shirt", Price = 21.99m, Stock = 160, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[0] } },
                    new Product { Name = "Pocket T-Shirt", Description = "T-shirt with a pocket", Price = 23.99m, Stock = 140, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[0] } },
                    new Product { Name = "Henley T-Shirt", Description = "Casual henley t-shirt", Price = 25.99m, Stock = 130, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[0] } },
                    new Product { Name = "Skinny Jeans", Description = "Slim fit skinny jeans", Price = 49.99m, Stock = 100, IsFeatured = true, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[1] } },
                    new Product { Name = "Straight Leg Jeans", Description = "Classic straight leg jeans", Price = 44.99m, Stock = 110, IsFeatured = false, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[1] } },
                    new Product { Name = "Bootcut Jeans", Description = "Stylish bootcut jeans", Price = 46.99m, Stock = 90, IsFeatured = false, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[1] } },
                    new Product { Name = "Slim Fit Pants", Description = "Modern slim fit pants", Price = 39.99m, Stock = 120, IsFeatured = false, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[1] } },
                    new Product { Name = "Chinos", Description = "Comfortable chinos", Price = 34.99m, Stock = 130, IsFeatured = false, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[1] } },
                    new Product { Name = "Cargo Pants", Description = "Utility cargo pants", Price = 42.99m, Stock = 80, IsFeatured = false, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[1] } },
                    new Product { Name = "Jogger Pants", Description = "Casual jogger pants", Price = 29.99m, Stock = 140, IsFeatured = false, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[1] } },
                    new Product { Name = "Denim Jacket", Description = "Classic denim jacket", Price = 59.99m, Stock = 70, IsFeatured = true, SupplierId = suppliers[2].Id, ProductCategories = new List<ProductCategory> { categories[2] } },
                    new Product { Name = "Leather Jacket", Description = "Stylish leather jacket", Price = 99.99m, Stock = 50, IsFeatured = false, SupplierId = suppliers[2].Id, ProductCategories = new List<ProductCategory> { categories[2] } },
                    new Product { Name = "Bomber Jacket", Description = "Trendy bomber jacket", Price = 79.99m, Stock = 60, IsFeatured = false, SupplierId = suppliers[2].Id, ProductCategories = new List<ProductCategory> { categories[2] } },
                    new Product { Name = "Puffer Jacket", Description = "Warm puffer jacket", Price = 89.99m, Stock = 40, IsFeatured = false, SupplierId = suppliers[2].Id, ProductCategories = new List<ProductCategory> { categories[2] } },
                    new Product { Name = "Windbreaker", Description = "Lightweight windbreaker", Price = 49.99m, Stock = 80, IsFeatured = false, SupplierId = suppliers[2].Id, ProductCategories = new List<ProductCategory> { categories[2] } },
                    new Product { Name = "Blazer", Description = "Formal blazer", Price = 69.99m, Stock = 30, IsFeatured = false, SupplierId = suppliers[2].Id, ProductCategories = new List<ProductCategory> { categories[2] } },
                    new Product { Name = "Running Shoes", Description = "Comfortable running shoes", Price = 59.99m, Stock = 100, IsFeatured = true, SupplierId = suppliers[3].Id, ProductCategories = new List<ProductCategory> { categories[3] } },
                    new Product { Name = "Sneakers", Description = "Casual sneakers", Price = 49.99m, Stock = 120, IsFeatured = false, SupplierId = suppliers[3].Id, ProductCategories = new List<ProductCategory> { categories[3] } },
                    new Product { Name = "Boots", Description = "Durable boots", Price = 79.99m, Stock = 90, IsFeatured = false, SupplierId = suppliers[3].Id, ProductCategories = new List<ProductCategory> { categories[3] } },
                    new Product { Name = "Sandals", Description = "Comfortable sandals", Price = 29.99m, Stock = 150, IsFeatured = false, SupplierId = suppliers[3].Id, ProductCategories = new List<ProductCategory> { categories[3] } },
                    new Product { Name = "Loafers", Description = "Stylish loafers", Price = 69.99m, Stock = 80, IsFeatured = false, SupplierId = suppliers[3].Id, ProductCategories = new List<ProductCategory> { categories[3] } },
                    new Product { Name = "Dress Shoes", Description = "Formal dress shoes", Price = 89.99m, Stock = 60, IsFeatured = false, SupplierId = suppliers[3].Id, ProductCategories = new List<ProductCategory> { categories[3] } },
                    new Product { Name = "Flip Flops", Description = "Casual flip flops", Price = 19.99m, Stock = 200, IsFeatured = false, SupplierId = suppliers[3].Id, ProductCategories = new List<ProductCategory> { categories[3] } },
                    new Product { Name = "Sunglasses", Description = "Stylish sunglasses", Price = 19.99m, Stock = 150, IsFeatured = false, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[4] } },
                    new Product { Name = "Watch", Description = "Elegant wristwatch", Price = 99.99m, Stock = 50, IsFeatured = true, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[4] } },
                    new Product { Name = "Belt", Description = "Leather belt", Price = 29.99m, Stock = 100, IsFeatured = false, SupplierId = suppliers[2].Id, ProductCategories = new List<ProductCategory> { categories[4] } },
                    new Product { Name = "Hat", Description = "Fashionable hat", Price = 24.99m, Stock = 80, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[4] } },
                    new Product { Name = "Scarf", Description = "Warm scarf", Price = 19.99m, Stock = 120, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[4] } },
                    new Product { Name = "Gloves", Description = "Winter gloves", Price = 14.99m, Stock = 90, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[4] } },
                    new Product { Name = "Backpack", Description = "Stylish backpack", Price = 49.99m, Stock = 70, IsFeatured = false, SupplierId = suppliers[1].Id, ProductCategories = new List<ProductCategory> { categories[4] } },
                    new Product { Name = "Wallet", Description = "Leather wallet", Price = 39.99m, Stock = 60, IsFeatured = false, SupplierId = suppliers[2].Id, ProductCategories = new List<ProductCategory> { categories[4] } },
                    new Product { Name = "Socks", Description = "Comfortable socks", Price = 9.99m, Stock = 200, IsFeatured = false, SupplierId = suppliers[0].Id, ProductCategories = new List<ProductCategory> { categories[4] } }
                };

                _context.Products.AddRange(products);
                _context.SaveChanges();
            }
        }
    }
}
