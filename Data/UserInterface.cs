using consoleshoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace consoleshoppen.Data;


internal class UserInterface
{
    private readonly ShopDbContext _dbContext;
    private readonly Window _welcomeWindow;
    private readonly Window _mainMenu;
    private readonly Window _shopMenu;
    private readonly Window _adminMenu;
    private readonly Window _manageProductsMenu;
    private readonly Window _categoryMenu;
    private Window _productsInCategoryWindow;
    private readonly Window _shoppingCartWindow;
    private readonly Window _addProductsToCartMenu;
    private Window _productWindow;
    private ShoppingCart _currentShoppingCart;
    private readonly Window _manageShoppingCartMenu;
    private readonly Window _firstMenu;
    private Customer _currentCustomer;

    public UserInterface(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
        _welcomeWindow = new Window("En Bra Affär", 0, 0, new List<string> { "Välkommen till vår butik!", "På en bra affär gör du en bra affär!" });
        _mainMenu = new Window("Huvudmeny", 0, 5, GetMenuItems<Menues.Main>());
        _shopMenu = new Window("Handla", 0, 5, GetMenuItems<Menues.Shop>());
        _adminMenu = new Window("Administration", 0, 5, GetMenuItems<Menues.Admin>());
        _manageProductsMenu = new Window("Hantera produkter", 0, 5, GetMenuItems<Menues.ManageProducts>());
        _categoryMenu = new Window("Kategorier", 0, 5, GetCategories());
        _addProductsToCartMenu = new Window("Lägg i varukorg", 100, 0, GetMenuItems<Menues.AddProductsToCart>());
        _manageShoppingCartMenu = new Window("Hantera kundkorg", 50, 25, GetMenuItems<Menues.ShoppingCart>());
        _firstMenu = new Window("Välkommen", 0, 5, GetMenuItems<Menues.First>());
    }

    public void Start()
    {
        Console.Clear();
        _welcomeWindow.Draw();
        _firstMenu.Draw();
        SelectFirstMenuItem();
    }
    public void MainMenu()
    {
        _mainMenu.Draw();
        try
        {
            _currentShoppingCart.CustomerId = _currentCustomer.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Något blev fel med kundkorgen. {(ex.InnerException == null ? string.Empty : ex.InnerException.Message)} Tryck en tangent för att fortsätta.");
            Console.ReadKey();
            _currentShoppingCart = new ShoppingCart();

        }
        _shoppingCartWindow.UpdateTextRows(_currentShoppingCart.ToList());
        _shoppingCartWindow.Draw();
        SelectMainMenuItem();
    }
    private void SelectFirstMenuItem()
    {
        while (true)
        {
            if (Enum.TryParse<Menues.First>(Console.ReadKey(true).KeyChar.ToString(), out var choice))
            {
                switch (choice)
                {
                    case Menues.First.Stäng_butiken:
                        Environment.Exit(0);
                        break;
                    case Menues.First.Skapa_nytt_konto:
                        CreateAccount();
                        break;
                    case Menues.First.Logga_in:
                        LogIn();
                        break;
                    case Menues.First.Växla_konto:
                        //Login();
                        break;
                        break;
                    case Menues.First.Till_butiken:
                        MainMenu();
                        break;
                }
            }
        }
    }

    private void LogIn()
    {
        Console.Write("Ange customer id: ");
        int customerId = int.Parse(Console.ReadLine());
        _currentCustomer = _dbContext.Customers.FirstOrDefault(customer => customer.Id == customerId);
        if (_currentCustomer == null)
        {
            Console.WriteLine("Kunde inte hitta kund med det id:t.");
            Console.ReadKey();

            Start();
        }
    }

    private void CreateAccount()
    {
        var countryWindow = new Window("Länder", 20, 0, GetCountryList());
        Console.Clear();
        _welcomeWindow.Draw();
        Console.Write("Förnamn: ");
        string firstName = Console.ReadLine();
        Console.Write("Efternamn: ");
        string lastName = Console.ReadLine();
        Console.Write("E-post: ");
        string email = Console.ReadLine();
        Console.Write("Telefonnummer: ");
        string phoneNumber = Console.ReadLine();
        Console.Write("Adress: ");
        string address = Console.ReadLine();
        Console.Write("Postnummer: ");
        string zipCode = Console.ReadLine();
        Console.Write("Stad: ");
        string city = Console.ReadLine();
        Console.Write("Välj land: ");
        countryWindow.Draw();
        int countryId = int.Parse(Console.ReadLine());
        var newCustomer = new Customer() { Address = address, City = city, Email = email, FirstName = firstName, LastName = lastName, ZipCode = zipCode, CountryId = countryId, PhoneNumber = phoneNumber };
        _dbContext.Customers.Add(newCustomer);
        _currentCustomer = newCustomer;
        _dbContext.SaveChanges();
    }

    private void SelectMainMenuItem()
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
                    case Menues.Main.Handla:
                        Console.Clear();
                        _welcomeWindow.Draw();
                        _shopMenu.Draw();
                        _shoppingCartWindow.Draw();
                        SelectShopMenuItem();
                        break;
                    case Menues.Main.Hantera_kundkorg:
                        _manageShoppingCartMenu.Draw();
                        SelectManageShoppingCartMenuItem();
                        break;
                }
            }
        }
    }

    private void SelectManageShoppingCartMenuItem()
    {
        while (true)
        {
            if (Enum.TryParse<Menues.ShoppingCart>(Console.ReadKey(true).KeyChar.ToString(), out var choice))
            {
                switch (choice)
                {
                    case Menues.ShoppingCart.Tillbaka:
                        Console.Clear();
                        Start();
                        break;
                    case Menues.ShoppingCart.Ändra_produkt:
                        SelectProductInCart();
                        break;
                    case Menues.ShoppingCart.Till_kassan:
                        break;
                }
            }
        }
    }

    private void SelectProductInCart()
    {
        int productId;
        Console.Write("Välj produkt (enter): ");
        var cursorPosition = Console.GetCursorPosition();
        while (true)
        {
            var userInput = Console.ReadLine();
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);

            if (userInput == "0")
            {
                Console.Clear();
                _welcomeWindow.Draw();
                _shoppingCartWindow.Draw();
                _categoryMenu.Draw();
                SelectCategory();
                break;
            }
            else if (int.TryParse(userInput, out productId) && _currentShoppingCart.Products.Select(p => p.Id).Contains(productId))
            {
                EditProductInCart(productId);
                break;
            }
        }
    }

    private void EditProductInCart(int productId)
    {
        var cursorPosition = Console.GetCursorPosition();
        Console.SetCursorPosition(0, cursorPosition.Top);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, cursorPosition.Top);

        Product? product = _currentShoppingCart.Products.Where(p => p.Id == productId).First();
        int productQuantity = _currentShoppingCart.Products.Where(p => p.Id == productId).Count();
        Console.Write($"Det ligger {productQuantity} stycken {product.Name} i kundkorgen. Ange nytt antal: ");
        int newProductQuantity;
        while (true)
        {
            var userInput = Console.ReadLine();
            if (int.TryParse(userInput, out newProductQuantity) && newProductQuantity >= 0)
            {
                break;
            }
        }
        // remove product until the amount is correct
        if (newProductQuantity == 0)
        {
            _currentShoppingCart.Products.RemoveAll(p => p.Id == productId);
        }
        else if (newProductQuantity < productQuantity)
        {
            int removeCount = productQuantity - newProductQuantity;
            for (int i = 0; i < removeCount; i++)
            {
                _currentShoppingCart.Products.Remove(_currentShoppingCart.Products.First(p => p.Id == productId));
            }
        }
        else if (newProductQuantity > productQuantity)
        {
            int addCount = newProductQuantity - productQuantity;
            for (int i = 0; i < addCount; i++)
            {
                _currentShoppingCart.Products.Add(product);
            }
        }
        UpdateShoppingCart();
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
                    case Menues.Shop.Hantera_kundkorg:
                        _manageShoppingCartMenu.Draw();
                        SelectManageShoppingCartMenuItem();
                        break;
                }
            }
        }
    }

    private void SelectCategory()
    {
        List<int> listedCategories = new();

        foreach (var row in _categoryMenu.TextRows)
        {
            listedCategories.Add(int.Parse(row.Split('.')[0]));
        }
        Console.Write("Välj kategori (enter): ");
        var cursorPosition = Console.GetCursorPosition();
        while (true)
        {
            int categoryId;
            var userInput = Console.ReadLine();
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            Console.Write(new string(' ', userInput.Length));
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            if (userInput == "0")
            {
                Console.Clear();
                _welcomeWindow.Draw();
                _shopMenu.Draw();
                _shoppingCartWindow.Draw();
                SelectShopMenuItem();
                break;
            }
            else if (int.TryParse(userInput, out categoryId) && listedCategories.Contains(categoryId))
            {
                ShowProducts(categoryId);
                break;
            }
        }
    }

    private void ShowProducts(int categoryId)
    {
        List<Product> products = _dbContext.Products.Include(p => p.ProductCategories)
                                          .Where(p => p.ProductCategories.Any(pc => pc.Id == categoryId))
                                          .ToList();
        string? categoryName = _dbContext.ProductCategories.Where(pc => pc.Id == categoryId).Select(pc => pc.Name).FirstOrDefault();
        var results = new List<string>();
        results.Add("0. Tillbaka");
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
        var cursorPosition = Console.GetCursorPosition();
        while (true)
        {
            var userInput = Console.ReadLine();
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);

            if (userInput == "0")
            {
                Console.Clear();
                _welcomeWindow.Draw();
                _shoppingCartWindow.Draw();
                _categoryMenu.Draw();
                SelectCategory();
                break;
            }
            else if (int.TryParse(userInput, out var productId) && listedProducts.Contains(productId))
            {
                ShowProduct(productId);
                break;
            }
        }
    }

    private void ShowProduct(int productId)
    {
        var product = _dbContext.Products.Find(productId);
        if (product == null)
        {
            _productWindow = new Window("Hittar inte produkten", _productsInCategoryWindow.LowerRightCorner.X, _productsInCategoryWindow.Top, new List<string> { "" });
            _productWindow.Draw();
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
                        _shoppingCartWindow.Draw();
                        _productsInCategoryWindow.Draw();
                        SelectProduct();
                        break;
                    case Menues.AddProductsToCart.Lägg_i_kundkorg:
                        AddProductToCart(productId);
                        break;
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
    private List<string> GetCategories()
    {
        var categories = _dbContext.ProductCategories.ToList();
        var results = new List<string>();
        results.Add("0. Tillbaka");
        foreach (var category in categories)
        {
            results.Add($"{category.Id}. {category.Name}");
        }
        return results;
    }
    private List<string> GetMenuItems<TEnum>() where TEnum : Enum
    {
        var results = new List<string>();
        foreach (var menuItem in Enum.GetValues(typeof(TEnum)))
        {
            results.Add($"{(int)menuItem}. {menuItem.ToString()?.Replace('_', ' ')}");
        }
        return results;
    }
    private List<string> GetCountryList()
    {
        return _dbContext.Countries.Select(c => $"{c.Id}. {c.Name}").ToList();
    }
}
