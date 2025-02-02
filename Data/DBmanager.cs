using ConsolesShoppen.Data;
using ConsolesShoppen.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsoleShoppen.Data;

internal class DBmanager
{
    private ShopDbContext _dbContext;
    public DBmanager(ShopDbContext db)
    {
        _dbContext = db;
    }
    public async Task AddProductAsync(Product newProduct)
    {
        await _dbContext.Products.AddAsync(newProduct);
        await _dbContext.SaveChangesAsync();
        Console.WriteLine("Produkten har lagts till.");
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
    public async Task LogIn(Customer _currentCustomer, Window _currentCustomerWindow)
    {
        Console.Write("Ange customer id: ");
        int customerId = int.Parse(Console.ReadLine()!);
        _currentCustomer = _dbContext.Customers.FirstOrDefault(customer => customer.Id == customerId);
        if (_currentCustomer == null)
        {
            Console.WriteLine("Kunde inte hitta kund med det id:t.");
            Console.ReadKey();
            await UserInterface.StartAsync();
        }
        else
        {
            _currentCustomerWindow.UpdateTextRows(GetCurrentCustomer());
            _currentCustomerWindow.Draw();
        }
    }
    private void EditProductInCart(int productId, ShoppingCart _currentShoppingCart)
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
    private async Task SelectCategoryAsync(Window _categoryMenu, Window _welcomeWindow, Window _shopMenu, Window _currentCustomerWindow, Window _shoppingCartWindow)
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
            Console.Write(new string(' ', userInput!.Length));
            Console.SetCursorPosition(cursorPosition.Left, cursorPosition.Top);
            if (userInput == "0")
            {
                Console.Clear();
                _welcomeWindow.Draw();
                _shopMenu.Draw();
                _currentCustomerWindow.Draw();
                _shoppingCartWindow!.Draw();
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

    private async Task ShowProductsAsync(int categoryId, Window _productsInCategoryWindow, Window _categoryMenu)
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
    private async Task SelectProductAsync(Window _productsInCategoryWindow, Window _welcomeWindow, Window _currentCustomerWindow, Window _shoppingCartWindow, Window _categoryMenu)
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

    private async Task ShowProductAsync(int productId, Window _productWindow, Window _productsInCategoryWindow)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            _productWindow = new Window("Hittar inte produkten", _productsInCategoryWindow!.LowerRightCorner.X, _productsInCategoryWindow.Top, new List<string> { "" });
            _productWindow.Draw();
        }
        else
        {
            _productWindow = new Window(product.Name, _productsInCategoryWindow!.LowerRightCorner.X, _productsInCategoryWindow.Top, product.ToList());
            _productWindow.Draw();
            await ProductSelectionAsync(productId);
        }
    }
    private async Task AddProductToCartAsync(int productId, ShoppingCart _currentShoppingCart)
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
    private async Task CheckoutMenuAsync(Customer _currentCustomer, Order _currentOrder, ShoppingCart _currentShoppingCart, Window _welcomeWindow, Window _currentCustomerWindow, Window _shoppingCartWindow, Window _checkoutDeliveryMenu)
    {
        Console.Clear();
        _currentOrder = new Order() { CustomerId = _currentCustomer!.Id, ShoppingCartId = _currentShoppingCart!.Id, OrderDate = DateTime.Now };
        _welcomeWindow.Draw();
        _currentCustomerWindow.Draw();
        _shoppingCartWindow!.Draw();
        _checkoutDeliveryMenu.Draw();
        await SelectCheckoutDeliveryMenuItemAsync();
    }
    private async Task SelectDeliveryMethodAsync(Window _deliveryMethodMenu, Order _currentOrder, ShoppingCart _currentShoppingCart, Window _paymentMethodMenu)
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
                _currentOrder.TotalPrice = (double)_currentShoppingCart!.Items.Sum(item => item.Product.Price * item.Quantity + _dbContext.ShippingMethods.Where(sm => sm.Id == methodId).FirstOrDefault()!.Price);
                await _dbContext.SaveChangesAsync();
                _paymentMethodMenu!.Draw();
                await SelectCheckoutPaymentMethodAsync();
                break;
            }
        }
    }

    private async Task SelectCheckoutPaymentMethodAsync(Window _paymentMethodMenu, Order _currentOrder, Window _checkoutConfirmationMenu)
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
    private async Task SaveOrderAsync(Order _currentOrder, ShoppingCart _currentShoppingCart)
    {
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
        // TODO Save order to MongoDB
        _currentOrder = null;
        _currentShoppingCart = null;
        await MainMenuAsync();
    }
    public async Task MainMenuAsync(Window _welcomeWindow, Window _currentCustomerWindow, Window _mainMenu, Window _shoppingCartWindow, ShoppingCart _currentShoppingCart, Customer _currentCustomer)
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
    private void DrawOrderConfirmationInfo(Order _currentOrder, ShoppingCart _currentShoppingCart)
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

    public async Task<List<string>> GetPaymentMethodsAsync()
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
    public async Task<List<string>> GetDeliveryMethodsAsync()
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
    public async Task<List<string>> GetCategoriesAsync()
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
    private async Task<List<string>> GetCountryListAsync()
    {
        return await _dbContext.Countries.Select(c => $"{c.Id,3}. {c.Name}").ToListAsync();
    }

}
