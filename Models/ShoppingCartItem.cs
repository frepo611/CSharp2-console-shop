namespace consoleshoppen.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public int QuantityInCart { get; set; }
        public int ProductId { get; internal set; }
    }
}