using consoleshoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace consoleshoppen.Data;


internal class UserInterface
{
    private ShopDbContext _dbContext;
    private Window _welcomeWindow;
    private Window _mainMenu;
    private Window _shopMenu;
    private Window _adminMenu;
    private Window _manageProductsMenu;
    private Window _categoryMenu;
    private Window _productWindow;
    private Window _shoppingCartWindow;
    private Window _checkoutWindow;

    public UserInterface(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
        _welcomeWindow = new Window("En Bra Affär", 0, 0, new List<string> { "Välkommen till vår butik!", "På en bra affär gör du en bra affär!" });
        _mainMenu = new("Huvudmeny", 0, 5, GetMenuItems<Menues.Main>());
        _shopMenu = new("Handla", 0, 5, GetMenuItems<Menues.Shop>());
        _adminMenu = new("Administration", 0, 5, GetMenuItems<Menues.Admin>());
        _manageProductsMenu = new("Hantera produkter", 0, 5, GetMenuItems<Menues.ManageProducts>());
        _categoryMenu = new("Kategorier", 0, 5, GetCategories());

    }
    public void Start()
    {
        _welcomeWindow.Draw();
        _mainMenu.Draw();
        SelectMainMenuItem();

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
        while (true)
        {
            if (Enum.TryParse<Menues.Main>(Console.ReadKey(true).KeyChar.ToString(), out var choice))
            {
                switch (choice)
                {
                    case Menues.Main.Stäng_butiken:
                        Environment.Exit(0);
                        break;
                    case Menues.Main.Logga_in:
                        Window loginWindow = new("Logga in", 1, 1, new List<string> { "Användarnamn:", "Lösenord:" });
                        break;
                    case Menues.Main.Handla:
                        Console.Clear();
                        _welcomeWindow.Draw();
                        _shopMenu.Draw();
                        SelectShopMenuItem();
                        break;
                    case Menues.Main.Se_kundkorg:
                        break;
                    default:
                        continue;
                }
            } 
        }
    }

    private void SelectShopMenuItem()
    {
        while (true)
        {
            if (Enum.TryParse<Menues.Shop>(Console.ReadKey(true).KeyChar.ToString(), out var choice))
            {
                switch (choice)
                {
                    case Menues.Shop.Tillbaka:
                        Console.Clear();
                        Start();
                        break;
                    case Menues.Shop.Se_kategorier:
                        Console.Clear();
                        _welcomeWindow.Draw();
                        _categoryMenu.Draw();
                        SelectCategory();
                        break;
                    case Menues.Shop.Sök:
                        break;
                    default:
                        continue;
                }
            }
        }
    }

    private void SelectCategory()
    {
        var cursorPosition = Console.GetCursorPosition();

        // get allowed categories from TextRows
        List<int> listedCategories = new();
        foreach (var row in _categoryMenu.TextRows)
        {
            listedCategories.Add(int.Parse(row.Split('.')[0]));
        }
        var userInput = Console.ReadLine();
        if (int.TryParse(userInput, out var categoryId) && listedCategories.Contains(categoryId))
        {
            ShowProducts(categoryId);
        }
        else
        {
            Console.SetCursorPosition(cursorPosition.Left,cursorPosition.Top);
            Console.Write(new string(' ',userInput.Length));
        }
    }

    public List<String> GetCategories()
    {
        var categories = _dbContext.ProductCategories.ToList();
        var results = new List<string>();
        foreach (var category in categories)
        {
            results.Add($"{category.Id}. {category.Name}");
        }
        return results;
    }

    public void ShowProducts(int categoryId)
    {
        var products = _dbContext.Products.Include(p => p.ProductCategories)
                                          .ToList()
                                          .Where(p => p.ProductCategories.Any(pc => pc.Id == categoryId));
        var categoryName = _dbContext.ProductCategories.Where(pc => pc.Id == categoryId).Select(pc => pc.Name).FirstOrDefault();
        var results = new List<string>();
        foreach (var product in products)
        {
            results.Add($"{product.Id,-2}. {product.Name,-20} {product.Price,-5}");
        }
        var productWindow = new Window(categoryName, 0, 10, results);
        productWindow.Draw();
    }
}
