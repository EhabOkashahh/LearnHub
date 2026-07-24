using Domain.Entities;
using Domain.Entities.Identity;

namespace Domain.Entities.Courses
{
    public class Enrollment : BaseEntity<Guid>
    {
        public string StudentId { get; set; } = null!;
        public AppUser Student { get; set; } = null!;
        public Guid CourseId { get; set; } 
        public Course Course { get; set; } = null!;

    }
}