using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Courses;

namespace Services.Specifications.CoursesSpecifications
{
    public class CourseByIdSpec : Specifications<Guid,Course>
    {
        public CourseByIdSpec(Guid id) : base(C => C.Id == id)
        {
            
        }
    }
}