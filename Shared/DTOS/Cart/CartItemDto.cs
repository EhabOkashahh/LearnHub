using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Cart
{
    public class CartItemDto
    {
        public int Id { get; set; }                      
        public Guid CourseId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}