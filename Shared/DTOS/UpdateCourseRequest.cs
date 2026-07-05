using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public class UpdateCourseRequest
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string ThumbnailUrl { get; set; } = null!;

        public string Level { get; set; } = null!;

        public Guid CategoryId { get; set; } = default!;
    }
}