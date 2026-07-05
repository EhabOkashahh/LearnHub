namespace Shared.DTOS.Categories
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int CoursesCount { get; set; }
    }
}
