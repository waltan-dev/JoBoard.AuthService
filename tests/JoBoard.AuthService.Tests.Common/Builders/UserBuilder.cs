using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Tests.Common.DataFixtures;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class UserBuilder
{
    private readonly ConfirmationTokenBuilder _confirmationTokenBuilder;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordStrengthValidator _passwordStrengthValidator;
    private readonly IUserEmailUniquenessChecker _userEmailUniquenessChecker;
    private readonly IExternalAccountUniquenessChecker _externalAccountUniquenessChecker;
    
    private bool WithGoogleAccountOption { get; set; } = false;
    private bool WithActiveStatusOption { get; set; } = false;
    private bool WithInactiveStatusOption { get; set; } = false;
    
    private Email Email { get; set; } = new("test_email@gmail.com");

    public UserBuilder(
        ConfirmationTokenBuilder confirmationTokenBuilder,
        IPasswordHasher passwordHasher,
        IPasswordStrengthValidator passwordStrengthValidator,
        IUserEmailUniquenessChecker userEmailUniquenessChecker, 
        IExternalAccountUniquenessChecker externalAccountUniquenessChecker)
    {
        _confirmationTokenBuilder = confirmationTokenBuilder;
        _passwordHasher = passwordHasher;
        _passwordStrengthValidator = passwordStrengthValidator;
        _userEmailUniquenessChecker = userEmailUniquenessChecker;
        _externalAccountUniquenessChecker = externalAccountUniquenessChecker;
    }
    
    public User Build()
    {
        User user;
        if (WithGoogleAccountOption)
            user = User.RegisterByGoogleAccount(userId: UserId.Generate(),
                fullName: new FullName("Ivan", "Ivanov"),
                email: Email,
                role: UserRole.Hirer,
                googleUserId: GoogleFixtures.UserProfileForNewUser.Id,
                _userEmailUniquenessChecker,
                _externalAccountUniquenessChecker);
        else
        {
            var password = UserPassword.Create(PasswordFixtures.DefaultPassword, _passwordStrengthValidator, _passwordHasher);
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
            user.RequestEmailConfirmation(token);
            user.ConfirmEmail(token.Value);
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
    
    public UserBuilder WithGoogleAccount()
    {
        WithGoogleAccountOption = true;
        return this;
    }
}