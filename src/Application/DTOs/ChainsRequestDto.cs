using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class ChainsRequestDto
{
    [Required]
    public Guid Id { get; set; }

    [Range(1, 100)]
    public int PageNumber { get; set; } = 1;

    [Range(1, 30)]
    public int PageSize { get; set; } = 10;
}
