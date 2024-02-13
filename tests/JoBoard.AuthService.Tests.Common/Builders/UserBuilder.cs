using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;
using Moq;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class UserBuilder
{
    private readonly string _password;
    private readonly ConfirmationTokenBuilder _confirmationTokenBuilder;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordStrengthValidator _passwordStrengthValidator;
    private readonly IUserEmailUniquenessChecker _userEmailUniquenessChecker;
    private readonly IExternalAccountUniquenessChecker _externalAccountUniquenessChecker;

    private bool WithGoogleAccountOption { get; set; } = false;
    private bool WithActiveStatusOption { get; set; } = false;
    private bool WithInactiveStatusOption { get; set; } = false;

    private string? GoogleUserId = null;
    private Email Email { get; set; } = new("test_email@gmail.com");

    public UserBuilder(string password,
        ConfirmationTokenBuilder confirmationTokenBuilder,
        IPasswordHasher passwordHasher,
        IPasswordStrengthValidator passwordStrengthValidator)
    {
        _password = password;
        _confirmationTokenBuilder = confirmationTokenBuilder;
        _passwordHasher = passwordHasher;
        _passwordStrengthValidator = passwordStrengthValidator;
        _userEmailUniquenessChecker = GetUserEmailUniquenessCheckerStub();
        _externalAccountUniquenessChecker = GetExternalAccountUniquenessCheckerStub();
    }

    private static IUserEmailUniquenessChecker GetUserEmailUniquenessCheckerStub()
    {
        var stub = new Mock<IUserEmailUniquenessChecker>();
        stub.Setup(x => x.IsUnique(It.IsAny<Email>())).Returns(true);
        return stub.Object;
    }
    
    private static IExternalAccountUniquenessChecker GetExternalAccountUniquenessCheckerStub()
    {
        var stub = new Mock<IExternalAccountUniquenessChecker>();
        stub.Setup(x => x.IsUnique(It.IsAny<ExternalAccountValue>())).Returns(true);
        return stub.Object;
    }
    
    public User Build()
    {
        User user;
        if (WithGoogleAccountOption)
            user = User.RegisterByGoogleAccount(
                userId: UserId.Generate(),
                fullName: new FullName("Ivan", "Ivanov"),
                email: Email,
                role: UserRole.Hirer,
                googleUserId: GoogleUserId!,
                _userEmailUniquenessChecker,
                _externalAccountUniquenessChecker);
        else
        {
            var password = UserPassword.Create(_password, _passwordStrengthValidator, _passwordHasher);
            user = User.RegisterByEmailAndPassword(
                userId: UserId.Generate(),
                fullName: new FullName("Ivan", "Ivanov"),
                email: Email,
                role: UserRole.Worker,
                password: password,
                _userEmailUniquenessChecker);
        }
        
        if (WithActiveStatusOption && user.Status.Equals(UserStatus.Active) == false)
        {
            var token = _confirmationTokenBuilder.BuildActive();
            var dateTimeProvider = new TestDateTimeProvider(DateTime.UtcNow);
            user.RequestEmailConfirmation(token, dateTimeProvider);
            user.ConfirmEmail(token.Value, dateTimeProvider);
        }
        
        if (WithInactiveStatusOption)
            user.Block();
        
        return user;
    }
    
    public UserBuilder WithEmail(string email)
    {
        Email = new Email(email);
        return this;
    }
    
    public UserBuilder WithActiveStatus()
    {
        if(WithInactiveStatusOption)
            throw new ArgumentException();
        
        WithActiveStatusOption = true;
        return this;
    }
    
    public UserBuilder WithInactiveStatus()
    {
        if (WithActiveStatusOption)
            throw new ArgumentException();
        
        WithInactiveStatusOption = true;
        return this;
    }
    
    public UserBuilder WithGoogleAccount(string googleUserId)
    {
        GoogleUserId = googleUserId;
        WithGoogleAccountOption = true;
        return this;
    }
}