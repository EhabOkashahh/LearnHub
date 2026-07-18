using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Domain.Entities.Courses.Enums;

namespace Domain.Entities.Identity
{
    public class InstructorRequest : BaseEntity<Guid>
    {
        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
    }
}