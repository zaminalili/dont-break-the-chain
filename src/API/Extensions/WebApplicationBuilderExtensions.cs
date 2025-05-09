using API.Attributes;
using API.Middlewares;
using Domain.Entities;

namespace API.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static void AddPresentation(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidateModelAttribute>();
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddIdentityApiEndpoints<User>();
            builder.Services.AddAuthentication();

            builder.Services.AddScoped<ErrorHandlingMiddleware>();
        }
    }
}
