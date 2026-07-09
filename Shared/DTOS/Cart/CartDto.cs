using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Cart
{
    public class CartDto
    {
        public string Id { get; set; } = null!;
        public IEnumerable<CartItemDto> Items { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}