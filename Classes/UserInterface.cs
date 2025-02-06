using ConsoleShoppen.Data;
using ConsoleShoppen.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace ConsoleShoppen.Classes;
public class UserInterface
{
    private readonly DBManager _dbManager;
    private Window? _shippingMethodMenu;
    private Window? _paymentMethodMenu;
    private Window _welcomeWindow;
    private Window _mainMenu;
    private Window _shopMenu;
    private Window _adminMenu;
    private Window _manageProductsMenu;
    private Window? _categoryMenu;
    private Window? _productsInCategoryWindow;
    private Window? _shoppingCartWindow;
    //private Window _checkoutWindow;
    //private Window _addToCartMenu;
    private Window? _productWindow;
    private Window _addProductsToCartMenu;
    //private Window _cartWindow;
    private Window _firstMenu;
    private Window _currentCustomerWindow;
    private Window? _checkoutPaymentMenu;
    private Window _checkoutDeliveryMenu;
    private Window _checkoutConfirmationMenu;
    private ShoppingCart? _currentShoppingCart;
    private Customer? _currentCustomer;
    private Window _manageShoppingCartMenu;
    private Order? _currentOrder;
    private Window? _statisticsMenu;
    private Window? _orderconfirmationWindow;
    private Window? _supplierMenu;
    public UserInterface(DBManager dbManager)
    {
        _dbManager = dbManager;
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
            _categoryMenu = new Window("Kategorier", 0, 5, await _dbManager.GetCategoryIdAndNamesAsync());
        }
        if (_shippingMethodMenu == null)
        {
            _shippingMethodMenu = new Window("Leveranssätt", 0, 5, await _dbManager.GetShippingMethodNamesAsync());
        }
        if (_paymentMethodMenu == null)
        {
            _paymentMethodMenu = new Window("Betalningssätt", 0, 5, await _dbManager.GetPaymentMethodNamesAsync());
        }

        _welcomeWindow.Draw();
        _currentCustomerWindow.UpdateTextRows(GetCurrentCustomer());
        _currentCustomerWindow.Draw();
        _firstMenu.Draw();
        await SelectFirstMenuItem();
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
            await _dbManager.CreateShoppingCartAsync(_currentShoppingCart);
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
                        await AdministrateAsync();
                        break;
                    case Menues.First.Skapa_nytt_konto:
                        await CreateAccountAsync();
                        break;
                    case Menues.First.Logga_in:
                        await LogInAsync();
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
    private async Task SelectMainMenuItemAsync()
    {
        while (true)
        {
            if (TryParseInput(out Menues.Main choice))
            {
                switch (choice)
                {
                    case Menues.Main.Tillbaka:
                        await StartAsync();
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
                    case Menues.Shop.Sök:
                        SearchForProducts();
                        break;
                    case Menues.Shop.Till_kassan:
                        await CheckoutMenuAsync();
                        break;
                    case Menues.Shop.Hantera_kundkorg:
                        _manageShoppingCartMenu.Draw();
                        await SelectManageShoppingCartMenuItemAsync();
                        break;
                }
            }
        }
    }

    private void SearchForProducts()
    {
        Console.Write("Ange sökterm: ");
        var searchTerm = Console.ReadLine();
        if (searchTerm == string.Empty)
        {
            Console.WriteLine("Söktermen kan inte vara tom. Tryck en tangent.");
            return;
        }
        var searchResults = _dbManager.SearchProductsAsync(searchTerm!).Result.ToList();
        searchResults.ForEach(p => Console.WriteLine($"{p.Id}. {p.Name}\t{p.Price:c}"));

        Console.Write("Gå till menyn \"Se kategorier\" för att köpa produkten.");
        return;
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
        List<Product> products = await _dbManager.GetProductsByCategoryAsync(categoryId);
        string? categoryName = await _dbManager.GetCategoryNameAsync(categoryId);

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
        var product = await _dbManager.GetProduct(productId);
        if (_productsInCategoryWindow == null) // If the window is not created, create it, if you are coming from search
        {
            _productsInCategoryWindow = new Window(null, 20, 20, null);
            _productsInCategoryWindow.LowerRightCorner = new Point(10, 10);
        }
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
        var product = await _dbManager.GetProductByIdAsync(productId);
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
        await _dbManager.SaveChangesAsync();
        UpdateShoppingCartWindow();
        Console.WriteLine("Produkten har lagts till i kundkorgen.");
    }
    private void UpdateShoppingCartWindow()
    {
        _shoppingCartWindow!.UpdateTextRows(_currentShoppingCart!.ToList());
        _shoppingCartWindow.Draw();
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
                        _shippingMethodMenu.Draw();
                        await SelectShippingMethodAsync();
                        break;
                }
            }
        }
    }
    private async Task SelectShippingMethodAsync()
    {
        List<int> listedMethods = new();
        foreach (var row in _shippingMethodMenu!.TextRows)
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
                await _dbManager.SaveChangesAsync();
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
                await _dbManager.SaveChangesAsync();
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
                        _currentOrder!.OrderDate = DateTime.Now;
                        //parse order price
                        var orderPrice = double.Parse(_orderconfirmationWindow!.TextRows.Last().Split(' ').Last());
                        _currentOrder.TotalPrice = orderPrice;
                        await _dbManager.CreateOrderAsync(_currentOrder!);

                        // Update stock for each product in the order
                        foreach (var item in _currentShoppingCart!.Items)
                        {
                            var product = await _dbManager.GetProduct(item.ProductId);
                            if (product != null)
                            {
                                product.Stock -= item.Quantity;
                            }
                        }

                        await _dbManager.SaveChangesAsync();
                        _currentOrder = null;
                        _currentShoppingCart = null;
                        await MainMenuAsync();
                        break;
                }
            }
        }
    }
    public async Task LogInAsync()
    {
        bool validInput = false;
        int customerId = 0;
        while (!validInput)
        {
            Console.Write("Ange customer id: ");
            validInput = int.TryParse(Console.ReadLine(), out customerId);
        }
        _currentCustomer = await _dbManager.GetCustomerAsync(customerId);

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
    public async Task CreateAccountAsync()
    {
        var countryWindow = new Window("Länder", 30, 0, await _dbManager.GetCountryNamesAsync());
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
        await _dbManager.AddCustomerAsync(_currentCustomer);
        await _dbManager.SaveChangesAsync();
        await StartAsync();
    }
    private async Task DrawOrderConfirmationInfo()
    {
        var orderDetails = new List<string>();
        _orderconfirmationWindow = new Window($"Order {_currentOrder!.Id}", 20, 20, orderDetails);
        int count = _currentShoppingCart!.ToList().Count();
        var costOfCart = (decimal)_currentShoppingCart.Items.Sum(item => item.Product.Price * item.Quantity);
        orderDetails.AddRange(_currentShoppingCart.ToList().Take(count - 1));
        var shippingMethod = await _dbManager.GetShippingMethodAsync(_currentOrder.ShippingMethodId);

        if (shippingMethod != null)
        {
            orderDetails.Add($"{shippingMethod.Name} {shippingMethod.Price:c}");
        }
        var paymentMethod = await _dbManager.GetPaymentMethodAsync(_currentOrder.ShippingMethodId);
        if (paymentMethod != null)
        {
            orderDetails.Add($"{paymentMethod.Name} {shippingMethod!.Price + costOfCart}");
        }
        _orderconfirmationWindow.Draw();
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
                await EditProductInCart(productId);
                break;
            }
        }
    }
    private async Task EditProductInCart(int productId)
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
        await _dbManager.SaveChangesAsync();
        UpdateShoppingCartWindow();
        Console.SetCursorPosition(0, cursorPosition.Top);
        Console.Write(new string(' ', Console.BufferWidth));
        Console.SetCursorPosition(0, cursorPosition.Top);
        Console.WriteLine("Kundkorgen är uppdaterad.");
    }
    private async Task AdministrateAsync()
    {
        Console.Clear();
        _adminMenu.Draw();
        _supplierMenu = new Window("Leverantörer", 0, 5, await _dbManager.GetSupplierNamesAsync());
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
                        await StartAsync();
                        break;
                    case Menues.Admin.Hantera_produkter:
                        await ManageProductsAsync();
                        break;
                    case Menues.Admin.Hantera_kategorier:
                        break;
                    case Menues.Admin.Hantera_leverantörer:
                        break;
                    case Menues.Admin.Hantera_kunder:
                        break;
                    case Menues.Admin.Statistik:
                        await StatisticsMenuAsync();
                        break;
                }
            }
        }
    }

    private async Task StatisticsMenuAsync()
    {
        _statisticsMenu = new Window("Statistik", 0, 5, GetMenuItems<Menues.Statistics>());
        _statisticsMenu.Draw();
        await SelectStatisticMenuItemAsync();
    }

    private async Task SelectStatisticMenuItemAsync()
    {
        if (TryParseInput(out Menues.Statistics choice))
        {
            switch (choice)
            {
                case Menues.Statistics.Tillbaka:
                    await AdministrateAsync();
                    break;
                case Menues.Statistics.Största_ordervärde:
                    Console.Clear();
                    Console.WriteLine("Största 3 order");
                    var orders = await _dbManager.GetOrderWithLargestValueAsync();
                    foreach (var order in orders)
                    {
                        Console.WriteLine(order);
                    }
                    Console.Write("Tryck en tangent");
                    Console.ReadKey();
                    await AdministrateAsync();
                    break;
                case Menues.Statistics.Leverantör_med_flest_produkter_i_lager:
                    Supplier supplier = await _dbManager.GetSupplierWithMostProductsInStockAsync();
                    break;
                case Menues.Statistics.Kunder_per_land:
                    var customersPerCountry = await _dbManager.GetCustomersPerCountryAsync();
                    break;
            }
        }
    }

    private async Task ManageProductsAsync()
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
                        await AdministrateAsync();
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
        {
            List<int> listedCategoryIds = new();
            List<string> listedCategoryNames = new();
            _categoryMenu!.Draw();
            foreach (var row in _categoryMenu!.TextRows)
            {
                listedCategoryIds.Add(int.Parse(row.Split('.')[0]));
                listedCategoryNames.Add(row.Split('.')[1]);
            }
            Console.Write("Vilken kategori (enter)? ");
            var cursorPosition = Console.GetCursorPosition();
            
            int categoryId;
            while (true)
            {
                var userInput = Console.ReadLine();
                Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
                Console.Write(new string(' ', userInput!.Length));
                Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);

                if (int.TryParse(userInput, out categoryId) && listedCategoryIds.Contains(categoryId))
                {
                    break;
                }
            }
            var existingCategory = await _dbManager.GetProductCategoryAsync(categoryId);
            List<int> supplierIds = new();
            List<string> listedSupplierNames = new();
            _supplierMenu!.Draw();
            foreach (var row in _supplierMenu!.TextRows)
            {
                supplierIds.Add(int.Parse(row.Split('.')[0]));
                listedSupplierNames.Add(row.Split('.')[1]);
            }
            Console.Write("Vilken leverantör (enter)? ");
            cursorPosition = Console.GetCursorPosition();

            int supplierId;
            while (true)
            {
                var userInput = Console.ReadLine();
                Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
                Console.Write(new string(' ', userInput!.Length));
                Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);

                if (int.TryParse(userInput, out supplierId) && supplierIds.Contains(supplierId))
                {
                    break;
                }
            }
            var existingSupplier = await _dbManager.GetSupplierAsync(supplierId);
            Console.WriteLine();
            Console.Write("Ange den nya produktens namn: ");
            string name = Console.ReadLine()!;
            Console.WriteLine();
            Console.WriteLine("Ange beskrivning: ");
            string description = Console.ReadLine()!;
            Console.WriteLine();
            Console.Write("Ange pris: ");
            decimal price;
            while (true)
            {
                var userInput = Console.ReadLine();
                if (decimal.TryParse(userInput, out price))
                {
                    break;
                }
            }
            Console.Write("Ange lagerantal: ");
            int stock;
            while (true)
            {
                var userInput = Console.ReadLine();
                if (int.TryParse(userInput, out stock))
                {
                    break;
                }
            }
            Console.WriteLine($"{existingCategory!.Name}: {name}\n{description}\nPris: {price:c}, Lager: {stock}\nLeverantör: {existingSupplier!.Name}");
            Console.WriteLine("Vill du lägga till produkten? (j/n)");
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.J)
            {
                var product = new Product()
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Stock = stock,
                    SupplierId = supplierId,
                    ProductCategories = new List<ProductCategory> {existingCategory}
                };
                await _dbManager.AddProductAsync(product);
                Console.WriteLine("Produkten är tillagd.");
                Console.ReadKey();
                await ManageProductsAsync();
            }
            else
            {
                await ManageProductsAsync();
            }
        }
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
    public List<string> GetMenuItems<TEnum>() where TEnum : Enum
    {
        var results = new List<string>();
        foreach (var menuItem in Enum.GetValues(typeof(TEnum)))
        {
            results.Add($"{(int)menuItem}. {menuItem.ToString()?.Replace('_', ' ')}");
        }
        return results;
    }
    private async Task UpdateProductAsync()
    {
        Console.WriteLine("Ange produkt att uppdatera: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var product = await _dbManager.GetProductByIdAsync(productId);
            if (product == null)
            {
                Console.WriteLine("Kunde inte hitta produkten.");
                return;
            }

            Console.WriteLine($"Uppdatera namn: {product.Name}");
            Console.Write("Ange nytt namn: ");
            product.Name = Console.ReadLine() ?? product.Name;

            Console.WriteLine($"Uppdatera pris: {product.Price:c}");
            Console.Write("Ange nytt pris: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
            {
                product.Price = newPrice;
            }

            await _dbManager.UpdateProductAsync(product);
            Console.WriteLine("Produkten är uppdaterad.");
        }
    }
    private static bool TryParseInput<T>(out T input) where T : struct
    {
        var rawInput = Console.ReadKey(true).KeyChar.ToString();

        return Enum.TryParse<T>(rawInput, out input);
    }
}
