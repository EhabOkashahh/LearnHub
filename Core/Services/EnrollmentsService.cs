using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.Courses;
using Domain.Entities.Identity;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Services.Specifications.CoursesSpecifications;
using ServicesAbstraction.Courses;
using Shared.DTOS.Courses;

namespace Services
{
    public class EnrollmentsService(IUnitOfWork _uof,UserManager<AppUser> _userManager) : IEnrollmentsService
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
            
            var EnrollmentSpec = new EnrollmentsSpec(studentId,CourseId);
            var StudentEnrollemnts = (await _uof.GetRepository<Guid,Enrollment>().GetAllAsync(EnrollmentSpec,ct)).Any();

            if(StudentEnrollemnts) throw new BadRequestException("You've already enrolled at this course");


            var EnrollmentEntity = new Enrollment()
            {
                CourseId = CourseId,
                StudentId = studentId,
            };
            await _uof.GetRepository<Guid,Enrollment>().AddAsync(EnrollmentEntity);
            await _uof.SaveChangesAsync(ct);
        }

        public Task GetMyEnrollmentsAsync(string StudentId, CourseQueryParams queryParams, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}