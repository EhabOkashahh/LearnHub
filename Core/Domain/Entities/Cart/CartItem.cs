using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities.Cart
{
    public class CartItem
    {
        public int Id { get; set; }                       // local item ID (sequential)
        public Guid CourseId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}