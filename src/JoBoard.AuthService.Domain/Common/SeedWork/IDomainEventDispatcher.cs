namespace JoBoard.AuthService.Domain.Common.SeedWork;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(CancellationToken ct = default);
}