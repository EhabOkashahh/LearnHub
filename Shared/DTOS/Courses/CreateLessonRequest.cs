using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.Courses
{
    public class CreateLessonRequest
    {
        [Required, StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Required, Url]
        public string VideoUrl { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Range(1, 1440)]
        public int DurationMinutes { get; set; }

        [Range(0, 1000)]
        public int Order { get; set; }

        public bool IsFree { get; set; }
    }
}
