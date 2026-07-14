using Domain.Entities.Courses;

namespace Domain.Entities.Identity
{
    public class StudentProfile : BaseEntity<string>
    {
        public AppUser AppUser { get; set; } = null!;
        public ICollection<Enrollment> Enrollments { get; set; } = null!;
    }
}