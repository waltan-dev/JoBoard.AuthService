using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;
using Microsoft.EntityFrameworkCore;
using ExternalAccountValue = JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects.ExternalAccountValue;

namespace JoBoard.AuthService.Infrastructure.Data.Repositories;

public class EfUserRepository : EfBaseRepository, IUserRepository
{
    public EfUserRepository(AuthDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
    {
    }

    public Task AddAsync(User user, CancellationToken ct = default)
    {
        DbContext.Users.Add(user);
        return SaveChangesWithoutCommitAsync(ct);
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        // ef core automatically generates sql based on tracked aggregate changes
        return SaveChangesWithoutCommitAsync(ct);
    }

    public async Task<User?> FindByIdAsync(UserId userId, CancellationToken ct = default)
    {
        // track all aggregate changes automatically to update it correctly
        // don't use .AsNoTracking()
        return await DbContext.Users
            .Include(x=>x.ExternalAccounts)
            .FirstOrDefaultAsync(x=>x.Id == userId, ct);
    }

    public async Task<User?> FindByEmailAsync(Email email, CancellationToken ct = default)
    {
        // track all aggregate changes automatically to update it correctly
        // don't use .AsNoTracking()
        return await DbContext.Users
            .Include(x=>x.ExternalAccounts)
            .FirstOrDefaultAsync(x=>x.Email.Value == email.Value, ct);
    }

    public async Task<User?> FindByExternalAccountValueAsync(ExternalAccountValue externalAccount, CancellationToken ct = default)
    {
        var extAccountDb = await DbContext.ExternalAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.Value.ExternalUserId == externalAccount.ExternalUserId && x.Value.Provider == externalAccount.Provider, ct);
        if (extAccountDb == null)
            return null;
        
        // track all aggregate changes automatically to update it correctly
        // don't use .AsNoTracking()
        return await FindByIdAsync(extAccountDb.Id, ct);
    }
    
    public async Task<bool> CheckEmailUniquenessAsync(Email email, CancellationToken ct = default)
    {
        var emailExists = await DbContext.Users
            .Where(x => x.Email.Value == email.Value)
            .AnyAsync(ct);
        return emailExists == false;
    }
}