using JoBoard.AuthService.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace JoBoard.AuthService.Infrastructure.Data;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private IDbContextTransaction? _currentTransaction;
    
    public EfUnitOfWork(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if(_currentTransaction != null)
            return;
        
        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction == null)
            throw new NullReferenceException();
        
        try
        {
            await _dbContext.SaveChangesAsync(ct);
            await _dbContext.Database.CommitTransactionAsync(ct);
        }
        catch
        {
            await RollbackTransactionAsync(ct);
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    private async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            if(_currentTransaction != null)
                await _currentTransaction.RollbackAsync(ct);
        }
        finally
        {
            if(_currentTransaction != null)
                await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}