using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Admin
{
    public class InstructorRequestResponse
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string UserDisplayName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime RequestedAt { get; set; } 
        public DateTime ReviewedAt { get; set; }
    }
}