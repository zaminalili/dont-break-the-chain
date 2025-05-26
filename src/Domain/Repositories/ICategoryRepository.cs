using Domain.Entities;

namespace Domain.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task ChangeStatusAsync(Guid id);
    Task<IEnumerable<Category>> GetAllActiveAsync();
    Task<IEnumerable<Category>> GetAllInactiveAsync();
}
