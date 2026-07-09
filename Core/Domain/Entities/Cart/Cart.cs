using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities.Cart
{
    public class Cart
    {
        public string Id { get; set; } = null!; //user id => redis key
        public List<CartItem> Items { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}