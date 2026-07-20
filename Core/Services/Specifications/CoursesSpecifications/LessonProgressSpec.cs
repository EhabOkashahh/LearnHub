using Domain.Entities.Courses;

namespace Services.Specifications.CoursesSpecifications
{
    public class LessonProgressSpec : Specifications<Guid, LessonProgress>
    {
        public LessonProgressSpec(Guid lessonId, string studentId)
            : base(LP => LP.LessonId == lessonId && LP.StudentId == studentId)
        {
        }

        public LessonProgressSpec(string studentId)
            : base(LP => LP.StudentId == studentId && LP.IsCompleted)
        {
        }
    }
}