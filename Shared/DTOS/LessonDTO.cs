using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS
{
    public class LessonDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public int DurationMinutes { get; set; }

        public string VideoUrl { get; set; } = null!;

        public int Order { get; set; }
    }
}