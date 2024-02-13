using System.Data;
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

    public async Task BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, 
        CancellationToken cancellationToken = default)
    {
        if(_currentTransaction != null)
            return;
        
        _currentTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            throw new NullReferenceException();
        
        await _dbContext.Database.CommitTransactionAsync(cancellationToken);
    }
}