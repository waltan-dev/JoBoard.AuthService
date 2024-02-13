using System.Data;
using JoBoard.AuthService.Domain.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace JoBoard.AuthService.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly IMediator? _mediator;
    private readonly IChangeTracker? _changeTracker;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(AuthDbContext dbContext, IMediator? mediator, IChangeTracker? changeTracker)
    {
        dbContext.Database.AutoTransactionsEnabled = false;
        _dbContext = dbContext;
        _mediator = mediator;
        _changeTracker = changeTracker;
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
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        if(_mediator != null && _changeTracker != null)
            await _mediator.DispatchDomainEventsAsync(_changeTracker);
        
        await _dbContext.Database.CommitTransactionAsync(cancellationToken);
    }
}