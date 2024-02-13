using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Domain.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Infrastructure.Data;

public class EfDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly AuthDbContext _dbContext;

    public EfDomainEventDispatcher(IMediator mediator, AuthDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }
    
    public async Task DispatchAsync(CancellationToken ct = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        
        var domainEntities = _dbContext.ChangeTracker.Entries<Entity>().Select(x => x.Entity)
            .Where(x => x.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, ct);
    }
}