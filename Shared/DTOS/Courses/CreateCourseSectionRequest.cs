namespace Shared.DTOS.Courses
{
    public class CreateCourseSectionRequest
    {
        public string Title { get; set; } = null!;
        public int Order { get; set; }
    }
}