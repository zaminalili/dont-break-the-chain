namespace Domain.Entities;

public class Chain
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    //public bool IsBroken { get; set; }
    public int CurrentStreak { get; set; }
    public bool IsPublic { get; set; } = true;
    public int RecoveryCount { get; set; } = 3;
    public int UsedRecoveryCount { get; set; }

    // foreign keys
    public Guid UserId { get; set; }
    public Guid? ChallengeId { get; set; }
    public Guid CategoryId { get; set; }

    // navigation properties
    public User User { get; set; } = null!;
    public Category Category { get; set; } = default!;
    public Challenge? Challenge { get; set; }
    public ICollection<ChainEntry> Entries { get; set; } = default!;
}