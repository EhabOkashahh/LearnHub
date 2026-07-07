using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Courses.Enums;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS;
using Shared.DTOS.Courses;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCourses([FromQuery]CourseQueryParams queryParams, CancellationToken ct)
        {
            var courses = await serviceManager.CourseService.GetAllCoursesAsync(queryParams,ct);
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid? id, CancellationToken ct)
        {
            var course = await serviceManager.CourseService.GetCourseByIdAsync(id!.Value , ct);
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody]CreateCourseRequest request, CancellationToken ct)
        {
             await serviceManager.CourseService.CreateCourseAsync(request, ct);
            return Ok();
        }

        [HttpPut("{id}")]   
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody]UpdateCourseRequest request, CancellationToken ct)
        {
            await serviceManager.CourseService.UpdateCourseAsync(id, request, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken ct)
        {
            await serviceManager.CourseService.DeleteCourseAsync(id, ct);
            return NoContent();
        }
    }
}