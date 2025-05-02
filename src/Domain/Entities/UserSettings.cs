namespace Domain.Entities;

public class UserSettings
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    //public bool EnableNotifications { get; set; }
    public string? PreferredLanguage { get; set; }
    public bool IsDarkTheme { get; set; }
}
