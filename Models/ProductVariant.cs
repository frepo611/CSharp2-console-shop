namespace consoleshoppen.Models;

public class ProductVariant
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int ColorId { get; set; }
    public int SizeId { get; set; }
    public int Quantity { get; set; }

    // Navigation properties
    public  virtual Product Product { get; set; }
    public  virtual Color Color { get; set; }
    public  virtual Size Size { get; set; }
}
