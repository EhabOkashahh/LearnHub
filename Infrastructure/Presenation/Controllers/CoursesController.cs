using Domain.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS.Courses;
using Shared.ErrorModels;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<CourseResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        public async Task<ActionResult<PaginatedResponse<CourseResponse>>> GetAllCourses([FromQuery]CourseQueryParams queryParams, CancellationToken ct)
        {
            var courses = await serviceManager.CourseService.GetAllCoursesAsync(queryParams,ct);
            return Ok(courses);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<CourseResponse>> GetCourseById(Guid? id, CancellationToken ct)
        {
            var course = await serviceManager.CourseService.GetCourseByIdAsync(id!.Value , ct);
            return Ok(course);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> CreateCourse([FromBody]CreateCourseRequest request, CancellationToken ct)
        {
             await serviceManager.CourseService.CreateCourseAsync(request, ct);
            return Ok();
        }

        [HttpPut("{id}")]   
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody]UpdateCourseRequest request, CancellationToken ct)
        {
            await serviceManager.CourseService.UpdateCourseAsync(id, request, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken ct)
        {
            await serviceManager.CourseService.DeleteCourseAsync(id, ct);
            return NoContent();
        }
    }
}