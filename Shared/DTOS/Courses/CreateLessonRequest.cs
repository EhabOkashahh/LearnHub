namespace Shared.DTOS.Courses
{
    public class CreateLessonRequest
    {
        public string Title { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public int Order { get; set; }
        public bool IsFree { get; set; }
    }
}