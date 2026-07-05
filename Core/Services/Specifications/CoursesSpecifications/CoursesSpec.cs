using Domain.Entities.Courses;
using Domain.Entities.Courses.Enums;

namespace Services.Specifications.CoursesSpecifications
{
    public class CoursesSpec : Specifications<Guid,Course>
    {
        public CoursesSpec(Guid id) : base(C => C.Id == id)
        {
            ApplyIncludeExpression();
        }
        public CoursesSpec(CourseLevel? Level, Guid? CategpryId) 
        : base(C => (!Level.HasValue || C.Level == Level) && (!CategpryId.HasValue || C.CategoryId == CategpryId))
        {
            ApplyIncludeExpression();
        }
        public CoursesSpec() : base(null)
        {
            ApplyIncludeExpression();
        }


        private void ApplyIncludeExpression()
        {
            IncludeExpression.Add(X => X.Category);
            IncludeExpression.Add(X => X.CourseSections);
        }
    }
}