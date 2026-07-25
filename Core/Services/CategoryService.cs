using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Courses;
using Domain.Exceptions.NotFoundExceptions;
using ServicesAbstraction.Categories;
using Services.Specifications.CategorySpecifications;
using Shared.DTOS;
using Shared.DTOS.Categories;

namespace Services
{
    public class CategoryService(IUnitOfWork _uof, IMapper _mapper) : ICategoriesService
    {
        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync(CancellationToken ct)
        {
            var spec = new CategorySpec();
            var categories = await _uof.GetRepository<Guid, Category>().GetAllAsync(spec, ct);
            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse?> GetCategoryByIdAsync(Guid Id, CancellationToken ct)
        {
            var spec = new CategorySpec(Id);
            var category = await _uof.GetRepository<Guid, Category>().GetAsync(spec, ct);
            if (category is null) throw new CategoryNotFoundException(Id);

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct)
        {
            var category = _mapper.Map<Category>(request);
            await _uof.GetRepository<Guid, Category>().AddAsync(category);
            await _uof.SaveChangesAsync(ct);
        }

        public async Task UpdateCategoryAsync(Guid Id, UpdateCategoryRequest request, CancellationToken ct)
        {
            var spec = new CategorySpec(Id);
            var category = await _uof.GetRepository<Guid, Category>().GetAsync(spec, ct);
            if (category is null) throw new CategoryNotFoundException(Id);

            _mapper.Map(request, category);
            category.UpdatedAt = DateTime.UtcNow;
            await _uof.SaveChangesAsync(ct);
        }

        public async Task DeleteCategoryAsync(Guid Id, CancellationToken ct)
        {
            var spec = new CategorySpec(Id);
            var category = await _uof.GetRepository<Guid, Category>().GetAsync(spec, ct);
            if (category is null) throw new CategoryNotFoundException(Id);

            _uof.GetRepository<Guid, Category>().Delete(Id);
            await _uof.SaveChangesAsync(ct);
        }
    }
}
