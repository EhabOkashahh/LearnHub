using Domain.Entities.Courses;
using Microsoft.EntityFrameworkCore;
using Services.Specifications;

namespace Services.Specifications.CoursesSpecifications
{
    public class CoursesByIdsSpec : Specifications<Guid, Course>
    {
        public CoursesByIdsSpec(IEnumerable<Guid> ids) : base(c => ids.Contains(c.Id))
        {
            AddInclude(q => q.Include(x => x.Category).Include(x => x.Instructor));
        }
    }
}
