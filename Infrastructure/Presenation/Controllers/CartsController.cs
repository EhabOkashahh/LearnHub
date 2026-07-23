using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS.Cart;
using Shared.ErrorModels;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<ActionResult<CartResponse>> GetCart(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var cart = await _serviceManager.CartServices.GetCartAsync(userId, ct);
            return Ok(cart);
        }

        [HttpPost("items")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemRequest request, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _serviceManager.CartServices.AddItemAsync(userId, request.CourseId, ct);
            return NoContent();
        }

        [HttpDelete("items/{courseId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> RemoveItem(Guid courseId, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _serviceManager.CartServices.RemoveItemAsync(userId, courseId, ct);
            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> ClearCart(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _serviceManager.CartServices.ClearCartAsync(userId, ct);
            return NoContent();
        }
    }
}
