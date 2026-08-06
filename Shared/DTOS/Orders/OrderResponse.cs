using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Orders
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = null!;
        public string Status { get; set; } = null!;            
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<OrderItemResponse> Items { get; set; } = [];

    }
}