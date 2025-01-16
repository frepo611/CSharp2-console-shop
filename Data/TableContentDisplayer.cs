namespace consoleshoppen;

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class TableContentDisplayer
{
    private readonly ShopDbContext _dbContext;

    public TableContentDisplayer(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void DisplayProducts()
    {
        var products = _dbContext.Products.Include(p => p.ProductCategories).ToList();
        foreach (var product in products)
        {
            Console.WriteLine($" {(product.ProductCategories.First().Name),-12} Product ID: {(product.Id),-2} Name: {(product.Name),-20} Price: {(product.Price),-5} Stock: {product.Stock}");
        }
    }

    public void DisplayCategories()
    {
        var categories = _dbContext.ProductCategories.ToList();
        foreach (var category in categories)
        {
            Console.WriteLine($"Category ID: {category.Id}, Name: {category.Name}");
        }
    }

    public void DisplayColors()
    {
        var colors = _dbContext.Colors.ToList();
        foreach (var color in colors)
        {
            Console.WriteLine($"Color ID: {color.Id}, Name: {color.Name}");
        }
    }

    public void DisplaySuppliers()
    {
        var suppliers = _dbContext.Suppliers.ToList();
        foreach (var supplier in suppliers)
        {
            Console.WriteLine($"Supplier ID: {supplier.Id}, Name: {supplier.Name}, Phone: {supplier.PhoneNumber}, Email: {supplier.Email}");
        }
    }

    public void DisplaySizes()
    {
        var sizes = _dbContext.Sizes.ToList();
        foreach (var size in sizes)
        {
            Console.WriteLine($"Size ID: {size.Id}, Name: {size.Name}");
        }
    }
}
