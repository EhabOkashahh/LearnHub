namespace Shared.DTOS.Courses
{
    public class CourseProgressResponse
    {
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public double Percentage { get; set; }
    }
}