using consoleshoppen.Models;
using Microsoft.EntityFrameworkCore;
using Models;

namespace consoleshoppen;

public class ShopDbContext : DbContext
{
    public DbSet<Color> Colors { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=.\\SQLExpress;Database=Webshop;Trusted_Connection=True;TrustServerCertificate=true";
        optionsBuilder.UseSqlServer(connectionString);
    }
}