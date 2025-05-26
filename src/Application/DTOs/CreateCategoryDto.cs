using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CreateCategoryDto
{
    [Required]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
    public string Name { get; set; } = default!;
    public string? Icon { get; set; }
}
