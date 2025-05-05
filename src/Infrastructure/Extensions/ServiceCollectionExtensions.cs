using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;

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
        }
    }
}
