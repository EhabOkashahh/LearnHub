using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Courses
{
    public class UpdateCourseRequest
    {
        public string? Title { get; set; } = null!;

        public string? Description { get; set; } = null!;

        public string? ThumbnailUrl { get; set; } = null!;
        public decimal? Price { get; set; } = default!;
        public string? Level { get; set; } = null!;

        public Guid? CategoryId { get; set; } = default!;
    }
}