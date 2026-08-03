namespace Domain.Entities.Courses
{
    public class Lesson : BaseEntity<Guid>
    {
        public string Title { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public string? Description { get; set; }
        public int DurationMinutes { get; set; }
        public int Order { get; set; }
        public bool IsFree { get; set; } = false;
        public Guid SectionId { get; set; }
        public CourseSection Section { get; set; } = null!;

        [CascadeSoftDelete]
        public ICollection<LessonProgress> ProgressRecords { get; set; } = [];
    }
}