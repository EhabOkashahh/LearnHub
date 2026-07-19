using Domain.Entities.Courses.Enums;
using Shared.DTOS;
using Shared.DTOS.Courses;

namespace ServicesAbstraction.Courses
    {
    public interface ICoursesService
    {
        Task<PaginatedResponse<CourseResponse>> GetAllCoursesAsync(CourseQueryParams queryParams,CancellationToken cancellationToken);
        Task<CourseResponse?> GetCourseByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task CreateCourseAsync(string instructorId, CreateCourseRequest request ,CancellationToken cancellationToken);
        Task UpdateCourseAsync(Guid Id, UpdateCourseRequest request, CancellationToken cancellationToken);
        Task DeleteCourseAsync(Guid Id, CancellationToken cancellationToken);
    }
}