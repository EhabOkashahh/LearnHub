using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.Courses.Enums;
using ServicesAbstraction.Courses;
using Shared.DTOS;
using Shared.DTOS.Courses;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Courses;
using Domain.Contracts;
using Services.Specifications.CoursesSpecifications;
using System.Globalization;

namespace Services
{
    public class CourseService(IUnitOfWork _uof, IMapper _mapper) : ICoursesService
    {
        public async Task<PaginatedResponse<CourseResponse>> GetAllCoursesAsync(CourseQueryParams queryParams,CancellationToken ct)
        {
            var spec = new CoursesSpec(queryParams);
            var courses = await _uof.GetRepository<Guid,Course>().GetAllAsync(spec,ct);
            var res = _mapper.Map<IEnumerable<CourseResponse>>(courses);
            

            var CountSpec = new CourseSpecifiationWihtoutPagination<Guid,Course>(queryParams);
            var totalCount = await _uof.GetRepository<Guid,Course>().GetCountAsync(CountSpec);

            return new PaginatedResponse<CourseResponse>(queryParams.PageIndex.Value, queryParams.PageSize.Value, totalCount,res);
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