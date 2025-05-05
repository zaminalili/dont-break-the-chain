namespace Domain.Entities;

public class ChainEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Date { get; set; }
    public string? Note { get; set; }
    public bool IsCompleted { get; set; }

    // foreign keys
    public Guid ChainId { get; set; }

    // navigation properties
    public Chain Chain { get; set; } = default!;
    
}
