using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
{
    public CategoryRepository(DBChDbContext dbContext) : base(dbContext)
    {
    }

    public async Task ChangeStatusAsync(Guid id)
    {
        var category = await GetByIdAsync(id);
        if (category != null)
        {
            category.IsDeactive = !category.IsDeactive;
            await UpdateAsync(category);
        }
    }

    public async Task<IEnumerable<Category>> GetAllActiveAsync()
    {
        return await dbSet
            .Where(c => !c.IsDeactive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllInactiveAsync()
    {
        return await dbSet
            .Where(c => c.IsDeactive)
            .ToListAsync();
    }
}
