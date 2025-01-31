namespace ConsolesShoppen.Data;

internal class Menues
{
    public enum First
    {
        Stäng_butiken = 0,
        Administrera,
        Skapa_nytt_konto,
        Logga_in,
        Växla_konto,
        Till_butiken
    }
    public enum Main
    {
        Stäng_butiken = 0,
        Handla,
        Hantera_kundkorg,
        Till_kassan,
    }

    public enum Shop
    {
        Tillbaka = 0,
        Se_kategorier,
        Sök,
        Hantera_kundkorg,
    }

    public enum Admin
    {
        Tillbaka = 0,
        Hantera_produkter,
        Hantera_kategorier,
        Hantera_leverantörer,
        Hantera_kunder,
    }

    public enum ManageProducts
    {
        Tillbaka = 0,
        Lägg_till_produkt,
        Uppdatera_produkt,
    }

    public enum ManageCategories
    {
        Tillbaka = 0,
        Lägg_till_kategori,
        Uppdatera_kategori,
    }

    public enum ManageCustomers
    {
        Tillbaka = 0,   
        Uppdatera_kunduppgifter,
        Se_orderhistorik,
    }

    public enum AddProductsToCart
    {
        Tillbaka = 0,
        Lägg_i_kundkorg,
    }
    public enum ShoppingCart
    {
        Tillbaka = 0,
        Ändra_produkt,
        Till_kassan,
    }
    public enum CheckoutDelivery
    {
        Avbryt = 0,
        Välj_leveranssätt,
    }
    public enum CheckoutPayment
    {
        Avbryt = 0,
        Välj_betalningssätt,
    }
    public enum CheckoutConfirmation
    {
        Avbryt = 0,
        Bekräfta_order,
    }
}

