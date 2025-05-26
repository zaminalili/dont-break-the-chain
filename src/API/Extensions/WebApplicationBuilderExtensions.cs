using API.Attributes;
using API.Middlewares;
using Domain.Entities;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddPresentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("FixedPolicy", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 100;
                    opt.QueueLimit = 2;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
            });

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelAttribute>();
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddIdentityApiEndpoints<User>();
            builder.Services.AddAuthentication();

            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddHttpClient();
        }
    }
}
