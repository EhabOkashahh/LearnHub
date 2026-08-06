using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Courses;
using Domain.Entities.Identity;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Identity;
using Services.Specifications.CoursesSpecifications;
using ServicesAbstraction.Courses;
using Shared.DTOS;
using Shared.DTOS.Courses;

namespace Services
{
    public class EnrollmentsService(IUnitOfWork _uof,UserManager<AppUser> _userManager, IMapper _mapper) : IEnrollmentsService
    {
        public async Task EnrollAsync(string studentId, Guid CourseId, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(studentId);
            if(user is null) throw new UserNotFoundException(studentId);

            var CourseSpec = new CoursesSpec(CourseId);
            var course = await _uof.GetRepository<Guid,Course>().GetAsync(CourseSpec,ct);
            if(course is null) throw new CourseNotFoundException(CourseId);

            var IsCourseInstructor = course.InstructorId == studentId;

            if(IsCourseInstructor) throw new BadRequestException("You cannot enroll in your own course");
            
            var IsEnroll = await IsEnrolledAsync(studentId,CourseId,ct);

            if(IsEnroll) throw new BadRequestException("You've already enrolled at this course");


            var EnrollmentEntity = new Enrollment()
            {
                CourseId = CourseId,
                StudentId = studentId,
            };
            await _uof.GetRepository<Guid,Enrollment>().AddAsync(EnrollmentEntity);
            await _uof.SaveChangesAsync(ct);
        }

        public async Task<PaginatedResponse<CourseResponse>> GetMyEnrollmentsAsync(string StudentId, CourseQueryParams queryParams, CancellationToken ct)
        {
            var EnrollmentSpec = new EnrollmentsSpec(StudentId,queryParams);
            var enrollments = await _uof.GetRepository<Guid,Enrollment>().GetAllAsync(EnrollmentSpec,ct);
            var courses = _mapper.Map<IEnumerable<CourseResponse>>(enrollments.Select(x =>x.Course));

            var CountSpec = new EnrollmentsSpec(StudentId, queryParams, paginated: false);
            var totalCount = await _uof.GetRepository<Guid,Enrollment>().GetCountAsync(CountSpec);

            return new PaginatedResponse<CourseResponse>(pageIndex: queryParams.PageIndex!.Value,pageSize: queryParams.PageSize!.Value,totalCount:totalCount,data:courses);
        }

        private async Task<bool> IsEnrolledAsync(string studentId, Guid courseId, CancellationToken ct)
        {
            var spec = new EnrollmentsSpec(studentId, courseId);
            return await _uof.GetRepository<Guid, Enrollment>().Exists(spec);
        }
    }
}