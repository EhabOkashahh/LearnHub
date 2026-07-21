using Shared.DTOS.Courses;

namespace ServicesAbstraction.Courses
{
    public interface IEnrollmentsService
    {
        Task EnrollAsync(string studentId, Guid CourseId, CancellationToken ct);
        Task GetMyEnrollmentsAsync(string StudentId, CourseQueryParams queryParams, CancellationToken ct);
    }
}