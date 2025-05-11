using Domain.Entities;

namespace Application.DTOs;

public class CreateChainEntryDto
{
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public string? ImageUrl { get; set; }

    public Guid ChainId { get; set; }

}
