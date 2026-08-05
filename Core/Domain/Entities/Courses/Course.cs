using Domain.Entities.Enums;
using Domain.Entities.Identity;

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
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }




        [CascadeSoftDelete]
        public ICollection<Enrollment> Enrollments { get; set; } = null!;
        public Category Category { get; set; } = null!;

        public string InstructorId { get; set; } = null!;      
        public AppUser Instructor { get; set; } = null!;      

        private List<CourseSection> _courseSections = [];
        
        [CascadeSoftDelete]
        public IReadOnlyCollection<CourseSection> CourseSections => _courseSections.AsReadOnly();

    }
}