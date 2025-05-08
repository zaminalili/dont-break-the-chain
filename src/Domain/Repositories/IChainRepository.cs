using Domain.Entities;

namespace Domain.Repositories;

public interface IChainRepository : IRepositoryBase<Chain>
{
    Task<(IEnumerable<Chain>, int)> GetAllChainsByUserAsync(Guid userId, int pageNumber = 1, int pageSize = 3);
    Task<(IEnumerable<Chain>, int)> GetChainsByUserAsync(Guid userId, bool isPublic = true, int pageNumber = 1, int pageSize = 3);
    Task<(IEnumerable<Chain>, int)> GetChainsByUserAndCategoryAsync(Guid userId, Guid categoryId, bool isPublic = true, int pageNumber = 1, int pageSize = 3);
    Task<(IEnumerable<Chain>, int)> GetPublicChainsByCategoryAsync(Guid categoryId, int pageNumber = 1, int pageSize = 15);
    Task<(IEnumerable<Chain>, int)> GetPublicChainsByChallengeAsync(Guid challengeId, Guid categoryId, int pageNumber = 1, int pageSize = 15);

    Task<int> GetChainCountAsync();
    Task<int> GetChainCountAsync(Guid categoryId);
}
