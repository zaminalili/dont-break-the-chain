namespace Application.DTOs;

public class CreateChainDto
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public DateTime StartDate { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
}
