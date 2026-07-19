using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Courses.Enums;
using Domain.Entities.Identity;

namespace Services.Specifications.UserSpecifications
{
    public class InstructorSpecifications : Specifications<Guid,InstructorRequest>
    {
        public InstructorSpecifications() : base(null)
        {
            IncludeExpression.Add(x => x.User);
        }
        
        public InstructorSpecifications(string userId) : base(x => x.UserId == userId)
        {
            IncludeExpression.Add(x => x.User);
        }
        
        public InstructorSpecifications(RequestStatus status) : base(x => x.Status == status)
        {
            IncludeExpression.Add(x => x.User);
        }
    }
}