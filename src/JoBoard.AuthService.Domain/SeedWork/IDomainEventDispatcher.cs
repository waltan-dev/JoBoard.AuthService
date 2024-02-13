namespace JoBoard.AuthService.Domain.SeedWork;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(CancellationToken cancellationToken = default);
}