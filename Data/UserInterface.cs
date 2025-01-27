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
    private Window _productsInCategoryWindow;
    private Window _shoppingCartWindow;
    private Window _checkoutWindow;
    private Window _addToCartMenu;
    private Window _productWindow;
    private Window _addProductsToCartMenu;
    private ShoppingCart _currentShoppingCart;

    public UserInterface(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
        _currentShoppingCart = GetOrCreateShoppingCart();
        _welcomeWindow = new Window("En Bra Affär", 0, 0, new List<string> { "Välkommen till vår butik!", "På en bra affär gör du en bra affär!" });
        _mainMenu = new("Huvudmeny", 0, 5, GetMenuItems<Menues.Main>());
        _shopMenu = new("Handla", 0, 5, GetMenuItems<Menues.Shop>());
        _adminMenu = new("Administration", 0, 5, GetMenuItems<Menues.Admin>());
        _manageProductsMenu = new("Hantera produkter", 0, 5, GetMenuItems<Menues.ManageProducts>());
        _categoryMenu = new("Kategorier", 0, 5, GetCategories());
        _addProductsToCartMenu = new("Lägg i varukorg", 150, 0, GetMenuItems<Menues.AddProductsToCart>());
        _shoppingCartWindow = new("Kundkorg", 0, 25, _currentShoppingCart.ToList());
    }

    public void Start()
    {
        _welcomeWindow.Draw();
        _mainMenu.Draw();
        _shoppingCartWindow.Draw();
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
                        _shoppingCartWindow.Draw();
                        SelectShopMenuItem();
                        break;
                    case Menues.Main.Se_kundkorg:
                        _shoppingCartWindow.UpdateTextRows(_currentShoppingCart.ToList());
                        _shoppingCartWindow.Draw();
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
                        _shoppingCartWindow.Draw();
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
        // get allowed categories from TextRows
        List<int> listedCategories = new();

        foreach (var row in _categoryMenu.TextRows)
        {
            listedCategories.Add(int.Parse(row.Split('.')[0]));
        }
        Console.Write("Välj kategori (enter): ");
        var categoryId = 0;
        var cursorPosition = Console.GetCursorPosition();
        while (true)
        {
            var userInput = Console.ReadLine();
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            Console.Write(new string(' ', userInput.Length));
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            if (int.TryParse(userInput, out categoryId) && listedCategories.Contains(categoryId))
            {
                break;
            }
        }
        ShowProducts(categoryId);
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
                                          .Where(p => p.ProductCategories.Any(pc => pc.Id == categoryId))
                                          .ToList();
        var categoryName = _dbContext.ProductCategories.Where(pc => pc.Id == categoryId).Select(pc => pc.Name).FirstOrDefault();
        var results = new List<string>();
        foreach (var product in products)
        {
            results.Add($"{product.Id,-2}. {product.Name,-20} {product.Price,-5}");
        }
        _productsInCategoryWindow = new Window(categoryName, 0, _categoryMenu.LowerRightCorner.Y, results);
        _productsInCategoryWindow.Draw();
        SelectProduct();
    }

    private void SelectProduct()
    {
        List<int> listedProducts = new();
        foreach (var row in _productsInCategoryWindow.TextRows)
        {
            listedProducts.Add(int.Parse(row.Split('.')[0]));
        }
        Console.Write("Välj produkt (enter): ");
        var productId = 0;
        var cursorPosition = Console.GetCursorPosition();
        while (true)
        {
            var userInput = Console.ReadLine();
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top); // clear user input
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            if (int.TryParse(userInput, out productId) && listedProducts.Contains(productId))
            {
                break;
            }
        }
        ShowProduct(productId);
    }

    private void ShowProduct(int productId)
    {
        var product = _dbContext.Products.Find(productId);
        if (product == null)
        {
            _productWindow = new Window("Hittar inte produkten", _productsInCategoryWindow.LowerRightCorner.X, _productsInCategoryWindow.Top, new List<string> { "" });
            _productWindow.Draw();
            return;
        }
        else
        {
            _productWindow = new Window(product.Name, _productsInCategoryWindow.LowerRightCorner.X, _productsInCategoryWindow.Top, product.ToList());
            _productWindow.Draw();
            ProductSelection(productId);
        }
    }

    private void ProductSelection(int productId)
    {
        _addProductsToCartMenu.Draw();
        while (true)
        {
            if (Enum.TryParse<Menues.AddProductsToCart>(Console.ReadKey(true).KeyChar.ToString(), out var choice))
            {
                switch (choice)
                {
                    case Menues.AddProductsToCart.Tillbaka:
                        Console.Clear();
                        _welcomeWindow.Draw();
                        _categoryMenu.Draw();
                        _productsInCategoryWindow.Draw();
                        SelectCategory();
                        break;
                    case Menues.AddProductsToCart.Lägg_i_kundkorg:
                        AddProductToCart(productId);

                        break;
                    default:
                        continue;
                }
            }
        }
    }

    private void AddProductToCart(int productId)
    {
        var product = _dbContext.Products.Find(productId);
        if (product == null)
        {
            Console.WriteLine("Produkten kunde inte hittas.");
            return;
        }

        _currentShoppingCart.Products.Add(product);
        _dbContext.SaveChanges();
        UpdateShoppingCart();
        Console.WriteLine("Produkten har lagts till i kundkorgen.");
    }

    private ShoppingCart GetOrCreateShoppingCart()
    {
        var shoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(cart => cart.CustomerId == null);
        if (shoppingCart == null)
        {
            shoppingCart = new ShoppingCart();
            _dbContext.ShoppingCarts.Add(shoppingCart);
            _dbContext.SaveChanges();
        }
        return shoppingCart;
    }

    private void UpdateShoppingCart()
    {
        _shoppingCartWindow.UpdateTextRows(_currentShoppingCart.ToList());
        _shoppingCartWindow.Draw();
    }

    public void CreateCustomerAndAssociateCart(string firstName, string lastName, string email, string address, string city, string zipCode, int countryId, string? phoneNumber = null)
    {
        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Address = address,
            City = city,
            ZipCode = zipCode,
            CountryId = countryId,
            PhoneNumber = phoneNumber
        };

        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();

        _currentShoppingCart.CustomerId = customer.Id;
        _dbContext.SaveChanges();
    }
}
