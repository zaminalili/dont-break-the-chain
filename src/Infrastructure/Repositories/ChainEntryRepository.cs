using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Extensions;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

internal class ChainEntryRepository : RepositoryBase<ChainEntry>, IChainEntryRepository
{
    public ChainEntryRepository(DBChDbContext context) : base(context)
    {
    }

    public async Task<(IEnumerable<ChainEntry>, int)> GetByChainIdAsync(Guid chainId, int pageNumber, int pageSize)
    {
        var baseQuery = dbSet
            .Where(ch => ch.ChainId == chainId);

        return await baseQuery.PaginateAsync(pageNumber, pageSize);
    }
}
