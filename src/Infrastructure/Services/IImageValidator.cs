using Infrastructure.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public interface IImageValidator
{
    Task<bool> IsMatchAsync(IFormFile image, string description);
}
