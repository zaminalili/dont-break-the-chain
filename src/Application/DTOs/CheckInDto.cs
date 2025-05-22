using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class CheckInDto
{
    public Guid ChainId { get; set; }
    public string CategoryName { get; set; } = default!;
    public IFormFile Image { get; set; } = default!;
    public DateTime Date { get; set; }
    public string? Note { get; set; }
}
