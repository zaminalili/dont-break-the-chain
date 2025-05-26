using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Seeders;
using Infrastructure.Services;


namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // Configure database context
            services.AddDbContext<DBChDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            // Other services like repositories, caching, etc.

            services.AddIdentityCore<User>()
                    .AddRoles<Role>()
                    .AddEntityFrameworkStores<DBChDbContext>();

            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

            services.AddScoped<IChainRepository, ChainRepository>();
            services.AddScoped<IChainEntryRepository, ChainEntryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<ISeeder, Seeder>();

           
            services.AddScoped<IImageValidator, GeminiService>();

        }
    }
}
