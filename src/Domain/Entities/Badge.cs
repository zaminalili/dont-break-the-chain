namespace Domain.Entities;

public class Badge
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string IconUrl { get; set; } = default!;
    public string? Description { get; set; }


    // Navigation properties
    public ICollection<UserBadge> Users { get; set; } = default!;
}
