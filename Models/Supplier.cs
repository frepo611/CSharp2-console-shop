using Models;

namespace consoleshoppen.Models;

public class Supplier
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int CountryId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    virtual public ICollection<Product> Products { get; set; } = new List<Product>();
}
