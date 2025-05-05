namespace Domain.Entities;

public class UserBadge
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid BadgeId { get; set; }
    public DateTime EarnedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = default!;
    public Badge Badge { get; set; } = default!;
}
