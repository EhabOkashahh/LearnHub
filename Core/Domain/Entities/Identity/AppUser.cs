using Domain.Entities.Courses;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class AppUser : IdentityUser, ISoftDeletable
    {
        public string DisplayName { get; set; } = null!;
        public string? HeadLine { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        [CascadeSoftDelete]
        public ICollection<Course> Courses { get; set; } = [];

        [CascadeSoftDelete]
        public ICollection<Enrollment> Enrollments { get; set; } = [];

        [CascadeSoftDelete]
        public ICollection<LessonProgress> LessonProgresses { get; set; } = [];

        [CascadeSoftDelete]
        public ICollection<InstructorRequest> InstructorRequests { get; set; } = [];
    }
}