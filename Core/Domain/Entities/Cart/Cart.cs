namespace Domain.Entities.Cart
{
    public class Cart
    {
        public string Id { get; set; } = null!;
        public List<CartItem> Items { get; set; } = null!;
    }
}
