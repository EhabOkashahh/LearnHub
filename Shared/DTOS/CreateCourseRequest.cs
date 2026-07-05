using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public class CreateCourseRequest
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string ThumbnailUrl { get; set; } = null!;
        public string Level { get; set; } = null!;
        public Guid CategoryId { get; set; }
    }
}
