using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly DBChDbContext dbContext;
    protected readonly DbSet<T> dbSet;
    public RepositoryBase(DBChDbContext dbContext)
    {
        this.dbContext = dbContext;
        dbSet = dbContext.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        dbSet.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            dbSet.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistAsync(Guid id)
    {
        return await GetByIdAsync(id) != null;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await dbSet.FindAsync(id);
    }

    public async Task UpdateAsync(T entity)
    {
        dbSet.Update(entity);
        await dbContext.SaveChangesAsync();
    }
}
