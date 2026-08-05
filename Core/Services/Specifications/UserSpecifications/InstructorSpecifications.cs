using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Enums;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.Specifications.UserSpecifications
{
    public class InstructorSpecifications : Specifications<Guid,InstructorRequest>
    {
        public InstructorSpecifications() : base(null)
        {
            AddInclude(q => q.Include(x => x.User));
        }

        public InstructorSpecifications(Guid id) : base(x => x.Id == id)
        {
            AddInclude(q => q.Include(x => x.User));
        }
        
        public InstructorSpecifications(string userId) : base(x => x.UserId == userId)
        {
            AddInclude(q => q.Include(x => x.User));
        }
        
        public InstructorSpecifications(RequestStatus status) : base(x => x.Status == status)
        {
            AddInclude(q => q.Include(x => x.User));
        }

        public InstructorSpecifications(RequestStatus? status, int pageIndex, int pageSize) : base(
            x => !status.HasValue || x.Status == status.Value)
        {
            AddInclude(q => q.Include(x => x.User));
            ApplyPagination(pageIndex, pageSize);
            AddOrderByDesc(x => x.CreatedAt);
        }
    }
}