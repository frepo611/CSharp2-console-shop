namespace consoleshoppen;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

internal class Program
{
    private static void Main(string[] args)
    {
        // Set console window size
        //Console.SetWindowSize(200, 60);

        using var dbContext = new ShopDbContext();
        //DependentDataSeeder dependentSeeder = new(dbContext);
        //dependentSeeder.Seed();
        ProductDataSeeder productSeeder = new(dbContext);
        productSeeder.Seed();


        //var ui = new UserInterface(dbContext);
        //ui.Start();

    }
    

}