using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS.Courses;
using Shared.ErrorModels;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet("my-courses")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<CourseResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<PaginatedResponse<CourseResponse>>> GetMyCourses([FromQuery] CourseQueryParams queryParams, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var courses = await serviceManager.EnrollmentsService.GetMyEnrollmentsAsync(userId, queryParams, ct);
            return Ok(courses);
        }
    }
}
