using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

internal class DBChDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public DBChDbContext(DbContextOptions<DBChDbContext> options)
        : base(options)
    {
    }

    public DbSet<Badge> Badges { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Chain> Chains { get; set; } = default!;
    public DbSet<ChainEntry> ChainEntries { get; set; } = default!;
    public DbSet<Challenge> Challenges { get; set; } = default!;
    public DbSet<UserBadge> UserBadges { get; set; } = default!;
    public DbSet<UserSettings> UserSettings { get; set; } = default!;

}

