using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;

public class UserEmailMustBeUniqueRule : IBusinessRule
{
    private readonly IUserEmailUniquenessChecker _userEmailUniquenessChecker;

    private readonly Email _email;

    public UserEmailMustBeUniqueRule(Email email, IUserEmailUniquenessChecker userEmailUniquenessChecker)
    {
        _userEmailUniquenessChecker = userEmailUniquenessChecker;
        _email = email;
    }

    public bool IsBroken()
    {
        return _userEmailUniquenessChecker.IsUnique(_email) == false;
    }

    public string Message => ExceptionMessage;
    public static string ExceptionMessage => "This email is already in use";
}