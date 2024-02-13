using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Infrastructure.Data.EntityConfigs;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Infrastructure.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public virtual DbSet<User> Users { get; init; } = null!;
    public virtual DbSet<ExternalAccount> ExternalAccounts { get; init; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityConfig).Assembly);
    }
}