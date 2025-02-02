using ConsolesShoppen.Models;
using ConsoleShoppen.Data;
using Microsoft.EntityFrameworkCore;
namespace ConsolesShoppen.Data;


internal class UserInterface
{
    private DBmanager _dbManager;
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

    public UserInterface(DBmanager dbmanager)
    {
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
        _dbManager = dbmanager;
    }

    public async Task StartAsync()
    {
        Console.Clear();
        if (_categoryMenu == null)
        {
            _categoryMenu = new Window("Kategorier", 0, 5, await _dbManager.GetCategoriesAsync());
        }
        if (_deliveryMethodMenu == null)
        {
            _deliveryMethodMenu = new Window("Leveranssätt", 0, 5, await _dbManager.GetDeliveryMethodsAsync());
        }
        if (_paymentMethodMenu == null)
        {
            _paymentMethodMenu = new Window("Betalningssätt", 0, 5, await _dbManager.GetPaymentMethodsAsync());
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
                        await CreateAccount();
                        break;
                    case Menues.First.Logga_in:
                        await _dbManager.LogIn(_currentCustomer, _currentCustomerWindow);
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

    private async Task AdministrateAsync()
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
                        _shoppingCartWindow!.Draw();
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
                _shoppingCartWindow!.Draw();
                _currentCustomerWindow.Draw();
                _categoryMenu!.Draw();
                await SelectCategoryAsync();
                break;
            }
            else if (int.TryParse(userInput, out productId) && _currentShoppingCart!.Items.Select(p => p.ProductId).Contains(productId))
            {
                EditProductInCart(productId);
                break;
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
                        _shoppingCartWindow!.Draw();
                        await SelectCategoryAsync();
                        break;
                    case Menues.Shop.Sök:
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


    private void UpdateShoppingCartWindow()
    {
        _shoppingCartWindow!.UpdateTextRows(_currentShoppingCart!.ToList());
        _shoppingCartWindow.Draw();
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
                        _deliveryMethodMenu!.Draw();
                        await SelectDeliveryMethodAsync();
                        break;
                }
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
                        await SaveOrderAsync();
                        break;
                }
            }
        }
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
    private static bool TryParseInput<T>(out T input) where T : struct
    {
        var rawInput = Console.ReadKey(true).KeyChar.ToString();

        return Enum.TryParse<T>(rawInput, out input);
    }
}
