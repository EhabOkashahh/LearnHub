using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.Courses
{
    public class CreateCourseSectionRequest
    {
        [Required, StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = null!;

        [Range(0, 1000)]
        public int Order { get; set; }
    }
}
