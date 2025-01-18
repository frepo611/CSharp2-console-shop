using Microsoft.EntityFrameworkCore;

namespace consoleshoppen.Data;


internal class UserInterface
{
    private ShopDbContext _dbContext;
    public UserInterface(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void ShowCategories()
    {
        var categories = _dbContext.ProductCategories.ToList();
        var results = new List<string>();
        foreach (var category in categories)
        {
            results.Add($"{category.Id}. {category.Name}");
        }
        var categoryWindow = new Window("Kategorier", 1, 1, results);
        categoryWindow.Draw();
    }
    public void ShowProducts(int categoryId)
    {
        var products = _dbContext.Products.Include(p => p.ProductCategories)
                                          .ToList()
                                          .Where(p => p.ProductCategories.Any(pc => pc.Id == categoryId));
        var results = new List<string>();
        foreach (var product in products)
        {
            results.Add($"{product.Id,-2}. {product.Name,-20} {product.Price,-5}");
        }
        var productWindow = new Window("Produkter", 1, 12, results);
        productWindow.Draw();
    }
}
