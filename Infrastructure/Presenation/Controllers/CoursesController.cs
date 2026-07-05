using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCourses(CancellationToken ct)
        {
            var courses = await serviceManager.CourseService.GetAllCoursesAsync(ct);
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid? id)
        {
            if(id == null) return BadRequest();

            var course = await serviceManager.CourseService.GetCourseByIdAsync(id.Value , CancellationToken.None);
            if(course == null) return NotFound();

            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody]CreateCourseRequest request, CancellationToken ct)
        {
            if(request == null) return BadRequest();

            var rowsAffected = await serviceManager.CourseService.CreateCourseAsync(request, ct);
            if(rowsAffected == 0) return BadRequest("Category does not exist");

            return Ok();
        }

        [HttpPut("{id}")]   
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody]UpdateCourseRequest request, CancellationToken ct)
        {
            var rowsAffected = await serviceManager.CourseService.UpdateCourseAsync(id, request, ct);
            if(rowsAffected == 0) return NotFound();    

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken ct)
        {
            var rowsAffected = await serviceManager.CourseService.DeleteCourseAsync(id, ct);
            if(rowsAffected == 0) return NotFound();

            return NoContent();
        }
    }
}