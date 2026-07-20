using AutoMapper;
using ServicesAbstraction.Courses;
using Shared.DTOS.Courses;
using Domain.Entities.Courses;
using Domain.Entities.Courses.Enums;
using Domain.Contracts;
using Services.Specifications.CoursesSpecifications;
using Domain.Exceptions.NotFoundExceptions;
using Services.Specifications.CategorySpecifications;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Domain.Exceptions.BadRequestExceptions;
using Microsoft.EntityFrameworkCore;

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

            return new PaginatedResponse<CourseResponse>(queryParams.PageIndex!.Value, queryParams.PageSize!.Value, totalCount,res);
        }


        public async Task<PaginatedResponse<CourseResponse>> GetInstructorCoursesAsync(string userId, CourseQueryParams queryParams, CancellationToken ct)
        {
            var spec = new CoursesSpec(queryParams, userId);
            var courses = await _uof.GetRepository<Guid, Course>().GetAllAsync(spec, ct);
            var res = _mapper.Map<IEnumerable<CourseResponse>>(courses);

            var countSpec = new CoursesSpec(userId);
            var totalCount = await _uof.GetRepository<Guid, Course>().GetCountAsync(countSpec);

            return new PaginatedResponse<CourseResponse>(queryParams.PageIndex!.Value, queryParams.PageSize!.Value, totalCount, res);
        }

        public async Task<CourseResponse?> GetCourseByIdAsync(Guid Id, CancellationToken ct)
        {
            var spec = new CoursesSpec(Id,true);
            var course = await _uof.GetRepository<Guid,Course>().GetAsync(spec,Id,ct);

            if(course is null) throw new CourseNotFoundException(Id);
            
            var MappedCourses = _mapper.Map<CourseResponse>(course);
            return MappedCourses;
        }

        public async Task<Guid> CreateCourseAsync(string instructorId, CreateCourseRequest request,CancellationToken ct)
        {
            var CatSpec = new CategorySpec(request.CategoryId);
            var categoryExists = await _uof.GetRepository<Guid,Category>().IsExsists(CatSpec);
            if (!categoryExists) throw new CateoryNotFoundException(request.CategoryId);


            var course = _mapper.Map<Course>(request);
            course.InstructorId = instructorId;
            await _uof.GetRepository<Guid,Course>().AddAsync(course);
            await _uof.SaveChangesAsync(ct);
            return course.Id;
        }

        public async Task UpdateCourseAsync(Guid Id, UpdateCourseRequest request, string userId, CancellationToken ct)
        {
            var spec = new CoursesSpec(Id,userId);
            var course = await _uof.GetRepository<Guid,Course>().GetAsync(spec,Id,ct);

            if(course is null) throw new CourseNotFoundException(Id);

            _mapper.Map(request,course);
            course.UpdatedAt = DateTime.UtcNow;
            await _uof.SaveChangesAsync(ct);
        }

        public async Task<CourseProgressResponse> GetProgressAsync(Guid courseId, string userId, CancellationToken ct)
        {
            var totalLessons = await _uof.GetRepository<Guid, Lesson>().GetCountAsync(new LessonSpec());
            if (totalLessons == 0)
                return new CourseProgressResponse { TotalLessons = 0, CompletedLessons = 0, Percentage = 0 };

            var completedSpec = new LessonProgressSpec(userId);
            var completedLessons = await _uof.GetRepository<Guid, LessonProgress>().GetCountAsync(completedSpec);

            var percentage = Math.Round((double)completedLessons / totalLessons * 100, 1);
            return new CourseProgressResponse
            {
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                Percentage = percentage
            };
        }

        public async Task PublishCourseAsync(Guid Id, string userId, CancellationToken ct)
        {
            var spec = new CoursesSpec(Id, userId);
            var course = await _uof.GetRepository<Guid, Course>().GetAsync(spec, Id, ct);
            if (course is null) throw new CourseNotFoundException(Id);
            if (course.Status == CourseStatus.Published)
                throw new BadRequestException("Course is already published");

            course.Status = CourseStatus.Published;
            course.UpdatedAt = DateTime.UtcNow;
            await _uof.SaveChangesAsync(ct);
        }

        public async Task UnpublishCourseAsync(Guid Id, string userId, CancellationToken ct)
        {
            var spec = new CoursesSpec(Id, userId);
            var course = await _uof.GetRepository<Guid, Course>().GetAsync(spec, Id, ct);
            if (course is null) throw new CourseNotFoundException(Id);
            if (course.Status == CourseStatus.Draft)
                throw new BadRequestException("Course is already a draft");

            course.Status = CourseStatus.Draft;
            course.UpdatedAt = DateTime.UtcNow;
            await _uof.SaveChangesAsync(ct);
        }

        public async Task DeleteCourseAsync(Guid Id, string userId, CancellationToken ct)
        {
            var spec = new CoursesSpec(Id,false);
            var course = await _uof.GetRepository<Guid,Course>().GetAsync(spec,Id,ct);

            if(course is null) throw new CourseNotFoundException(Id);
            if (course.InstructorId != userId) throw new UnauthorizedAccessException("You can only delete your own courses");

           _uof.GetRepository<Guid,Course>().Delete(Id);
           await _uof.SaveChangesAsync(ct);
        }
    }
}       