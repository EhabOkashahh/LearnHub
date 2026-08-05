using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Enums;
using Domain.Entities.Identity;

namespace Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public string OrderNumber { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.pending;

        [CascadeSoftDelete]
        public ICollection<OrderItems> Items { get; set; } = [];

        public string StudentId { get; set; } = null!;
        public AppUser Student { get; set; } = null!;
    }
}