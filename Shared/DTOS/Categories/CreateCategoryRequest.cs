using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.Categories
{
    public class CreateCategoryRequest
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;
    }
}
