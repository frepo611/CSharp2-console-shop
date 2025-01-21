using consoleshoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace consoleshoppen.Data;


internal class UserInterface
{
    private ShopDbContext _dbContext;
    public UserInterface(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Start()
    {
        Console.SetWindowSize(200, 60);
        List<string> welcomeText = ["Välkommen till vår butik!","På en bra affär gör du en bra affär!"];
        Window welcomeWindow = new("En Bra Affär", 0, 0, welcomeText);
        Window mainMenu = new("Huvudmeny", 1, 1, GetMenuItems<Menues.Main>());
        var lowerRightCorner = mainMenu.Draw();
        Window shopMenu= new("Handla", lowerRightCorner.Left, 1, GetMenuItems<Menues.Shop>());
        lowerRightCorner = shopMenu.Draw(); 
        Window adminMenu = new("Administration", lowerRightCorner.Left, 1, GetMenuItems<Menues.Admin>());
        lowerRightCorner = adminMenu.Draw();
        Window manageProductsMenu = new("Hantera produkter", 30, 1, GetMenuItems<Menues.ManageProducts>());
        
    }
    public List<string> GetMenuItems<TEnum>() where TEnum : Enum
    {
        var results = new List<string>();
        foreach (var menuItem in Enum.GetValues(typeof(TEnum)))
        {
            results.Add($"{(int)menuItem}. {menuItem.ToString()?.Replace('_', ' ')}");
        }
        return results;
    }

    public void SelectMainMenuItem()
    {
        if (Enum.TryParse<Menues.Main>(Console.ReadKey().KeyChar.ToString(), out var choice))
        {
            switch (choice)
            {
                case Menues.Main.Stäng_butiken:
                    return;
                case Menues.Main.Logga_in:
                    break;
                case Menues.Main.Handla:
                    //RunCategories();
                    break;
                case Menues.Main.Se_kundkorg:
                    break;
            }
        }
    }

    public void GetCategories()
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
