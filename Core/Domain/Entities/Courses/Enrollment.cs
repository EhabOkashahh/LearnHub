using Domain.Entities;
using Domain.Entities.Identity;

namespace Domain.Entities.Courses
{
    public class Enrollment : BaseEntity<Guid>
    {
        public double ProgressPercentage { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string StudentId { get; set; } = null!;
        public AppUser Student { get; set; } = null!;
        public Guid CourseId { get; set; } 
        public Course Course { get; set; } = null!;

    }
}