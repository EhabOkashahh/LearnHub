using System.Security.Claims;
using Domain.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presenation.CustomAttributes;
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
        [Cache(300)]
        public async Task<ActionResult<PaginatedResponse<CourseResponse>>> GetAllCourses([FromQuery]CourseQueryParams queryParams, CancellationToken ct)
        {
            var courses = await serviceManager.CourseService.GetAllCoursesAsync(queryParams,ct);
            return Ok(courses);
        }

        [HttpGet("mine")]
        [Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<CourseResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        public async Task<ActionResult<PaginatedResponse<CourseResponse>>> GetMyCourses([FromQuery] CourseQueryParams queryParams, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var courses = await serviceManager.CourseService.GetInstructorCoursesAsync(userId, queryParams, ct);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [Authorize(Roles = "Instructor")]

        public async Task<IActionResult> CreateCourse([FromBody]CreateCourseRequest request, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var courseId = await serviceManager.CourseService.CreateCourseAsync(userId, request, ct);
            return CreatedAtAction(nameof(GetCourseById), new { id = courseId }, null);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody]UpdateCourseRequest request, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await serviceManager.CourseService.UpdateCourseAsync(id, request, userId, ct);
            return NoContent();
        }

        [HttpPut("{id}/publish")]
        [Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> PublishCourse(Guid id, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await serviceManager.CourseService.PublishCourseAsync(id, userId, ct);
            return NoContent();
        }

        [HttpPut("{id}/unpublish")]
        [Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UnpublishCourse(Guid id, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await serviceManager.CourseService.UnpublishCourseAsync(id, userId, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await serviceManager.CourseService.DeleteCourseAsync(id, userId, ct);
            return NoContent();
        }

        [HttpGet("{courseId}/progress")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseProgressResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<CourseProgressResponse>> GetProgress(Guid courseId, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var progress = await serviceManager.CourseService.GetProgressAsync(courseId, userId, ct);
            return Ok(progress);
        }
    }
}