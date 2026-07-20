using Domain.Entities.Courses;

namespace Services.Specifications.CoursesSpecifications
{
    public class CourseSectionSpec : Specifications<Guid, CourseSection>
    {
        public CourseSectionSpec(Guid courseId, Guid id) : base(S => S.CourseId == courseId && S.Id == id)
        {
        }

        public CourseSectionSpec(Guid courseId) : base(S => S.CourseId == courseId)
        {
            AddOrderByAsc(S => S.Order);
        }
    }
}