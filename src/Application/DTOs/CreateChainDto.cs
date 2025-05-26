using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CreateChainDto
{
    [Required]
    [MaxLength(20)]
    [MinLength(3)]
    public string Title { get; set; } = default!;

    [MaxLength(250)]
    public string? Description { get; set; }
    public bool IsPublic { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    
    [Required]
    public Guid CategoryId { get; set; }
}
