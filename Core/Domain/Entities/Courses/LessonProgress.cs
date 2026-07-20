using Domain.Entities.Identity;

namespace Domain.Entities.Courses
{
    public class LessonProgress : BaseEntity<Guid>
    {
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string StudentId { get; set; } = null!;
        public AppUser Student { get; set; } = null!;
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}