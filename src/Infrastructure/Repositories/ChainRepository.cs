using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class ChainRepository : RepositoryBase<Chain>, IChainRepository
{
    public ChainRepository(DBChDbContext dbContext) : base(dbContext)
    {
    }

    private async Task<(IEnumerable<Chain>, int)> PaginateChains(int pageNumber, int pageSize, IQueryable<Chain> baseQuery)
    {
        int totalCount = await baseQuery.CountAsync();

        var chains = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (chains, totalCount);
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

        return await PaginateChains(pageNumber, pageSize, baseQuery);
    }



    public async Task<(IEnumerable<Chain>, int)> GetPublicChainsByCategoryAsync(Guid categoryId, int pageNumber = 1, int pageSize = 15)
    {
        var baseQuery = dbSet
            .Where(ch => ch.CategoryId == categoryId
                         && ch.IsPublic == true);

        return await PaginateChains(pageNumber, pageSize, baseQuery);
    }

    public async Task<(IEnumerable<Chain>, int)> GetChainsByUserAndCategoryAsync(Guid userId, Guid categoryId, bool isPublic = true, int pageNumber = 1, int pageSize = 3)
    {
        var baseQuery = dbSet
            .Where(ch => ch.UserId == userId
                     && ch.CategoryId == categoryId
                     && ch.IsPublic == isPublic);

        return await PaginateChains(pageNumber, pageSize, baseQuery);
    }

    public async Task<(IEnumerable<Chain>, int)> GetAllChainsByUserAsync(Guid userId, int pageNumber = 1, int pageSize = 3)
    {
        var baseQuery = dbSet
            .Where(ch => ch.UserId == userId);

        return await PaginateChains(pageNumber, pageSize, baseQuery);
    }

    public async Task<(IEnumerable<Chain>, int)> GetChainsByUserAsync(Guid userId, bool isPublic = true, int pageNumber = 1, int pageSize = 3)
    {
        var baseQuery = dbSet
            .Where(ch => ch.UserId == userId
                     && ch.IsPublic == isPublic);

        return await PaginateChains(pageNumber, pageSize, baseQuery);
    }
}
