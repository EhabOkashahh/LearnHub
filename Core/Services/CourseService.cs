using AutoMapper;
using ServicesAbstraction.Courses;
using Shared.DTOS.Courses;
using Domain.Entities.Courses;
using Domain.Contracts;
using Services.Specifications.CoursesSpecifications;
using Domain.Exceptions.NotFoundExceptions;
using Services.Specifications.CategorySpecifications;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Domain.Exceptions.BadRequestExceptions;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class CourseService(IUnitOfWork _uof, IMapper _mapper, UserManager<AppUser> _userManager) : ICoursesService
    {
        public async Task<PaginatedResponse<CourseResponse>> GetAllCoursesAsync(CourseQueryParams queryParams,CancellationToken ct)
        {
            var spec = new CoursesSpec(queryParams);
            var courses = await _uof.GetRepository<Guid,Course>().GetAllAsync(spec,ct);
            var res = _mapper.Map<IEnumerable<CourseResponse>>(courses);
            

            var CountSpec = new CourseSpecifiationWihtoutPagination<Guid,Course>(queryParams);
            var totalCount = await _uof.GetRepository<Guid,Course>().GetCountAsync(CountSpec);

            return new PaginatedResponse<CourseResponse>(queryParams.PageIndex!.Value, queryParams.PageSize!.Value, totalCount,res);
        }


        public async Task<CourseResponse?> GetCourseByIdAsync(Guid Id, CancellationToken ct)
        {
            var spec = new CoursesSpec(Id);
            var course = await _uof.GetRepository<Guid,Course>().GetAsync(spec,Id,ct);

            if(course is null) throw new CourseNotFoundException(Id);

            return _mapper.Map<CourseResponse>(course);
        }

        public async Task CreateCourseAsync(CreateCourseRequest request,CancellationToken ct)
        {
            var CatSpec = new CategorySpec(request.CategoryId);
            var categoryExists = await _uof.GetRepository<Guid,Category>().IsExsists(CatSpec);
            if (!categoryExists) throw new CateoryNotFoundException(request.CategoryId);


            var course = _mapper.Map<Course>(request);
            await _uof.GetRepository<Guid,Course>().AddAsync(course);
            await _uof.SaveChangesAsync(ct);
        }

        public async Task UpdateCourseAsync(Guid Id, UpdateCourseRequest request, CancellationToken ct)
        {
            var spec = new CourseByIdSpec(Id);
            var course = await _uof.GetRepository<Guid,Course>().GetAsync(spec,Id,ct);

            if(course is null) throw new CourseNotFoundException(Id);

            _mapper.Map(request,course);
        }

        public async Task DeleteCourseAsync(Guid Id, CancellationToken ct)
        {
            var spec = new CourseByIdSpec(Id);
            var exists = await _uof.GetRepository<Guid,Course>().IsExsists(spec);

            if(!exists) throw new CourseNotFoundException(Id);

           _uof.GetRepository<Guid,Course>().Delete(Id);
        }
    }
}       