using Shared.DTOS;
using Shared.DTOS.Courses;

namespace ServicesAbstraction.Courses
{
    public interface ILessonsService
    {
        Task<IEnumerable<LessonDTO>> GetAllAsync(Guid courseId, Guid sectionId, CancellationToken ct);
        Task<LessonDTO> GetByIdAsync(Guid courseId, Guid sectionId, Guid id, CancellationToken ct);
        Task CreateAsync(Guid courseId, Guid sectionId, CreateLessonRequest request, string userId, CancellationToken ct);
        Task UpdateAsync(Guid courseId, Guid sectionId, Guid id, UpdateLessonRequest request, string userId, CancellationToken ct);
        Task DeleteAsync(Guid courseId, Guid sectionId, Guid id, string userId, CancellationToken ct);
        Task CompleteAsync(Guid lessonId, string studentId, CancellationToken ct);
        Task UncompleteAsync(Guid lessonId, string studentId, CancellationToken ct);
    }
}