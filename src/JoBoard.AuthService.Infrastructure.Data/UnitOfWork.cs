using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AuthDbContext _dbContext;

    public UnitOfWork(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}