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
        public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken ct)
        {
            var category = await serviceManager.CategoryService.GetCategoryByIdAsync(id, ct);
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request, CancellationToken ct)
        {
            await serviceManager.CategoryService.CreateCategoryAsync(request, ct);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
        {
            await serviceManager.CategoryService.UpdateCategoryAsync(id, request, ct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken ct)
        {
            await serviceManager.CategoryService.DeleteCategoryAsync(id, ct);
            return NoContent();
        }
    }
}
