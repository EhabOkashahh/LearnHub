using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.Courses
{
    public class UpdateCourseRequest
    {
        [StringLength(200, MinimumLength = 3)]
        public string? Title { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }

        [Url]
        public string? ThumbnailUrl { get; set; }

        [Range(0, 99999.99)]
        public decimal? Price { get; set; }

        public string? Level { get; set; }

        public Guid? CategoryId { get; set; }
    }
}
