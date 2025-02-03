namespace ConsoleShoppen.Classes;
public class UserInterface
{
    private readonly DBManager _dbmanager;

    public UserInterface(DBManager dbmanager)
    {
        _dbmanager = dbmanager;
    }

    private async Task UpdateProductAsync()
    {
        Console.WriteLine("Ange produkt att uppdatera: ");
        if (int.TryParse(Console.ReadLine(), out int productId))
        {
            var product = await _dbmanager.GetProductByIdAsync(productId);
            if (product == null)
            {
                Console.WriteLine("Kunde inte hitta produkten.");
                return;
            }

            Console.WriteLine($"Uppdatera namn: {product.Name}");
            Console.Write("Ange nytt namn: ");
            product.Name = Console.ReadLine() ?? product.Name;

            Console.WriteLine($"Uppdatera pris: {product.Price:c}");
            Console.Write("Ange nytt pris: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
            {
                product.Price = newPrice;
            }

            // Update other fields similarly...

            await _dbmanager.UpdateProductAsync(product);
            Console.WriteLine("Produkten är uppdaterad.");
        }
        else
        {
            Console.WriteLine("Ogiltigt produkt-ID.");
        }
    }

    // Implement other methods similarly...
}
