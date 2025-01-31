using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace consoleshoppen.Models;

public class ShopDbContext : DbContext
{
    //public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    //{
    //}
    public DbSet<Country> Countries { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShippingMethod> ShippingMethods { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=tcp:frepodb.database.windows.net,1433;Initial Catalog=chsarp2;Persist Security Info=False;User ID=fredrik;Password=drunken3-shun¤-gazette-warships;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        optionsBuilder.UseSqlServer(connectionString);
    }
}
