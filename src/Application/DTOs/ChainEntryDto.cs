namespace Application.DTOs;

public class ChainEntryDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsCompleted { get; set; }
    public Guid ChainId { get; set; }
}
