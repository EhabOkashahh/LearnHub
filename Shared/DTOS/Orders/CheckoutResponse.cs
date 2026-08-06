using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Orders
{
    public class CheckoutResponse
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = null!;
        public decimal Total { get; set; }
    }
}