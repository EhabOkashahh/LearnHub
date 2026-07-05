using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities.Courses;

namespace Services.Specifications.CoursesSpecifications
{
    public class CoursesSpec : Specifications<Guid,Course>
    {
        public CoursesSpec(Guid id) : base(C => C.Id == id)
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