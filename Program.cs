namespace consoleshoppen;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;

internal class Program
{
    private static void Main(string[] args)
    {
        using var dbContext = new ShopDbContext();
        //var dependentSeeder = new DependentDataSeeder(dbContext);
        //dependentSeeder.Seed();
        var productSeeder = new ProductDataSeeder(dbContext);
        productSeeder.Seed();
        var displayer = new TableContentDisplayer(dbContext);

        displayer.DisplayProducts();
        displayer.DisplayCategories();
        displayer.DisplayColors();
        displayer.DisplaySizes();

    }
    

}