using Application.DTOs;

namespace Application.Services.Abstract;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto> GetByIdAsync(Guid id);
    Task<CategoryDto> CreateAsync(CreateCategoryDto category);
    Task<CategoryDto> UpdateAsync(CategoryDto category);
    Task DeleteAsync(Guid id);
    Task RemoveAsync(Guid id);

}
