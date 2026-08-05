using Domain.Entities.Enums;
using Shared.DTOS.Admin;
using Shared.DTOS.Courses;

namespace ServicesAbstraction.Users
{
    public interface IAdminService
    {
        Task<PaginatedResponse<InstructorRequestResponse>> GetInstructorRequestsAsync(RequestStatus? status, int pageIndex, int pageSize, CancellationToken ct);
        Task ApproveRequestAsync(Guid requestId, CancellationToken ct);
        Task RejectRequestAsync(Guid requestId, CancellationToken ct);
    }
}
