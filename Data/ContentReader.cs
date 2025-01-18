namespace consoleshoppen.Data;

using System.Linq;
using Microsoft.EntityFrameworkCore;

public class ContentReader
{
    private readonly ShopDbContext _dbContext;

    public ContentReader(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<string> GetProducts()
    {
        var products = _dbContext.Products.Include(p => p.ProductCategories).ToList();
        var results = new List<string>(); 
        foreach (var product in products)
        {
            results.Add($" {product.ProductCategories.First().Name,-12} Product ID: {product.Id,-2} Name: {product.Name,-20} Price: {product.Price,-5} Stock: {product.Stock}");
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
}
