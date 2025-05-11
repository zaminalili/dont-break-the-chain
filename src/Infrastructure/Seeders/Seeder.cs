
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seeders;

internal class Seeder(ILogger<Seeder> logger, DBChDbContext dbContext) : ISeeder
{
    public async Task Seed()
    {
        logger.LogInformation("Check database connection for seed datas");
        if (await dbContext.Database.CanConnectAsync())
        {
            await AddCategoriesToDbContext();

            await dbContext.SaveChangesAsync();
        }
    }

    private async Task AddCategoriesToDbContext()
    {
        if (!dbContext.Categories.Any())
        {
            logger.LogInformation($"Seed data for {nameof(Category)}");
            var categories = GetCategories();
            await dbContext.Categories.AddRangeAsync(categories);
        }
    }

    private ICollection<Category> GetCategories()
    {
        List<Category> categories =
            [
                new() { Name = "Self improvment" },
                new() { Name = "Physical Health" },
                new() { Name = "Mental Health" },
                new() { Name = "Education and Career" },
                new() { Name = "Productivity and Notes" },
                new() { Name = "Daily Routines and Housework" },
                new() { Name = "Financial Regulation" },
                new() { Name = "Relationships and Social Life" },
                new() { Name = "Hobbies and Creativity" },

            ];

        return categories;
    }
}
