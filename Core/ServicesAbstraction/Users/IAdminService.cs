using Domain.Entities.Courses.Enums;
using Shared.DTOS.Admin;

namespace ServicesAbstraction.Users
{
    public interface IAdminService
    {
        Task<IEnumerable<InstructorRequestResponse>> GetInstructorRequestsAsync(RequestStatus? status, CancellationToken ct);
        Task ApproveRequestAsync(Guid requestId, CancellationToken ct);
        Task RejectRequestAsync(Guid requestId, CancellationToken ct);
    }
}