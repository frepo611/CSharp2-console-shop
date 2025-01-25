namespace consoleshoppen.Data;

internal class Menues
{
    public enum Main
    {
        Stäng_butiken = 0,
        Logga_in,
        Handla,
        Se_kundkorg,
        Shipping,
        Payment,
        Admin,
    }

    public enum Shop
    {
        Tillbaka = 0,
        Se_kategorier,
        Sök,
        ViewProduct,
        AddToCart,
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
        Ta_bort_produkt,
        Uppdatera_produkt,
    }

    public enum ManageCategories
    {
        Tillbaka = 0,
        Lägg_till_kategori,
        Ta_bort_kategori,
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
}

