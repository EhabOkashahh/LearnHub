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

        public string Level { get; set; } = null!;

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;
        public ICollection<CourseSectionDTO> Sections { get; set; } = new List<CourseSectionDTO>();
    }
}