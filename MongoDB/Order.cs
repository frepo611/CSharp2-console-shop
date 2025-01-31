using consoleshoppen.MongoDB.Bson;

namespace consoleshoppen.MongoDB;

public class Order
{
    public ObjectId Id { get; set; }
    public required Customer Customer { get; set; }
    public required DateTime OrderDate { get; set; }
    public required ShippingMethod ShippingMethod { get; set; }
    public required PaymentMethod PayPaymentMethod { get; set; }
    public required double TotalPrice { get; set; }
    public List<Product> OrderDetails { get; set; } = [];
}
