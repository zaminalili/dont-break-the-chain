namespace Domain.Entities;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public string? Icon { get; set; }
    public bool IsDeactive { get; set; }


    // Navigation properties
    public ICollection<Challenge> Challenges { get; set; } = default!;
    public ICollection<Chain> Chains { get; set; } = default!;
}
