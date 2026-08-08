using Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Enums;

namespace Services.Specifications.CoursesSpecifications
{
    public class CoursesByIdsSpec : Specifications<Guid, Course>
    {
        public CoursesByIdsSpec(IEnumerable<Guid> ids) : base(c => ids.Contains(c.Id) && c.Status == CourseStatus.Published)
        {
            AddInclude(q => q.Include(x => x.Category).Include(x => x.Instructor));
        }
    }
}
