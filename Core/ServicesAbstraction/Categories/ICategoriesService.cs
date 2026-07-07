using Shared.DTOS.Categories;

namespace ServicesAbstraction.Categories
{
    public interface ICategoriesService
    {
        Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(CancellationToken cancellationToken);
        Task<CategoryResponse?> GetCategoryByIdAsync(Guid Id, CancellationToken cancellationToken);
        Task CreateCategoryAsync(CreateCategoryRequest request, CancellationToken cancellationToken);
        Task UpdateCategoryAsync(Guid Id, UpdateCategoryRequest request, CancellationToken cancellationToken);
        Task DeleteCategoryAsync(Guid Id, CancellationToken cancellationToken);
    }
}
