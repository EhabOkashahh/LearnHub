using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Courses;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using Services.Specifications.CoursesSpecifications;
using ServicesAbstraction.Courses;
using Shared.DTOS.Courses;

namespace Services
{
    public class CourseSectionsService(IUnitOfWork _uof, IMapper _mapper) : ICourseSectionsService
    {
        public async Task<IEnumerable<CourseSectionDTO>> GetAllAsync(Guid courseId, CancellationToken ct)
        {
            var spec = new CourseSectionSpec(courseId);
            var sections = await _uof.GetRepository<Guid, CourseSection>().GetAllAsync(spec, ct);
            return _mapper.Map<IEnumerable<CourseSectionDTO>>(sections);
        }

        public async Task<CourseSectionDTO> GetByIdAsync(Guid courseId, Guid id, CancellationToken ct)
        {
            var spec = new CourseSectionSpec(courseId, id);
            var section = await _uof.GetRepository<Guid, CourseSection>().GetAsync(spec, ct);
            if (section is null) throw new NotFoundException($"Section with id: {id} was not found");
            return _mapper.Map<CourseSectionDTO>(section);
        }

        public async Task CreateAsync(Guid courseId, CreateCourseSectionRequest request, string userId, CancellationToken ct)
        {
            var Course = await _uof.GetRepository<Guid,Course>().GetAsync(new CoursesSpec(courseId,false),ct);
            if(Course is null) throw new CourseNotFoundException(courseId);

            var section = _mapper.Map<CourseSection>(request);
            section.CourseId = courseId;
            await _uof.GetRepository<Guid, CourseSection>().AddAsync(section);
            await _uof.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Guid courseId, Guid id, UpdateCourseSectionRequest request, string userId, CancellationToken ct)
        {
            await GetCourseWithOwnerCheck(courseId, userId, ct);

            var spec = new CourseSectionSpec(courseId, id);
            var section = await _uof.GetRepository<Guid, CourseSection>().GetAsync(spec, ct);
            if (section is null) throw new NotFoundException($"Section with id: {id} was not found");

            _mapper.Map(request, section);
            section.UpdatedAt = DateTime.UtcNow;
            await _uof.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid courseId, Guid id, string userId, CancellationToken ct)
        {
            await GetCourseWithOwnerCheck(courseId, userId, ct);

            var spec = new CourseSectionSpec(courseId, id);
            var section = await _uof.GetRepository<Guid, CourseSection>().GetAsync(spec, ct);
            if (section is null) throw new NotFoundException($"Section with id: {id} was not found");

            _uof.GetRepository<Guid, CourseSection>().Delete(id);
            await _uof.SaveChangesAsync(ct);
        }

        private async Task<Course> GetCourseWithOwnerCheck(Guid courseId, string userId, CancellationToken ct)
        {
            var courseSpec = new CoursesSpec(courseId, userId);
            var course = await _uof.GetRepository<Guid, Course>().GetAsync(courseSpec, ct);
            if (course is null) throw new NotFoundException($"Course with id: {courseId} was not found");
            return course;
        }
    }
}