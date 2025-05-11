using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;

internal static class QueryableExtensions
{
    public static async Task<(IEnumerable<T>, int)> PaginateAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
