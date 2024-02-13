using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;

public interface IUserRepository : IRepository<User>
{
    Task AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
    
    Task<User?> FindByIdAsync(UserId userId, CancellationToken ct = default);
    Task<User?> FindByEmailAsync(Email email, CancellationToken ct = default);
    Task<User?> FindByExternalAccountValueAsync(ValueObjects.ExternalAccountValue externalAccount, CancellationToken ct = default);
}