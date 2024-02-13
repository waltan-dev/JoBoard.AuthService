using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Infrastructure.Data.Repositories;

// ReSharper disable once ReplaceWithSingleCallToAny
public class EfExternalAccountUniquenessChecker : EfBaseRepository, IExternalAccountUniquenessChecker
{
    public EfExternalAccountUniquenessChecker(AuthDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
    {
    }

    public bool IsUnique(ExternalAccountValue externalAccountValue)
    {
        var exists = DbContext.ExternalAccounts
            .Where(x => x.Value.ExternalUserId == externalAccountValue.ExternalUserId &&
                        x.Value.Provider == externalAccountValue.Provider)
            .Any();
        return exists == false;
    }
}