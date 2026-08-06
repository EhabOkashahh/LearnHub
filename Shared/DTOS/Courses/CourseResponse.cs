using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Courses
{
    public class CourseResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public string ThumbnailUrl { get; set; } = null!;

        public int TotalDurationMinutes { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime DiscountEndsAt { get; set; }
        public string Level { get; set; } = null!;
        public string Status { get; set; } = null!;

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string InstructorId { get; set; } = null!;
        public string InstructorName { get; set; } = null!;

        public ICollection<CourseSectionDTO> CourseSections { get; set; } = new List<CourseSectionDTO>();
    }
}