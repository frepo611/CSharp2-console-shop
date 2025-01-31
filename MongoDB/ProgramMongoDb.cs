using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ConsolesShoppen.Data;
using ConsolesShoppen.Models;
using MongoDB.Driver;

namespace ConsolesShoppen.MongoDB;

internal class ProgramMOngoDb
{
    //private static async Task Main(string[] args)
    //{
    //    // Set console window size
    //    //Console.SetWindowSize(200, 60);

    //    var configuration = new ConfigurationBuilder()
    //        .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..")))
    //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //        .Build();

    //    var connectionString = configuration.GetConnectionString("DefaultConnection");

    //    var optionsBuilder = new DbContextOptionsBuilder<ShopDbContext>();
    //    optionsBuilder.UseSqlServer(connectionString);

    //    using var dbContext = new ShopDbContext(optionsBuilder.Options);
    //    DependentDataSeeder dependentSeeder = new(dbContext);
    //    dependentSeeder.Seed();
    //    ProductDataSeeder productSeeder = new(dbContext);
    //    productSeeder.Seed();

    //    var mongoOrders = GetMongoOrderCollection(configuration);

    //    var ui = new UserInterface(dbContext);
    //    await ui.StartAsync();
    //}

    //private static IMongoCollection<Order> GetMongoOrderCollection(IConfiguration configuration)
    //{
    //    var mongoDBConnection = configuration.GetConnectionString("MongoDBConnection");
    //    var client = new MongoClient(mongoDBConnection);
    //    var database = client.GetDatabase("consoleShoppen");
    //    return database.GetCollection<Order>("order");
    //}
}
