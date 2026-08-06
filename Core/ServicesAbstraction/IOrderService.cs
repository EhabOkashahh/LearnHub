using Shared.DTOS;
using Shared.DTOS.Orders;

namespace ServicesAbstraction
{
    public interface IOrderService
    {
        Task<OrderSummaryResponse> OrderSummaryAsync(string userId, CancellationToken cancellationToken);
        Task<CheckoutResponse> CreateOrderAsync(string userId, CancellationToken ct);
        Task<PaginatedResponse<OrderResponse>> GetMyOrdersAsync(string userId, OrderQueryParams queryParams, CancellationToken ct);
        Task<OrderResponse> GetOrderByIdAsync(string userId, Guid orderId, CancellationToken ct);
    }
}