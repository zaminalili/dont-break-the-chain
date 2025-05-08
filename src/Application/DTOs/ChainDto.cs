using Domain.Entities;

namespace Application.DTOs;

public class ChainDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsBroken { get; set; }
    public int CurrentStreak { get; set; }
    public int LongStreak { get; set; }
    public bool IsPublic { get; set; }
    public int RecoveryCount { get; set; }
    public int UsedRecoveryCount { get; set; }

    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }

    public User User { get; set; } = default!;
    public Category Category { get; set; } = default!;
}
