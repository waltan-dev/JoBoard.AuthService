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

    public Task AddAsync(User user, CancellationToken ct = default)
    {
        _dbContext.Users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _dbContext.Entry(user).State = EntityState.Detached;
        _dbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<User?> FindByIdAsync(UserId userId, CancellationToken ct = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(x=>x.ExternalAccounts)
            .FirstOrDefaultAsync(x=>x.Id == userId, ct);
    }

    public async Task<User?> FindByEmailAsync(Email email, CancellationToken ct = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Include(x=>x.ExternalAccounts)
            .FirstOrDefaultAsync(x=>x.Email.Value == email.Value, ct);
    }

    public async Task<User?> FindByExternalAccountAsync(ExternalAccount externalAccount, CancellationToken ct = default)
    {
        var extAccount = await _dbContext.ExternalAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.ExternalUserId == externalAccount.ExternalUserId && x.Provider == externalAccount.Provider, ct);
        if (extAccount == null)
            return null;

        return await FindByIdAsync(externalAccount.Id, ct);
    }
    
    public async Task<bool> CheckEmailUniquenessAsync(Email email, CancellationToken ct = default)
    {
        var emailExists = await _dbContext.Users
            .Where(x => x.Email.Value == email.Value)
            .AnyAsync(ct);
        return emailExists == false;
    }
}