using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Courses.Enums;
using Shared.DTOS.Admin;

namespace ServicesAbstraction.Users
{
    public interface IAdminService
    {
        Task<IEnumerable<InstructorRequestResponse>> GetInstructorRequestsAsync(RequestStatus? status, CancellationToken ct);
        Task<ApproveInstructorResponse> ApproveRequestAsync(Guid requestId, CancellationToken ct);
        Task<ApproveInstructorResponse> RejectRequestAsync(Guid requestId, CancellationToken ct);
    }
}