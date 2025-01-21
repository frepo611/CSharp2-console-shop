namespace consoleshoppen.Models;

public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public int ShippingMethodId { get; set; }
    public int PaymentMethodId { get; set; }
    public double TotalPrice { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
