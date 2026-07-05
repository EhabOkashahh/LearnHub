namespace Domain.Entities.Courses
{
    public class Category : BaseEntity<Guid>
    {
        public string Name { get; set; } = null!;
        public ICollection<Course> Courses { get; set; } = [];
    }
}