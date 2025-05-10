using Application.Services.Abstract;
using Application.Services.Concrete;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMapster(); 

            services.AddScoped<IChainService, ChainService>();
            services.AddScoped<IChainEntryService, ChainEntryService>();
        }
    }
}
