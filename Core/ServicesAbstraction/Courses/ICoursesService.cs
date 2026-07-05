using Domain.Entities.Courses.Enums;
using Shared.DTOS;
using Shared.DTOS.Courses;

namespace ServicesAbstraction.Courses
    {
    public interface ICoursesService
    {
        Task<IEnumerable<CourseResponse>> GetAllCoursesAsync(CourseLevel? Level, Guid? CategpryId, CancellationToken cancellationToken);
        Task<CourseResponse?> GetCourseByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task<int> CreateCourseAsync(CreateCourseRequest request, CancellationToken cancellationToken);
        Task<int> UpdateCourseAsync(Guid Id, UpdateCourseRequest request, CancellationToken cancellationToken);
        Task<int> DeleteCourseAsync(Guid Id, CancellationToken cancellationToken);
    }
}