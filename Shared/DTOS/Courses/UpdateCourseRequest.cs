using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Courses
{
    public class UpdateCourseRequest
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? ThumbnailUrl { get; set; }
        public decimal? Price { get; set; }
        public string? Level { get; set; }

        public Guid? CategoryId { get; set; }
    }
}