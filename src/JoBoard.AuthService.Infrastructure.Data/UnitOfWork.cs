using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AuthDbContext _dbContext;

    public UnitOfWork(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task StartTransactionAsync(CancellationToken ct = default)
    {
        await _dbContext.Database.BeginTransactionAsync(ct);
    }
    
    public async Task CommitAsync(CancellationToken ct = default)
    {
        await _dbContext.SaveChangesAsync(ct);
        await _dbContext.Database.CommitTransactionAsync(ct);
    }
}