using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Infrastructure.Data.EntityConfigs;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Infrastructure.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<ExternalAccount> ExternalAccounts { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityConfig).Assembly);
    }
}