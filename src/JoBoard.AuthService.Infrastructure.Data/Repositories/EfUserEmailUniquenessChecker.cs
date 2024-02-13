using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Infrastructure.Data.Repositories;

// TODO replace to dapper

// ReSharper disable once ReplaceWithSingleCallToAny
public class EfUserEmailUniquenessChecker : EfBaseRepository, IUserEmailUniquenessChecker
{
    public EfUserEmailUniquenessChecker(AuthDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
    {
    }
    
    public bool IsUnique(Email email)
    {
        var emailExists = DbContext.Users
            .Where(x => x.Email.Value == email.Value)
            .Any();
        return emailExists == false;
    }
}