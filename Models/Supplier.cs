using Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace consoleshoppen.Models;

public class Supplier
{
    public int Id { get; set; }
    public required string Name { get; set; }
    [ForeignKey("CountryId")]
    public int CountryId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    virtual public ICollection<Product> Products { get; set; } = new List<Product>();
}
