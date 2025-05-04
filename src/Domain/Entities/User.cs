using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<Guid>
{

    // Navigation properties
    public ICollection<UserBadge> Badges { get; set; } = default!;
    public UserSettings Settings { get; set; } = default!;
    public ICollection<Chain> Chains { get; set; } = default!;
}
