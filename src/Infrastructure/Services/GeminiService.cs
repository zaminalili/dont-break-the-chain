using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Infrastructure.Extensions;

namespace Infrastructure.Services;

internal class GeminiService : IImageValidator
{
    private readonly HttpClient httpClient;
    private readonly string apiKey;
    private readonly string url;

    public GeminiService(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        apiKey = configuration["GeminiSettings:ApiKey"] ?? "";
        url = configuration["GeminiSettings:BaseUrl"] + apiKey;
    }

    public async Task<bool> IsMatchAsync(IFormFile image, string description)
    {
        using var ms = new MemoryStream();
        await image.CopyToAsync(ms);
        
        var base64Image = Convert.ToBase64String(ms.ToArray());
        var requestBody = ResponseBody(image, description, base64Image);
        var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);

        var text = await response.GetResponseTextAsync();

        return text?.ToLower().Contains("yes") ?? false;
    }


    private static object ResponseBody(IFormFile image, string description, string base64Image) => new
    {
        contents = new[]
        {
            new 
            {
                parts = new object[]
                {
                    new { text = $"Does this image reflect the description below? '{description}'. Just answer 'yes' or 'no'." },
                    new {
                        inlineData = new {
                            mimeType = image.ContentType,
                            data = base64Image
                        }
                    }
                }
            }
        }
    };
}
