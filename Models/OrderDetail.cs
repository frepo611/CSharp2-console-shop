namespace consoleshoppen.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int OrderedQuantity { get; set; }
        public double PriceOfOrderDetail { get; set; }
    }
}
