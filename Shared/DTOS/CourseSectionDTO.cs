namespace Shared.DTOS
{
    public class CourseSectionDTO
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public int Order { get; set; }

        public ICollection<LessonDTO> Lessons { get; set; } = new List<LessonDTO>();
    }
}