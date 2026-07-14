using Domain.Entities.Courses;

namespace Domain.Entities.Identity
{
    public class InstructorProfile : BaseEntity<string>
    {
        public AppUser AppUser { get; set; } = null!;
        public ICollection<Course> Courses { get; set; } = null!;
    }
}