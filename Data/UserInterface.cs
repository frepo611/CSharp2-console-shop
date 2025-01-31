using consoleshoppen.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime;

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
    private Window? _deliveryMethodMenu;
    private Window? _paymentMethodMenu;
    private readonly Window _addProductsToCartMenu;
    private Window? _productWindow;
    private ShoppingCart? _currentShoppingCart;
    private readonly Window _manageShoppingCartMenu;
    private readonly Window _firstMenu;
    private Customer? _currentCustomer;
    private readonly Window _currentCustomerWindow;
    private readonly Window _checkoutDeliveryMenu;
    private readonly Window _checkoutPaymentMenu;
    private readonly Window _checkoutConfirmationMenu;
    private Order? _currentOrder;

    public UserInterface(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
        _welcomeWindow = new Window("En Bra Affär", 0, 0, new List<string> { "Välkommen till vår butik!", "På en bra affär gör du en bra affär!" });
        _mainMenu = new Window("Huvudmeny", 0, 5, GetMenuItems<Menues.Main>());
        _shopMenu = new Window("Handla", 0, 5, GetMenuItems<Menues.Shop>());
        _adminMenu = new Window("Administration", 0, 5, GetMenuItems<Menues.Admin>());
        _manageProductsMenu = new Window("Hantera produkter", 0, 5, GetMenuItems<Menues.ManageProducts>());
        _addProductsToCartMenu = new Window("Lägg i varukorg", 100, 0, GetMenuItems<Menues.AddProductsToCart>());
        _manageShoppingCartMenu = new Window("Hantera kundkorg", 50, 25, GetMenuItems<Menues.ShoppingCart>());
        _firstMenu = new Window("Välkommen", 0, 5, GetMenuItems<Menues.First>());
        _currentCustomerWindow = new Window("Kunduppgifter", 50, 0, GetCurrentCustomer());
        _checkoutPaymentMenu = new Window("Välj betalningssätt", 0, 5, GetMenuItems<Menues.CheckoutPayment>());
        _checkoutDeliveryMenu = new Window("Välj leveranssätt", 0, 5, GetMenuItems<Menues.CheckoutDelivery>());
        _checkoutConfirmationMenu = new Window("Bekräfta order", 0, 5, GetMenuItems<Menues.CheckoutConfirmation>());
    }

    public async Task StartAsync()
    {
        Console.Clear();
        if (_categoryMenu == null)
        {
            _categoryMenu = new Window("Kategorier", 0, 5, await GetCategoriesAsync());
        }
        if (_deliveryMethodMenu == null)
        {
            _deliveryMethodMenu = new Window("Leveranssätt", 0, 5, await GetDeliveryMethodsAsync());
        }
        if (_paymentMethodMenu == null)
        {
            _paymentMethodMenu = new Window("Betalningssätt", 0, 5, await GetPaymentMethodsAsync());
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
            _currentShoppingCart = new ShoppingCart() { CustomerId = _currentCustomer!.Id };
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
            if (TryParseInput(out Menues.First choice))
            {
                switch (choice)
                {
                    case Menues.First.Stäng_butiken:
                        Environment.Exit(0);
                        break;
                    case Menues.First.Administrera:
                        Administrate();
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

    private async Task Administrate()
    {
        Console.Clear();
        _adminMenu.Draw();
        await SelectAdminMenuItemAsync();
    }

    private async Task SelectAdminMenuItemAsync()
    {
        while (true)
        {
            if (TryParseInput(out Menues.Admin choice))
            {
                switch (choice)
                {
                    case Menues.Admin.Tillbaka:
                        await MainMenuAsync();
                        break;
                    case Menues.Admin.Hantera_produkter:
                        await ManageProducts();
                        break;
                    case Menues.Admin.Hantera_kategorier:
                        break;
                    case Menues.Admin.Hantera_leverantörer:
                        break;
                    case Menues.Admin.Hantera_kunder:
                        break;
                }
            }
        }
    }

    private async Task ManageProducts()
    {
        _manageProductsMenu.Draw();
        await SelectManageProductsItem();
    }

    private async Task SelectManageProductsItem()
    {
        while (true)
        {
            if (TryParseInput(out Menues.ManageProducts choice))
            {
                switch (choice)
                {
                    case Menues.ManageProducts.Tillbaka:
                        await Administrate();
                        break;
                    case Menues.ManageProducts.Uppdatera_produkt:
                        await UpdateProductAsync();
                        break;
                    case Menues.ManageProducts.Lägg_till_produkt:
                        await AddProductAsync();
                        break;
                }
            }
        }
    }

    private async Task AddProductAsync()
    {
        throw new NotImplementedException();
    }

    private async Task UpdateProductAsync()
    {
        Console.WriteLine("Ange produkt att uppdatera: ");
        int productId = int.Parse(Console.ReadLine()!);
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            Console.WriteLine("Kunde inte hitta produkten.");
            Console.ReadKey();
            await ManageProducts();
        }
        else
        {
            Console.WriteLine($"Uppdatera namn: {product.Name}");
            Console.Write("Ange nytt namn: ");
            product.Name = Console.ReadLine()!;
            Console.WriteLine($"Uppdatera pris: {product.Price:c}");
            Console.Write("Ange nytt pris: ");
            product.Price = decimal.Parse(Console.ReadLine()!);
            Console.WriteLine($"Uppdatera beskrivning: {product.Description}");
            Console.Write("Ange ny beskrivning: ");
            product.Description = Console.ReadLine();
            Console.WriteLine($"Uppdatera lagersaldo: {product.Stock}");
            Console.Write("Ange nytt lagersaldo: ");
            product.Stock = int.Parse(Console.ReadLine()!);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine("Produkten är uppdaterad.");
            Console.ReadKey();
            await ManageProducts();
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
        _currentCustomer = newCustomer;
        _dbContext.Customers.Add(_currentCustomer);
        await _dbContext.SaveChangesAsync();
        await StartAsync();


    }
    private async Task SelectMainMenuItemAsync()
    {
        while (true)
        {
            if (TryParseInput(out Menues.Main choice))
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
                    case Menues.Main.Till_kassan:
                        await CheckoutMenuAsync();
                        break;
                }
            }
        }
    }

    private async Task SelectManageShoppingCartMenuItemAsync()
    {
        while (true)
        {
            if (TryParseInput(out Menues.ShoppingCart choice))
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
                        await CheckoutMenuAsync();
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
        item.Quantity = newProductQuantity;
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
            if (TryParseInput(out Menues.Shop choice))
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
            if (TryParseInput(out Menues.AddProductsToCart choice))
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
    private async Task CheckoutMenuAsync()
    {
        Console.Clear();
        _currentOrder = new Order() { CustomerId = _currentCustomer!.Id, ShoppingCartId = _currentShoppingCart!.Id };
        _welcomeWindow.Draw();
        _currentCustomerWindow.Draw();
        _shoppingCartWindow!.Draw();
        _checkoutDeliveryMenu.Draw();
        await SelectCheckoutDeliveryMenuItemAsync();
    }
    private async Task SelectCheckoutDeliveryMenuItemAsync()
    {
        while (true)
        {
            if (TryParseInput(out Menues.CheckoutDelivery choice))
            {
                switch (choice)
                {
                    case Menues.CheckoutDelivery.Avbryt:
                        await MainMenuAsync();
                        break;
                    case Menues.CheckoutDelivery.Välj_leveranssätt:
                        _deliveryMethodMenu.Draw();
                        await SelectDeliveryMethodAsync();
                        break;
                }
            }
        }
    }

    private async Task SelectDeliveryMethodAsync()
    {
        List<int> listedMethods = new();
        foreach (var row in _deliveryMethodMenu!.TextRows)
        {
            listedMethods.Add(int.Parse(row.Split('.')[0]));
        }
        Console.Write("Välj metod (enter): ");
        var cursorPosition = Console.GetCursorPosition();
        while (true)
        {
            var userInput = Console.ReadLine();
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);

            if (userInput == "0")
            {
                await MainMenuAsync();
                break;
            }
            else if (int.TryParse(userInput, out var methodId) && listedMethods.Contains(methodId))
            {
                _currentOrder!.ShippingMethodId = methodId;
                await _dbContext.SaveChangesAsync();
                _paymentMethodMenu!.Draw();
                await SelectCheckoutPaymentMethodAsync();
                break;
            }
        }
    }

    private async Task SelectCheckoutPaymentMethodAsync()
    {
        List<int> listedMethods = new();
        foreach (var row in _paymentMethodMenu!.TextRows)
        {
            listedMethods.Add(int.Parse(row.Split('.')[0]));
        }
        Console.Write("Välj metod (enter): ");
        var cursorPosition = Console.GetCursorPosition();
        while (true)
        {
            var userInput = Console.ReadLine();
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);

            if (userInput == "0")
            {
                await MainMenuAsync();
                break;
            }
            else if (int.TryParse(userInput, out var methodId) && listedMethods.Contains(methodId))
            {
                _currentOrder!.PaymentMethodId = methodId;
                await _dbContext.SaveChangesAsync();
                _checkoutConfirmationMenu.Draw();
                DrawOrderConfirmationInfo();
                await SelectCheckoutConfirmationAsync();
                break;
            }
        }
    }

    private async Task SelectCheckoutConfirmationAsync()
    {
        while (true)
        {
            if (TryParseInput(out Menues.CheckoutConfirmation choice))
            {
                switch (choice)
                {
                    case Menues.CheckoutConfirmation.Avbryt:
                        await MainMenuAsync();
                        break;
                    case Menues.CheckoutConfirmation.Bekräfta_order:
                        _dbContext.Orders.Add(_currentOrder!);

                        // Update stock for each product in the order
                        foreach (var item in _currentShoppingCart!.Items)
                        {
                            var product = await _dbContext.Products.FindAsync(item.ProductId);
                            if (product != null)
                            {
                                product.Stock -= item.Quantity;
                            }
                        }

                        await _dbContext.SaveChangesAsync();
                        _currentOrder = null;
                        _currentShoppingCart = null;
                        await MainMenuAsync();
                        break;
                }
            }
        }
    }

    private void DrawOrderConfirmationInfo()
    {
        var orderDetails = new List<string>();
        var orderconfirmationWindow = new Window($"Order {_currentOrder!.Id}", 20, 20, orderDetails);
        int count = _currentShoppingCart!.ToList().Count();
        var costOfCart = (decimal)_currentShoppingCart.Items.Sum(item => item.Product.Price * item.Quantity);
        orderDetails.AddRange(_currentShoppingCart.ToList().Take(count - 1));
        var shippingMethod = _dbContext.ShippingMethods.FirstOrDefault(pm => pm.Id == _currentOrder.ShippingMethodId);
        if (shippingMethod != null)
        {
            orderDetails.Add($"{shippingMethod.Name} - {shippingMethod.Price:c}");
        }
        var paymentMethod = _dbContext.PaymentMethods.FirstOrDefault(pm => pm.Id == _currentOrder.PaymentMethodId);
        if (paymentMethod != null)
        {
            orderDetails.Add($"{paymentMethod.Name} {shippingMethod!.Price + costOfCart}");
        }
        orderconfirmationWindow.Draw();
    }

    private async Task<List<string>> GetPaymentMethodsAsync()
    {
        var methods = await _dbContext.PaymentMethods.ToListAsync();
        var results = new List<string>();
        results.Add("0. Tillbaka");
        foreach (var method in methods)
        {
            results.Add($"{method.Id}. {method.Name}");
        }
        return results;
    }
    private async Task<List<string>> GetDeliveryMethodsAsync()
    {
        var methods = await _dbContext.ShippingMethods.ToListAsync();
        var results = new List<string>();
        results.Add("0. Tillbaka");
        foreach (var method in methods)
        {
            results.Add($"{method.Id}. {method.Name}");
        }
        return results;
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
    private static bool TryParseInput<T>(out T input) where T : struct
    {
        var rawInput = Console.ReadKey(true).KeyChar.ToString();

        return Enum.TryParse<T>(rawInput, out input);
    }
}
