using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Orders
{
    public class OrderSummaryResponse
    {
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public ICollection<OrderItemResponse> Items { get; set; } = [];
    }
}