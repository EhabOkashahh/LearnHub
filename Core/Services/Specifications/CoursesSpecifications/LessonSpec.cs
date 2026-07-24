using Domain.Entities.Courses;

namespace Services.Specifications.CoursesSpecifications
{
    public class LessonSpec : Specifications<Guid, Lesson>
    {
        public LessonSpec() : base(null)
        {
        }

        public LessonSpec(Guid sectionId, Guid id) : base(L => L.SectionId == sectionId && L.Id == id)
        {
        }

        public LessonSpec(Guid sectionId) : base(L => L.SectionId == sectionId)
        {
            AddOrderByAsc(L => L.Order);
        }

        public LessonSpec(Guid lessonId, bool includeSection) : base(L => L.Id == lessonId)
        {
            if (includeSection)
                IncludeExpression.Add(L => L.Section);
        }
    }
}