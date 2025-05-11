using Domain.Entities;

namespace Domain.Repositories;

public interface IChainEntryRepository : IRepositoryBase<ChainEntry>
{
    Task<(IEnumerable<ChainEntry>, int)> GetByChainIdAsync(Guid chainId, int pageNumber, int pageSize);
}
