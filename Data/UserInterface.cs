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
    private Window? _categoryMenu;
    private Window? _productsInCategoryWindow;
    private Window? _shoppingCartWindow;
    private readonly Window _addProductsToCartMenu;
    private Window? _productWindow;
    private ShoppingCart? _currentShoppingCart;
    private readonly Window _manageShoppingCartMenu;
    private readonly Window _firstMenu;
    private Customer? _currentCustomer;
    private readonly Window _currentCustomerWindow;

    public UserInterface(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
        _welcomeWindow = new Window("En Bra Affär", 0, 0, new List<string> { "Välkommen till vår butik!", "På en bra affär gör du en bra affär!" });
        _mainMenu = new Window("Huvudmeny", 0, 5, GetMenuItems<Menues.Main>());
        _shopMenu = new Window("Handla", 0, 5, GetMenuItems<Menues.Shop>());
        _adminMenu = new Window("Administration", 0, 5, GetMenuItems<Menues.Admin>());
        _manageProductsMenu = new Window("Hantera produkter", 0, 5, GetMenuItems<Menues.ManageProducts>());
        _addProductsToCartMenu = new Window("Lägg i varukorg", 100, 0, GetMenuItems<Menues.AddProductsToCart>());
        //_shoppingCartWindow = new Window("Kundkorg", 0, 25, _currentShoppingCart.ToList());
        _manageShoppingCartMenu = new Window("Hantera kundkorg", 50, 25, GetMenuItems<Menues.ShoppingCart>());
        _firstMenu = new Window("Välkommen", 0, 5, GetMenuItems<Menues.First>());
        _currentCustomerWindow = new Window("Kunduppgifter", 50, 0, GetCurrentCustomer());



    }

    public async Task StartAsync()
    {
        Console.Clear();
        if (_categoryMenu == null)
        {
            _categoryMenu = new Window("Kategorier", 0, 5, await GetCategoriesAsync());
        }

        _welcomeWindow.Draw();
        _currentCustomerWindow.UpdateTextRows(GetCurrentCustomer());
        _currentCustomerWindow.Draw();
        _firstMenu.Draw();
        await SelectFirstMenuItem();
    }

    private List<string> GetCurrentCustomer()
    {
        var result = new List<string>();
        if (_currentCustomer == null)
        {
            result.Add("Ingen kund inloggad");
        }
        else
        {
            result.Add($"{_currentCustomer.FirstName} {_currentCustomer.LastName}");
            result.Add($"{_currentCustomer.Email}");
        }
        return result;
    }

    public async Task MainMenuAsync()
    {
        Console.Clear();
        _welcomeWindow.Draw();
        _currentCustomerWindow.UpdateTextRows(GetCurrentCustomer());
        _currentCustomerWindow.Draw();
        _mainMenu.Draw();
        if (_currentShoppingCart == null)
        {
            _currentShoppingCart = new ShoppingCart() { CustomerId = _currentCustomer!.Id};
            await _dbContext.ShoppingCarts.AddAsync(_currentShoppingCart);
        }
        if (_shoppingCartWindow == null)
        {
            _shoppingCartWindow = new Window("Kundkorg", 0, 25, _currentShoppingCart.ToList());
        }
        _shoppingCartWindow.UpdateTextRows(_currentShoppingCart.ToList());
        _shoppingCartWindow.Draw();
        await SelectMainMenuItemAsync();
    }
    private async Task SelectFirstMenuItem()
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
                        await CreateAccount();
                        break;
                    case Menues.First.Logga_in:
                        await LogIn();
                        break;
                    case Menues.First.Växla_konto:
                            //Login();
                            //break;
                    case Menues.First.Till_butiken:
                        if (_currentCustomer != null)
                        {
                            await MainMenuAsync();
                        }
                        else
                        {
                            Console.WriteLine("Du måste logga in för att fortsätta.");
                            Console.ReadKey();
                            await StartAsync();
                        }
                        break;
                }
            }
        }
    }

    private async Task LogIn()
    {
        Console.Write("Ange customer id: ");
        int customerId = int.Parse(Console.ReadLine()!);
        _currentCustomer = _dbContext.Customers.FirstOrDefault(customer => customer.Id == customerId);
        if (_currentCustomer == null)
        {
            Console.WriteLine("Kunde inte hitta kund med det id:t.");
            Console.ReadKey();
            await StartAsync();
        }
        else
        {
            _currentCustomerWindow.UpdateTextRows(GetCurrentCustomer());
            _currentCustomerWindow.Draw();
        }
    }

    private async Task CreateAccount()
    {
        var countryWindow = new Window("Länder", 30, 0, await GetCountryListAsync());
        Console.Clear();
        _welcomeWindow.Draw();
        Console.Write("Förnamn: ");
        string firstName = Console.ReadLine()!;
        Console.Write("Efternamn: ");
        string lastName = Console.ReadLine()!;
        Console.Write("E-post: ");
        string email = Console.ReadLine()!;
        Console.Write("Telefonnummer: ");
        string phoneNumber = Console.ReadLine()!;
        Console.Write("Adress: ");
        string address = Console.ReadLine()!;
        Console.Write("Postnummer: ");
        string zipCode = Console.ReadLine()!;
        Console.Write("Stad: ");
        string city = Console.ReadLine()!;
        Console.Write("Välj land: ");
        var cursorPosition = Console.GetCursorPosition();
        countryWindow.Draw();
        Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
        int countryId = int.Parse(Console.ReadLine()!);
        var newCustomer = new Customer() { Address = address!, City = city!, Email = email!, FirstName = firstName!, LastName = lastName!, ZipCode = zipCode!, CountryId = countryId, PhoneNumber = phoneNumber };
        _dbContext.Customers.Add(newCustomer);
        _currentCustomer = newCustomer;
        await StartAsync();

        await _dbContext.SaveChangesAsync();

    }
    private async Task SelectMainMenuItemAsync()
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
                        _currentCustomerWindow.UpdateTextRows(GetCurrentCustomer());
                        _currentCustomerWindow.Draw();
                        _shopMenu.Draw();
                        _shoppingCartWindow.Draw();
                        await SelectShopMenuItemAsync();
                        break;
                    case Menues.Main.Hantera_kundkorg:
                        _manageShoppingCartMenu.Draw();
                        await SelectManageShoppingCartMenuItemAsync();
                        break;
                }
            }
        }
    }

    private async Task SelectManageShoppingCartMenuItemAsync()
    {
        while (true)
        {
            if (Enum.TryParse<Menues.ShoppingCart>(Console.ReadKey(true).KeyChar.ToString(), out var choice))
            {
                switch (choice)
                {
                    case Menues.ShoppingCart.Tillbaka:
                        Console.Clear();
                        await StartAsync();
                        break;
                    case Menues.ShoppingCart.Ändra_produkt:
                        await SelectProductInCartAsync();
                        break;
                    case Menues.ShoppingCart.Till_kassan:
                        break;
                }
            }
        }
    }

    private async Task SelectProductInCartAsync()
    {
        int productId;
        var cursorPosition = Console.GetCursorPosition();
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
        Console.Write("Välj produkt (enter): ");
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
                _currentCustomerWindow.Draw();
                _categoryMenu!.Draw();
                await SelectCategoryAsync();
                break;
            }
            else if (int.TryParse(userInput, out productId) && _currentShoppingCart.Items.Select(p => p.ProductId).Contains(productId))
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

        ShoppingCartItem? item = _currentShoppingCart!.Items.Where(p => p.ProductId == productId).First();
        Console.Write($"Det ligger {item.Quantity} stycken {item.Product.Name} i kundkorgen. Ange nytt antal: ");
        int newProductQuantity;
        while (true)
        {
            var userInput = Console.ReadLine();
            if (int.TryParse(userInput, out newProductQuantity) && newProductQuantity >= 0)
            {
                break;
            }
        }
        if (newProductQuantity == 0)
        {
            _currentShoppingCart.Items.RemoveAll(p => p.ProductId == productId);
        }
        else if (newProductQuantity < item.Quantity)
        {
            int removeCount = item.Quantity - newProductQuantity;
            for (int i = 0; i < removeCount; i++)
            {
                _currentShoppingCart.Items.Remove(_currentShoppingCart.Items.First(p => p.ProductId == productId));
            }
        }
        else if (newProductQuantity > item.Quantity)
        {
            int addCount = newProductQuantity - item.Quantity;
            for (int i = 0; i < addCount; i++)
            {
                _currentShoppingCart.Items.Add(item);
            }
        }
        _dbContext.SaveChangesAsync();
        UpdateShoppingCartWindow();
        Console.SetCursorPosition(0, cursorPosition.Top);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, cursorPosition.Top);
        Console.WriteLine("Kundkorgen är uppdaterad.");
    }

    private async Task SelectShopMenuItemAsync()
    {
        while (true)
        {
            if (Enum.TryParse<Menues.Shop>(Console.ReadKey(true).KeyChar.ToString(), out var choice))
            {
                switch (choice)
                {
                    case Menues.Shop.Tillbaka:
                        await StartAsync();
                        break;
                    case Menues.Shop.Se_kategorier:
                        Console.Clear();
                        _welcomeWindow.Draw();
                        _categoryMenu!.Draw();
                        _currentCustomerWindow.Draw();
                        _shoppingCartWindow.Draw();
                        await SelectCategoryAsync();
                        break;
                    case Menues.Shop.Hantera_kundkorg:
                        _manageShoppingCartMenu.Draw();
                        await SelectManageShoppingCartMenuItemAsync();
                        break;
                }
            }
        }
    }

    private async Task SelectCategoryAsync()
    {
        List<int> listedCategories = new();

        foreach (var row in _categoryMenu!.TextRows)
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
                _currentCustomerWindow.Draw();
                _shoppingCartWindow.Draw();
                await SelectShopMenuItemAsync();
                break;
            }
            else if (int.TryParse(userInput, out categoryId) && listedCategories.Contains(categoryId))
            {
                await ShowProductsAsync(categoryId);
                break;
            }
        }
    }

    private async Task ShowProductsAsync(int categoryId)
    {
        List<Product> products = await _dbContext.Products.Include(p => p.ProductCategories)
                                          .Where(p => p.ProductCategories.Any(pc => pc.Id == categoryId))
                                          .ToListAsync();
        string? categoryName = await _dbContext.ProductCategories.Where(pc => pc.Id == categoryId).Select(pc => pc.Name).FirstOrDefaultAsync();

        var results = new List<string>();
        results.Add("0. Tillbaka");
        foreach (var product in products)
        {
            results.Add($"{product.Id,-2}. {product.Name,-20} {product.Price,-5}");
        }
        _productsInCategoryWindow = new Window(categoryName!, 0, _categoryMenu!.LowerRightCorner.Y, results);
        _productsInCategoryWindow.Draw();
        await SelectProductAsync();
    }
    private async Task SelectProductAsync()
    {
        List<int> listedProducts = new();
        foreach (var row in _productsInCategoryWindow!.TextRows)
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
                _currentCustomerWindow.Draw();
                _shoppingCartWindow!.Draw();
                _categoryMenu!.Draw();
                await SelectCategoryAsync();
                break;
            }
            else if (int.TryParse(userInput, out var productId) && listedProducts.Contains(productId))
            {
                await ShowProductAsync(productId);
                break;
            }
        }
    }

    private async Task ShowProductAsync(int productId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            _productWindow = new Window("Hittar inte produkten", _productsInCategoryWindow.LowerRightCorner.X, _productsInCategoryWindow.Top, new List<string> { "" });
            _productWindow.Draw();
        }
        else
        {
            _productWindow = new Window(product.Name, _productsInCategoryWindow.LowerRightCorner.X, _productsInCategoryWindow.Top, product.ToList());
            _productWindow.Draw();
            await ProductSelectionAsync(productId);
        }
    }

    private async Task ProductSelectionAsync(int productId)
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
                        _categoryMenu!.Draw();
                        _shoppingCartWindow!.Draw();
                        _currentCustomerWindow.Draw();
                        _productsInCategoryWindow!.Draw();
                        await SelectProductAsync();
                        break;
                    case Menues.AddProductsToCart.Lägg_i_kundkorg:
                        await AddProductToCartAsync(productId);
                        break;
                }
            }
        }
    }

    private async Task AddProductToCartAsync(int productId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            Console.WriteLine("Produkten kunde inte hittas.");
            return;
        }
        var item = _currentShoppingCart!.Items.Find(i => i.ProductId == productId);
        if (item == null)
        {
            item = new ShoppingCartItem { ProductId = productId, Product = product, ShoppingCartId = _currentShoppingCart.Id, Quantity = 1 };
            _currentShoppingCart.Items.Add(item);
        }
        else
        {
            item.Quantity++;
        }
        await _dbContext.SaveChangesAsync();
        UpdateShoppingCartWindow();
        Console.WriteLine("Produkten har lagts till i kundkorgen.");
    }

    private void UpdateShoppingCartWindow()
    {
        _shoppingCartWindow!.UpdateTextRows(_currentShoppingCart!.ToList());
        _shoppingCartWindow.Draw();
    }

    private async Task<List<string>> GetCategoriesAsync()
    {
        var categories = await _dbContext.ProductCategories.ToListAsync();
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
    private async Task<List<string>> GetCountryListAsync()
    {
        return await _dbContext.Countries.Select(c => $"{c.Id,3}. {c.Name}").ToListAsync();
    }
}
