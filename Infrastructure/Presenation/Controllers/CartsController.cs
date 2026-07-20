using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS.Cart;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCartAsync()
        {
            var cartId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var res = await _serviceManager.CartServices.GetCartAsync(cartId);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrUpdateCartAsync([FromBody] CartDto cart)
        {
            cart.Id = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var res = await _serviceManager.CartServices.CreateCartAsync(cart, TimeSpan.FromDays(10));
            return Ok(res);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteCartAsync()
        {
            var cartId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _serviceManager.CartServices.DeleteCartAsync(cartId);
            return NoContent();
        }
    }
}