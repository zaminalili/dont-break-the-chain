using System.Text.Json;

namespace Infrastructure.Extensions;

public static class HttpResponseExtensions
{
    public static async Task<string?> GetResponseTextAsync(this HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();

        var responseStr = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseStr);

        var text = doc.RootElement
                      .GetProperty("candidates")[0]
                      .GetProperty("content")
                      .GetProperty("parts")[0]
                      .GetProperty("text")
                      .GetString();

        return text;
    }
}
