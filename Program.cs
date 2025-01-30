namespace consoleshoppen;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;

internal class Program
{
    private static void Main(string[] args)
    {
        // Set console window size
        //Console.SetWindowSize(200, 60);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..")))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        using var dbContext = new ShopDbContext(optionsBuilder.Options);
        DependentDataSeeder dependentSeeder = new(dbContext);
        dependentSeeder.Seed();
        ProductDataSeeder productSeeder = new(dbContext);
        productSeeder.Seed();

        var ui = new UserInterface(dbContext);
        ui.Start();
    }
}
