using JoBoard.AuthService.Domain.SeedWork;
using MediatR;

namespace JoBoard.AuthService.Application.Contracts;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(CancellationToken ct = default);
}