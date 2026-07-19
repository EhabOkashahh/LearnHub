using Domain.Entities.Identity;

namespace Domain.Entities.Courses
{
    public class Enrollment
    {
        public string StudentId { get; set; } = null!;
        public Guid CourseId { get; set; } 
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public double ProgressPercentage { get; set; }
        public DateTime? CompletedAt { get; set; }

        public AppUser Student { get; set; } = null!;
        public Course Course { get; set; } = null!;

    }
}