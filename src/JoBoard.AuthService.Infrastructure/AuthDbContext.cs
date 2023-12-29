using JoBoard.AuthService.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Infrastructure;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<User> Users { get; set; } = null!;
}