namespace Domain.Entities;

public class Challenge
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public bool IsPublic { get; set; } = true;


    // foreign keys
    public Guid CategoryId { get; set; }
    

    // Navigation properties
    public ICollection<Chain> Chains { get; set; } = default!;
    public Category Category { get; set; } = default!;
}
