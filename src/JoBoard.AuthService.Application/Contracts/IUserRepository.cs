using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Application.Contracts;

public interface IUserRepository
{
    void Add(User user, CancellationToken ct);
    Task<bool> CheckEmailUniquenessAsync(Email email, CancellationToken ct);
    Task<User?> GetByEmailAndPasswordAsync(Email email, string passwordHash, CancellationToken ct);
}