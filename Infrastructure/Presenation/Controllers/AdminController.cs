using Domain.Entities.Courses.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS.Admin;
using Shared.ErrorModels;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet("requests")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InstructorRequestResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<IEnumerable<InstructorRequestResponse>>> GetInstructorRequests([FromQuery] RequestStatus? status, CancellationToken ct)
        {
            var requests = await serviceManager.AdminService.GetInstructorRequestsAsync(status, ct);
            return Ok(requests);
        }

        [HttpPut("requests/{id}/approve")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApproveInstructorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ApproveInstructorResponse>> ApproveRequest(Guid id, CancellationToken ct)
        {
            var result = await serviceManager.AdminService.ApproveRequestAsync(id, ct);
            return Ok(result);
        }

        [HttpPut("requests/{id}/reject")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApproveInstructorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<ApproveInstructorResponse>> RejectRequest(Guid id, CancellationToken ct)
        {
            var result = await serviceManager.AdminService.RejectRequestAsync(id, ct);
            return Ok(result);
        }
    }
}
