using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Courses;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using Services.Specifications.CoursesSpecifications;
using ServicesAbstraction.Courses;
using Shared.DTOS;
using Shared.DTOS.Courses;

namespace Services.Courses
{
    public class LessonsService(IUnitOfWork _uof, IMapper _mapper) : ILessonsService
    {
        public async Task<IEnumerable<LessonDTO>> GetAllAsync(Guid courseId, Guid sectionId, CancellationToken ct)
        {
            var spec = new LessonSpec(sectionId);
            var lessons = await _uof.GetRepository<Guid, Lesson>().GetAllAsync(spec, ct);
            return _mapper.Map<IEnumerable<LessonDTO>>(lessons);
        }

        public async Task<LessonDTO> GetByIdAsync(Guid courseId, Guid sectionId, Guid id, CancellationToken ct)
        {
            var spec = new LessonSpec(sectionId, id);
            var lesson = await _uof.GetRepository<Guid, Lesson>().GetAsync(spec, ct);
            if (lesson is null) throw new NotFoundException($"Lesson with id: {id} was not found");
            return _mapper.Map<LessonDTO>(lesson);
        }

        public async Task CreateAsync(Guid courseId, Guid sectionId, CreateLessonRequest request, string userId, CancellationToken ct)
        {
            await VerifySectionOwnership(courseId, sectionId, userId, ct);

            var lesson = _mapper.Map<Lesson>(request);
            lesson.SectionId = sectionId;
            await _uof.GetRepository<Guid, Lesson>().AddAsync(lesson);
            await _uof.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Guid courseId, Guid sectionId, Guid id, UpdateLessonRequest request, string userId, CancellationToken ct)
        {
            await VerifySectionOwnership(courseId, sectionId, userId, ct);

            var spec = new LessonSpec(sectionId, id);
            var lesson = await _uof.GetRepository<Guid, Lesson>().GetAsync(spec, ct);
            if (lesson is null) throw new NotFoundException($"Lesson with id: {id} was not found");

            _mapper.Map(request, lesson);
            lesson.UpdatedAt = DateTime.UtcNow;
            await _uof.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid courseId, Guid sectionId, Guid id, string userId, CancellationToken ct)
        {
            await VerifySectionOwnership(courseId, sectionId, userId, ct);

            var spec = new LessonSpec(sectionId, id);
            var lesson = await _uof.GetRepository<Guid, Lesson>().GetAsync(spec, ct);
            if (lesson is null) throw new NotFoundException($"Lesson with id: {id} was not found");

            _uof.GetRepository<Guid, Lesson>().Delete(id);
            await _uof.SaveChangesAsync(ct);
        }

        public async Task CompleteAsync(Guid lessonId, string studentId, CancellationToken ct)
        {
            await VerifyEnrollmentAsync(lessonId, studentId, ct);

            var repo = _uof.GetRepository<Guid, LessonProgress>();
            var existing = await repo.GetAsync(new LessonProgressSpec(lessonId, studentId), ct);

            if (existing is not null)
            {
                if (existing.IsCompleted)
                    throw new BadRequestException("Lesson is already completed");
                existing.IsCompleted = true;
                existing.CompletedAt = DateTime.UtcNow;
            }
            else
            {
                await repo.AddAsync(new LessonProgress
                {
                    LessonId = lessonId,
                    StudentId = studentId,
                    IsCompleted = true,
                    CompletedAt = DateTime.UtcNow
                });
            }

            await _uof.SaveChangesAsync(ct);
        }

        public async Task UncompleteAsync(Guid lessonId, string studentId, CancellationToken ct)
        {
            await VerifyEnrollmentAsync(lessonId, studentId, ct);

            var repo = _uof.GetRepository<Guid, LessonProgress>();
            var existing = await repo.GetAsync(new LessonProgressSpec(lessonId, studentId), ct);

            if (existing is null)
                throw new BadRequestException("Lesson is not completed yet");

            existing.IsCompleted = false;
            existing.CompletedAt = null;
            await _uof.SaveChangesAsync(ct);
        }

        private async Task VerifyEnrollmentAsync(Guid lessonId, string studentId, CancellationToken ct)
        {
            var lessonSpec = new LessonSpec(lessonId, includeSection: true);
            var lesson = await _uof.GetRepository<Guid, Lesson>().GetAsync(lessonSpec, ct);
            if (lesson is null)
                throw new NotFoundException($"Lesson with id: {lessonId} was not found");

            var courseId = lesson.Section.CourseId;

            var enrollmentSpec = new EnrollmentsSpec(studentId, courseId);
            if (!await _uof.GetRepository<Guid, Enrollment>().Exists(enrollmentSpec))
                throw new BadRequestException("You are not enrolled in this course");
        }

        private async Task VerifySectionOwnership(Guid courseId, Guid sectionId, string userId, CancellationToken ct)
        {
            var courseSpec = new CoursesSpec(courseId, userId);
            var course = await _uof.GetRepository<Guid, Course>().GetAsync(courseSpec, ct);
            if (course is null) throw new NotFoundException($"Course with id: {courseId} was not found");

            var sectionSpec = new CourseSectionSpec(courseId, sectionId);
            var section = await _uof.GetRepository<Guid, CourseSection>().GetAsync(sectionSpec, ct);
            if (section is null) throw new NotFoundException($"Section with id: {sectionId} was not found in course: {courseId}");
        }
    }
}