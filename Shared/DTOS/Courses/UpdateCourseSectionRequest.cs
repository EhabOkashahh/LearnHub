using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.Courses
{
    public class UpdateCourseSectionRequest
    {
        [StringLength(200, MinimumLength = 2)]
        public string? Title { get; set; }

        [Range(0, 1000)]
        public int? Order { get; set; }
    }
}
