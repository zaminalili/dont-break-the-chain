using Application.DTOs;
using Application.Services.Abstract;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services.Concrete;

internal class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> logger;
    private readonly IMapper mapper;
    private readonly ICategoryRepository categoryRepository;

    public CategoryService(
        ILogger<CategoryService> logger, 
        IMapper mapper, 
        ICategoryRepository categoryRepository)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.categoryRepository = categoryRepository;
    }

    private async Task<Category> GetCategoryOrThrow(Guid id)
    {
        return await categoryRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Category), id);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto category)
    {
        var categoryEntity = mapper.Map<Category>(category);
        await categoryRepository.AddAsync(categoryEntity);
        
        logger.LogInformation("Category created with ID: {CategoryId}", categoryEntity.Id);

        return mapper.Map<CategoryDto>(categoryEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await GetCategoryOrThrow(id);

        await categoryRepository.DeleteAsync(id);
    }


    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllActiveAsync();
        logger.LogInformation("Retrieved categories.");

        return mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> GetByIdAsync(Guid id)
    {
        
        var category = await GetCategoryOrThrow(id);
        logger.LogInformation("Retrieved category with ID: {CategoryId}", id);
        
        return mapper.Map<CategoryDto>(category);
    }

    public async Task RemoveAsync(Guid id)
    {
        var category = await GetCategoryOrThrow(id);
        await categoryRepository.ChangeStatusAsync(id);
        
        logger.LogInformation("Category with ID: {CategoryId} removed.", id);
    }

    public async Task<CategoryDto> UpdateAsync(CategoryDto category)
    {
        var existingCategory = await GetCategoryOrThrow(category.Id);
        mapper.Map(existingCategory, category);
        
        await categoryRepository.UpdateAsync(existingCategory);
        logger.LogInformation("Category with ID: {CategoryId} updated.", category.Id);
        
        return mapper.Map<CategoryDto>(existingCategory);
    }
}
