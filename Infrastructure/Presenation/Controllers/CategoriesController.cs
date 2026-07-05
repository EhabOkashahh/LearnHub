using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DTOS;
using Shared.DTOS.Categories;

namespace Presenation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllCategories(CancellationToken ct)
        {
            var categories = await serviceManager.CategoryService.GetAllCategoriesAsync(ct);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid? id, CancellationToken ct)
        {
            if (id == null) return BadRequest();

            var category = await serviceManager.CategoryService.GetCategoryByIdAsync(id.Value, ct);
            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken ct)
        {
            if (request == null) return BadRequest();

            var rowsAffected = await serviceManager.CategoryService.CreateCategoryAsync(request, ct);
            if (rowsAffected == 0) return BadRequest("Failed to create category");

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
        {
            var rowsAffected = await serviceManager.CategoryService.UpdateCategoryAsync(id, request, ct);
            if (rowsAffected == 0) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken ct)
        {
            var rowsAffected = await serviceManager.CategoryService.DeleteCategoryAsync(id, ct);
            if (rowsAffected == 0) return NotFound();

            return NoContent();
        }
    }
}
