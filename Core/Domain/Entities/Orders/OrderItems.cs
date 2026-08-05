using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Courses;
using Domain.Entities.Identity;

namespace Domain.Entities.Orders
{
    public class OrderItems : BaseEntity<Guid>
    {
        public string CourseTitle { get; set; } = null!;
        public string ThumbnailUrl { get; set; } = null!;
        public decimal Price { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }
}