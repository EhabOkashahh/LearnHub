using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS;
using Shared.DTOS.Courses;
using Shared.ErrorModels;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/courses/{courseId}/sections/{sectionId}/lessons")]
    public class LessonsController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<LessonDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<IEnumerable<LessonDTO>>> GetAll(Guid courseId, Guid sectionId, CancellationToken ct)
        {
            var lessons = await serviceManager.LessonsService.GetAllAsync(courseId, sectionId, ct);
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LessonDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<LessonDTO>> GetById(Guid courseId, Guid sectionId, Guid id, CancellationToken ct)
        {
            var lesson = await serviceManager.LessonsService.GetByIdAsync(courseId, sectionId, id, ct);
            return Ok(lesson);
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Create(Guid courseId, Guid sectionId, [FromBody] CreateLessonRequest request, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await serviceManager.LessonsService.CreateAsync(courseId, sectionId, request, userId, ct);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Update(Guid courseId, Guid sectionId, Guid id, [FromBody] UpdateLessonRequest request, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await serviceManager.LessonsService.UpdateAsync(courseId, sectionId, id, request, userId, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Delete(Guid courseId, Guid sectionId, Guid id, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await serviceManager.LessonsService.DeleteAsync(courseId, sectionId, id, userId, ct);
            return NoContent();
        }

        [HttpPost("{id}/complete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Complete(Guid courseId, Guid sectionId, Guid id, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await serviceManager.LessonsService.CompleteAsync(id, userId, ct);
            return NoContent();
        }

        [HttpDelete("{id}/complete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Uncomplete(Guid courseId, Guid sectionId, Guid id, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await serviceManager.LessonsService.UncompleteAsync(id, userId, ct);
            return NoContent();
        }
    }
}