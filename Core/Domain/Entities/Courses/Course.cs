using Domain.Entities.Courses;
using Domain.Entities.Courses.Enums;

namespace Domain.Entities.Courses
{
    public class Course : BaseEntity<Guid>
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string ThumbnailUrl { get; set; } = null!;
        public CourseStatus Status { get; set; } = CourseStatus.Draft;
        public int TotalDurationMinutes { get; set; }
        public CourseLevel Level { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;      

        private List<CourseSection> _courseSections = [];
        public IReadOnlyCollection<CourseSection> CourseSections => _courseSections.AsReadOnly();

    }
}