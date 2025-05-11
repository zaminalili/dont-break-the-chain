using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class ChainRepository : RepositoryBase<Chain>, IChainRepository
{
    public ChainRepository(DBChDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<int> GetChainCountAsync()
    {
        return await dbSet.CountAsync();
    }

    public async Task<int> GetChainCountAsync(Guid categoryId)
    {
        return await dbSet
            .Where(ch => ch.CategoryId == categoryId)
            .CountAsync();
    }

    public async Task<(IEnumerable<Chain>, int)> GetPublicChainsByChallengeAsync(Guid callengeId, Guid categoryId, int pageNumber = 1, int pageSize = 15)
    {
        var baseQuery = dbSet
                .Where(ch => ch.ChallengeId == callengeId
                            && ch.CategoryId == categoryId
                            && ch.IsPublic == true);

        return await baseQuery.PaginateAsync(pageNumber, pageSize);
    }



    public async Task<(IEnumerable<Chain>, int)> GetPublicChainsByCategoryAsync(Guid categoryId, int pageNumber = 1, int pageSize = 15)
    {
        var baseQuery = dbSet
            .Where(ch => ch.CategoryId == categoryId
                         && ch.IsPublic == true);

        return await baseQuery.PaginateAsync(pageNumber, pageSize);
    }

    public async Task<(IEnumerable<Chain>, int)> GetChainsByUserAndCategoryAsync(Guid userId, Guid categoryId, bool isPublic = true, int pageNumber = 1, int pageSize = 3)
    {
        var baseQuery = dbSet
            .Where(ch => ch.UserId == userId
                     && ch.CategoryId == categoryId
                     && ch.IsPublic == isPublic);

        return await baseQuery.PaginateAsync(pageNumber, pageSize);
    }

    public async Task<(IEnumerable<Chain>, int)> GetAllChainsByUserAsync(Guid userId, int pageNumber = 1, int pageSize = 3)
    {
        var baseQuery = dbSet
            .Where(ch => ch.UserId == userId);

        return await baseQuery.PaginateAsync(pageNumber, pageSize);
    }

    public async Task<(IEnumerable<Chain>, int)> GetChainsByUserAsync(Guid userId, bool isPublic = true, int pageNumber = 1, int pageSize = 3)
    {
        var baseQuery = dbSet
            .Where(ch => ch.UserId == userId
                     && ch.IsPublic == isPublic);

        return await baseQuery.PaginateAsync(pageNumber, pageSize);
    }
}
