namespace Shared.DTOS.Courses
{
    public class UpdateLessonRequest
    {
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public string? Description { get; set; }
        public int? DurationMinutes { get; set; }
        public int? Order { get; set; }
        public bool? IsFree { get; set; }
    }
}