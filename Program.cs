﻿using ConsoleShoppen.Classes;
using ConsoleShoppen.Data;
namespace ConsoleShoppen;


internal class Program
{
    private static async Task Main(string[] args)
    {
        // Set console window size
        //Console.SetWindowSize(200, 60);

        //var configuration = new ConfigurationBuilder()
        //    .SetBasePath(Path.GetFullPath(Directory.GetCurrentDirectory()))
        //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //    .Build();

        //var connectionString = configuration.GetConnectionString("SQLConnection");

        //var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
        //optionsBuilder.UseSqlServer(connectionString);

        using var dbContext = new ShopDbContext();
        DependentDataSeeder dependentSeeder = new(dbContext);
        dependentSeeder.Seed();
        ProductDataSeeder productSeeder = new(dbContext);
        productSeeder.Seed();
        var dbManager = new DBManager(dbContext);

        var ui = new UserInterface(dbManager);
        await ui.StartAsync();
    }
}
