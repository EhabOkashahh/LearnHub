using Shared.DTOS.Courses;

namespace ServicesAbstraction.Courses
{
    public interface ICourseSectionsService
    {
        Task<IEnumerable<CourseSectionDTO>> GetAllAsync(Guid courseId, CancellationToken ct);
        Task<CourseSectionDTO> GetByIdAsync(Guid courseId, Guid id, CancellationToken ct);
        Task CreateAsync(Guid courseId, CreateCourseSectionRequest request, string userId, CancellationToken ct);
        Task UpdateAsync(Guid courseId, Guid id, UpdateCourseSectionRequest request, string userId, CancellationToken ct);
        Task DeleteAsync(Guid courseId, Guid id, string userId, CancellationToken ct);
    }
}