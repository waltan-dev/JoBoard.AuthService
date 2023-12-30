using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Application.Contracts;

public interface IUserRepository
{
    void Add(User user, CancellationToken ct);
    void Update(User user, CancellationToken ct);
    
    Task<User?> FindByIdAsync(UserId userId, CancellationToken ct);
    Task<User?> FindByEmailAndPasswordAsync(Email email, string passwordHash, CancellationToken ct);
    Task<User?> FindByExternalAccountAsync(ExternalNetworkAccount externalAccount, CancellationToken ct);
    Task<User?> FindByConfirmationTokenAsync(string confirmationToken, CancellationToken ct);
    
    Task<bool> CheckEmailUniquenessAsync(Email email, CancellationToken ct);
}