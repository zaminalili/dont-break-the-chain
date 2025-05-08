namespace Application.DTOs;

public class UpdateChainDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public Guid CategoryId { get; set; }
}
