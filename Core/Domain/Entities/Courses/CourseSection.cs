namespace Domain.Entities.Courses
{
    public class CourseSection : BaseEntity<Guid>
    {
        public string Title { get; set; } = null!;

        public int Order { get; set; }

        public Guid CourseId { get; set; }

        public Course Course { get; set; } = null!;

        [CascadeSoftDelete]
        public ICollection<Lesson> Lessons { get; set; } = [];
    }
}