using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.Courses;

namespace Services.Specifications.CoursesSpecifications
{
    public class EnrollmentsSpec : Specifications<Guid,Enrollment>
    {
        public EnrollmentsSpec(string studentId, Guid courseId) : base(x => x.StudentId == studentId && x.CourseId == courseId)
        {

        }
    }
}