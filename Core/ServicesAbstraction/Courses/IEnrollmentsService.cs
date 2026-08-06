using Shared.DTOS;
using Shared.DTOS.Courses;

namespace ServicesAbstraction.Courses
{
    public interface IEnrollmentsService
    {
        Task EnrollAsync(string studentId, Guid CourseId, CancellationToken ct);
        Task<PaginatedResponse<CourseResponse>> GetMyEnrollmentsAsync(string StudentId, CourseQueryParams queryParams, CancellationToken ct);
    }
}