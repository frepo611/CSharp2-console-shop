
namespace ConsolesShoppen.MongoDB;

public class Product
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required int OrderedQuantity { get; set; }
    public required decimal TotalPrice { get; set; }
}