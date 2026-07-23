using Shared.DTOS.Cart;

namespace ServicesAbstraction.Cart
{
    public interface ICartServices
    {
        Task<CartResponse> GetCartAsync(string userId, CancellationToken ct);
        Task AddItemAsync(string userId, Guid courseId, CancellationToken ct);
        Task RemoveItemAsync(string userId, Guid courseId, CancellationToken ct);
        Task ClearCartAsync(string userId, CancellationToken ct);
    }
}
