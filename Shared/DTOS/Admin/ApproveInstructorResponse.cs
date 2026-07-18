using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOS.Admin
{
    public class ApproveInstructorResponse
    {
        public string Message { get; set; } = null!;
        public string? Token { get; set; } = null!;
    }
}