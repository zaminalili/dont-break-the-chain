using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{

    // Navigation properties
    public ICollection<Badge> Badges { get; set; } = default!;
    public UserSettings Settings { get; set; } = default!;
}
