using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Orders
{
    public class OrderItemResponse
    {
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string ThumbnailUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }

    }
}