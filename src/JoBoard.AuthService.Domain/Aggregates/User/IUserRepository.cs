namespace JoBoard.AuthService.Domain.Aggregates.User;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct);
    Task UpdateAsync(User user, CancellationToken ct);
    
    Task<User?> FindByIdAsync(UserId userId, CancellationToken ct);
    Task<User?> FindByEmailAsync(Email email, CancellationToken ct);
    Task<User?> FindByEmailAndPasswordAsync(Email email, string passwordHash, CancellationToken ct);
    Task<User?> FindByExternalAccountAsync(ExternalNetworkAccount externalAccount, CancellationToken ct);
    Task<User?> FindByConfirmationTokenAsync(string confirmationToken, CancellationToken ct);
    
    Task<bool> CheckEmailUniquenessAsync(Email email, CancellationToken ct);
}