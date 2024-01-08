using JoBoard.AuthService.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _dbContext;

    public UserRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(User user, CancellationToken ct)
    {
        _dbContext.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user, CancellationToken ct)
    {
        _dbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<User?> FindByIdAsync(UserId userId, CancellationToken ct)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Id == userId, ct);
    }

    public async Task<User?> FindByEmailAsync(Email email, CancellationToken ct)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Email == email, ct);
    }

    public async Task<User?> FindByEmailAndPasswordAsync(Email email, string passwordHash, CancellationToken ct)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x=>x.Email == email && x.PasswordHash == passwordHash, ct);
    }

    public async Task<User?> FindByExternalAccountAsync(ExternalAccount externalAccount, CancellationToken ct)
    {
        var extAccount = await _dbContext.ExternalAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.ExternalUserId == externalAccount.ExternalUserId && x.Provider == externalAccount.Provider, ct);
        if (extAccount == null)
            return null;

        return await FindByIdAsync(externalAccount.Id, ct);
    }
    
    public async Task<bool> CheckEmailUniquenessAsync(Email email, CancellationToken ct)
    {
        var emailExists = await _dbContext.Users
            .Where(x => x.Email.Value == email.Value)
            .AnyAsync(ct);
        return emailExists == false;
    }
}