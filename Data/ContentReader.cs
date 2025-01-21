namespace consoleshoppen.Data;
using static ListExtensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using consoleshoppen.Models;

public class ContentReader
{
    private readonly ShopDbContext _dbContext;

    public ContentReader(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<string> GetCategoryProductsInStock(int categoryId)
    {
        var products = _dbContext.Products
                                .Include(p => p.ProductCategories)
                                .Where(p => p.ProductCategories
                                .Any(pc => pc.Id == categoryId) && p.Stock > 0)
                                .ToList();
        var results = new List<string>();
        foreach (var product in products)
        {
            results.Add($"{product.Id,-2}. {product.Name,-20} Price: {product.Price,-5}");
        }
        return results;
    }

    public List<string> GetCategories()
    {
        var categories = _dbContext.ProductCategories.ToList();
        var results = new List<string>();
        foreach (var category in categories)
        {
            results.Add($"{category.Id}. {category.Name}");
        }
        return results;
    }

    public List<string> GetColors()
    {
        var colors = _dbContext.Colors.ToList();
        var results = new List<string>();
        foreach (var color in colors)
        {
            results.Add($"{color.Id} {color.Name}");
        }
        return results;
    }

    public List<string> GetSuppliers()
    {
        var suppliers = _dbContext.Suppliers.ToList();
        var results = new List<string>();
        foreach (var supplier in suppliers)
        {
            results.Add($"{supplier.Id} {supplier.Name}");
        }
        return results;
    }

    public List<string> GetSizes()
    {
        var sizes = _dbContext.Sizes.ToList();
        var results = new List<string>();
        foreach (var size in sizes)
        {
            results.Add($"Size ID: {size.Id}, Name: {size.Name}");
        }
        return results;
    }
    public List<string> GetFeaturedProducts(int productCount)
    {
        var products = _dbContext.Products.Where(p => p.IsFeatured).ToList();
        products.Shuffle();
        var randomProducts = products.Take(productCount);

        var results = new List<string>();

        return results;
    }
  }