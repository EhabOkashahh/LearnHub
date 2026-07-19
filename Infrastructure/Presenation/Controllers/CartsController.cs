using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetCartAsync(string cartId)
        {
            var res = await _serviceManager.CartServices.GetCartAsync(cartId);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrUpdateCartAsync([FromBody] CartDto cart)
        {
            var res = await _serviceManager.CartServices.CreateCartAsync(cart, TimeSpan.FromDays(10));
            return Ok(res);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteCartAsync(string cartId)
        {
            await _serviceManager.CartServices.DeleteCartAsync(cartId);
            return NoContent();
        }
    }
}