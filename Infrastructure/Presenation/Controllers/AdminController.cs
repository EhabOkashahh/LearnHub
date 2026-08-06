using Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS;
using Shared.DTOS.Admin;
using Shared.DTOS.Courses;
using Shared.ErrorModels;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet("requests")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedResponse<InstructorRequestResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<PaginatedResponse<InstructorRequestResponse>>> GetInstructorRequests(
            [FromQuery] RequestStatus? status,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var requests = await serviceManager.AdminService.GetInstructorRequestsAsync(status, pageIndex, pageSize, ct);
            return Ok(requests);
        }

        [HttpPut("requests/{id}/approve")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ApproveRequest(Guid id, CancellationToken ct)
        {
            await serviceManager.AdminService.ApproveRequestAsync(id, ct);
            return NoContent();
        }

        [HttpPut("requests/{id}/reject")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RejectRequest(Guid id, CancellationToken ct)
        {
            await serviceManager.AdminService.RejectRequestAsync(id, ct);
            return NoContent();
        }

        [HttpDelete("users/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteUser(string id, CancellationToken ct)
        {
            await serviceManager.UserService.DeleteUserAsync(id, ct);
            return NoContent();
        }
    }
}
