using Microsoft.EntityFrameworkCore;
namespace consoleshoppen.Models;

public class ShopDbContext : DbContext
{
    public DbSet<Color> Colors { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShippingMethod> ShippingMethods { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<PaymentType> PaymentTypes { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=.\\SQLExpress;Database=Webshop;Trusted_Connection=True;TrustServerCertificate=true";
        optionsBuilder.UseSqlServer(connectionString);
    }
}