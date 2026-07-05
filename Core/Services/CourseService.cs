using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.Courses.Enums;
using ServicesAbstraction.Courses;
using Shared.DTOS;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Courses;
using Domain.Contracts;
using Services.Specifications.CoursesSpecifications;

namespace Services
{
    public class CourseService(IUnitOfWork _uof, IMapper _mapper) : ICoursesService
    {
        public async Task<IEnumerable<CourseResponse>> GetAllCoursesAsync(CancellationToken ct)
        {
            var spec = new CoursesSpec();
            var courses = await _uof.GetRepository<Guid,Course>().GetAllAsync(spec,ct);
            return _mapper.Map<IEnumerable<CourseResponse>>(courses);
        }

        public async Task<CourseResponse?> GetCourseByIdAsync(Guid Id, CancellationToken ct)
        {
            var spec = new CoursesSpec(Id);
            var course = await _uof.GetRepository<Guid,Course>().GetAsync(spec,Id,ct);
            return _mapper.Map<CourseResponse>(course);
        }

        public async Task<int> CreateCourseAsync(CreateCourseRequest request, CancellationToken ct)
        {
            // var categoryExists = await _uof.GetRepository<Guid,Category>().GetAsync(request.CategoryId) is not null;
            // if (!categoryExists) return 0;

            var course = _mapper.Map<Course>(request);
            await _uof.GetRepository<Guid,Course>().AddAsync(course);
            return await _uof.SaveChangesAsync(ct);
        }

        public async Task<int> UpdateCourseAsync(Guid Id, UpdateCourseRequest request, CancellationToken ct)
        {
            var spec = new CoursesSpec(Id);
            var courseExists = await _uof.GetRepository<Guid,Course>().GetAsync(spec,Id,ct);
            if (courseExists is null) return 0;

            var res = _mapper.Map(request,courseExists);
            _uof.GetRepository<Guid,Course>().Update(res);

            return await _uof.SaveChangesAsync(ct);
        }

        public async Task<int> DeleteCourseAsync(Guid Id, CancellationToken ct)
        {
           _uof.GetRepository<Guid,Course>().Delete(Id);
           return await _uof.SaveChangesAsync(ct);
        }
    }
}       