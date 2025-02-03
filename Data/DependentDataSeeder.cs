using ConsoleShoppen.Classes;
using ConsoleShoppen.Models;

namespace ConsoleShoppen.Data;

public class DependentDataSeeder
{
    private readonly ShopDbContext _context;

    public DependentDataSeeder(ShopDbContext context)
    {
        _context = context;
    }
    public void Seed()
    {
        if (!_context.Suppliers.Any())
        {
            var suppliers = new List<Supplier>
            {
                new Supplier { Name = "Fashion Hub", CountryId = 1, PhoneNumber = "123-456-7890", Email = "info@fashionhub.com" },
                new Supplier { Name = "Clothing World", CountryId = 2, PhoneNumber = "234-567-8901", Email = "contact@clothingworld.com" },
                new Supplier { Name = "Apparel Co.", CountryId = 3, PhoneNumber = "345-678-9012", Email = "support@apparelco.com" },
                new Supplier { Name = "Style Store", CountryId = 4, PhoneNumber = "456-789-0123", Email = "sales@stylestore.com" }
            };
            _context.Suppliers.AddRange(suppliers);
        }

        if (!_context.ProductCategories.Any())
        {
            var categories = new List<ProductCategory>
            {
                new ProductCategory { Name = "T-Shirts", Description = "Various styles of t-shirts" },
                new ProductCategory { Name = "Pants", Description = "Different types of pants" },
                new ProductCategory { Name = "Jackets", Description = "Variety of jackets" },
                new ProductCategory { Name = "Shoes", Description = "Different styles of shoes" },
                new ProductCategory { Name = "Accessories", Description = "Various fashion accessories" }
            };
            _context.ProductCategories.AddRange(categories);
        }
        _context.SaveChanges();
    }
}
