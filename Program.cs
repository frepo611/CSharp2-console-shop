namespace consoleshoppen;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

internal class Program
{
    private static void Main(string[] args)
    {
        using var dbContext = new ShopDbContext();
        var ui = new UserInterface(dbContext);
        ui.ShowCategories();
        ui.ShowProducts(1);
        //var dependentSeeder = new DependentDataSeeder(dbContext);
        //dependentSeeder.Seed();
        //var productSeeder = new ProductDataSeeder(dbContext);
        //productSeeder.Seed();
        //displayer.GetProducts();
        //displayer.GetCategories();
        //displayer.GetColors();
        //displayer.GetSizes();

    }
    

}