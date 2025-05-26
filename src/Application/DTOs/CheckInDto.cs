using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Validation.Attributes;

namespace Application.DTOs;

public class CheckInDto
{
    
    [Required(ErrorMessage = "Category name is required.")]
    public string CategoryName { get; set; } = default!;

    
    [Required(ErrorMessage = "Image is required.")]
    [MaxFileSize(5 * 1024*1024, ErrorMessage = "Image size must not exceed 5 MB.")]
    [AllowedExtensions([".heic", "jpg", ".jpeg"], ErrorMessage = "Only .heic, .jpg, .jpeg files are allowed.")]
    public IFormFile Image { get; set; } = default!;

    
    [Required(ErrorMessage = "Date is required.")]
    public DateTime Date { get; set; }

    
    [MaxLength(500, ErrorMessage = "Note must not exceed 500 characters.")]
    public string? Note { get; set; }
}
