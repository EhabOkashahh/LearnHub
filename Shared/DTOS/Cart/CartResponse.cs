namespace Shared.DTOS.Cart
{
    public class CartResponse
    {
        public string Id { get; set; } = null!;
        public IEnumerable<CartItemResponse> Items { get; set; } = [];
    }
}
