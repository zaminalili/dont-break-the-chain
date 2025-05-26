using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CategoryDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string Name { get; set; } = default!;
    public string? Icon { get; set; }
}
