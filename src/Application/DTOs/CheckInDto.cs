using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class CheckInDto
{
    public IFormFile Image { get; set; } = default!;
    public DateTime Date { get; set; }
}
