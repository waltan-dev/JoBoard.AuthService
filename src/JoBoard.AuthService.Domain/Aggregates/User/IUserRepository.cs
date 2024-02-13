using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public interface IUserRepository : IRepository<User>
{
    Task AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
    
    Task<User?> FindByIdAsync(UserId userId, CancellationToken ct = default);
    Task<User?> FindByEmailAsync(Email email, CancellationToken ct = default);
    Task<User?> FindByExternalAccountAsync(ExternalAccount externalAccount, CancellationToken ct = default);
    
    Task<bool> CheckEmailUniquenessAsync(Email email, CancellationToken ct = default);
}