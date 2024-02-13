using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Events;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Rules;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Extensions;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate;

public class User : Entity, IAggregateRoot
{
    public new UserId Id { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public FullName FullName { get; private set; }
    public Email Email { get; private set; }
    public Email? NewEmail { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public UserPassword? Password { get; private set; }
    
    private List<ExternalAccount> _externalAccounts;
    public IReadOnlyCollection<ExternalAccount> ExternalAccounts => _externalAccounts.AsReadOnly();
    
    public ConfirmationToken? EmailConfirmToken { get; private set; }
    public ConfirmationToken? ResetPasswordConfirmToken { get; private set; }
    public ConfirmationToken? ChangeEmailConfirmToken { get; private set; }
    public ConfirmationToken? AccountDeactivationConfirmToken { get; private set; }
    
    private User() {} // only for ef core 
    
    // universal constructor for different scenarios, e.g. register by google account or register by email and password
    private User(UserId userId, FullName fullName, Email email, bool emailConfirmed, UserRole role, UserStatus status,
        UserPassword? password, ExternalAccountValue? externalAccountValue)
    {
        Id = userId;
        RegisteredAt = DateTime.UtcNow.TrimMilliseconds();
        FullName = fullName;
        Email = email;
        EmailConfirmed = emailConfirmed;
        Role = role;
        Status = status;
        Password = password;
        _externalAccounts = externalAccountValue == null 
            ? new List<ExternalAccount>() 
            : new List<ExternalAccount> { new(Id, externalAccountValue) };
        
        AddDomainEvent(new UserRegisteredDomainEvent(this));
    }

    public static User RegisterByEmailAndPassword(UserId userId, FullName fullName, Email email, UserRole role, 
        UserPassword password, 
        IUserEmailUniquenessChecker userEmailUniquenessChecker)
    {
        CheckRule(new UserEmailMustBeUniqueRule(email, userEmailUniquenessChecker));
        CheckRule(new UserRoleMustBeWorkerOrHirerRule(role));
        
        return new User(
            userId: userId,
            fullName: fullName,
            email: email,
            emailConfirmed: false,
            role: role,
            status: UserStatus.Pending, 
            password: password, 
            null);
    }
    
    public static User RegisterByGoogleAccount(UserId userId, FullName fullName, Email email, UserRole role, 
        string googleUserId, 
        IUserEmailUniquenessChecker userEmailUniquenessChecker, 
        IExternalAccountUniquenessChecker externalAccountUniquenessChecker)
    {
        var externalAccountValue = new ExternalAccountValue(googleUserId, ExternalAccountProvider.Google);
        
        CheckRule(new UserRoleMustBeWorkerOrHirerRule(role));
        CheckRule(new UserEmailMustBeUniqueRule(email, userEmailUniquenessChecker));
        CheckRule(new ExternalAccountMustBeUniqueRule(externalAccountValue, null, externalAccountUniquenessChecker));
        
        return new User(
            userId: userId,
            fullName: fullName,
            email: email,
            emailConfirmed: true,
            role: role,
            status: UserStatus.Active, 
            password: null, 
            externalAccountValue: externalAccountValue);
    }

    public void RequestEmailConfirmation(ConfirmationToken confirmationToken, IDateTime dateTime)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new ConfirmTokenCanNotBeRequestedYetRule(EmailConfirmToken, dateTime));
        
        EmailConfirmToken = confirmationToken;
        
        AddDomainEvent(new UserRequestedEmailConfirmationDomainEvent(this));
    }
    
    public void ConfirmEmail(string requestToken, IDateTime dateTime)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new ConfirmTokenMustBeRequestedFirstRule(EmailConfirmToken));
        EmailConfirmToken!.Verify(requestToken, dateTime);
        
        EmailConfirmed = true;
        EmailConfirmToken = null;
        if(Status.Equals(UserStatus.Pending)) Status = UserStatus.Active;
        
        AddDomainEvent(new UserConfirmedEmailDomainEvent(this));
    }
    
    public void UpdateFullName(FullName fullName)
    {
        CheckBlockedOrDeactivatedRule();
        
        FullName = fullName;
        
        AddDomainEvent(new UserUpdatedNameDomainEvent(this));
    }
    
    public void RequestPasswordReset(ConfirmationToken confirmationToken, IDateTime dateTime)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new UserEmailMustBeConfirmedRule(Status, EmailConfirmed));
        CheckRule(new ConfirmTokenCanNotBeRequestedYetRule(ResetPasswordConfirmToken, dateTime));
        
        ResetPasswordConfirmToken = confirmationToken;
        
        AddDomainEvent(new UserRequestedPasswordResetDomainEvent(this));
    }

    public void ConfirmPasswordReset(string requestToken, UserPassword newPassword, IDateTime dateTime)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new UserEmailMustBeConfirmedRule(Status, EmailConfirmed));
        CheckRule(new ConfirmTokenMustBeRequestedFirstRule(ResetPasswordConfirmToken));
        ResetPasswordConfirmToken!.Verify(requestToken, dateTime);
        
        Password = newPassword;
        ResetPasswordConfirmToken = null;
        
        AddDomainEvent(new UserChangedPasswordDomainEvent(this));
    }

    public void ChangePassword(string currentPassword, UserPassword newPassword, IPasswordHasher passwordHasher)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new UserMustHavePasswordRule(Password));
        Password!.Verify(currentPassword, passwordHasher);
        
        Password = newPassword;
        
        AddDomainEvent(new UserChangedPasswordDomainEvent(this));
    }

    public void RequestEmailChange(Email newEmail, ConfirmationToken confirmationToken, 
        IUserEmailUniquenessChecker userEmailUniquenessChecker, IDateTime dateTime)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new UserEmailMustBeConfirmedRule(Status, EmailConfirmed));
        CheckRule(new UserEmailMustBeUniqueRule(newEmail, userEmailUniquenessChecker));
        CheckRule(new ConfirmTokenCanNotBeRequestedYetRule(ChangeEmailConfirmToken, dateTime));
        
        NewEmail = newEmail;
        ChangeEmailConfirmToken = confirmationToken;
        
        AddDomainEvent(new UserRequestedEmailChangeDomainEvent(this));
    }

    public void ConfirmEmailChange(string requestToken, IDateTime dateTime)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new UserEmailMustBeConfirmedRule(Status, EmailConfirmed));
        CheckRule(new ConfirmTokenMustBeRequestedFirstRule(ChangeEmailConfirmToken));
        ChangeEmailConfirmToken!.Verify(requestToken, dateTime);
        
        Email = NewEmail!;
        EmailConfirmed = true;
        NewEmail = null;
        ChangeEmailConfirmToken = null;
        
        AddDomainEvent(new UserChangedEmailDomainEvent(this));
    }

    public void ChangeRole(UserRole newRole)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new UserEmailMustBeConfirmedRule(Status, EmailConfirmed));
        CheckRule(new UserRoleMustBeWorkerOrHirerRule(newRole));
        
        Role = newRole;
        
        AddDomainEvent(new UserChangedRoleDomainEvent(this));
    }

    public void RequestAccountDeactivation(ConfirmationToken confirmationToken, IDateTime dateTime)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new UserEmailMustBeConfirmedRule(Status, EmailConfirmed));
        CheckRule(new ConfirmTokenCanNotBeRequestedYetRule(AccountDeactivationConfirmToken, dateTime));
        
        AccountDeactivationConfirmToken = confirmationToken;
        
        AddDomainEvent(new UserRequestedAccountDeactivationDomainEvent(this));
    }

    public void ConfirmAccountDeactivation(string requestToken, IDateTime dateTime)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new UserEmailMustBeConfirmedRule(Status, EmailConfirmed));
        CheckRule(new ConfirmTokenMustBeRequestedFirstRule(AccountDeactivationConfirmToken));
        AccountDeactivationConfirmToken!.Verify(requestToken, dateTime);
        
        Status = UserStatus.Deactivated;
        AccountDeactivationConfirmToken = null;
        
        AddDomainEvent(new UserDeactivatedDomainEvent(this));
    }
    
    public void Block()
    {
        Status = UserStatus.Blocked;
        
        AddDomainEvent(new UserBlockedDomainEvent(this));
    }

    public void CanLogin()
    {
        CheckBlockedOrDeactivatedRule();
    }
    
    public void CanLoginWithPassword(string currentPassword, IPasswordHasher passwordHasher)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new UserMustHavePasswordRule(Password));
        
        Password!.Verify(currentPassword, passwordHasher);
    }

    public void CanLoginWithExternalAccount(ExternalAccountValue externalAccountValue)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new ExternalAccountMustBelongToUserRule(_externalAccounts, externalAccountValue));
    }
    
    public void AttachExternalAccount(ExternalAccountValue value, IExternalAccountUniquenessChecker externalAccountUniquenessChecker)
    {
        CheckBlockedOrDeactivatedRule();
        CheckRule(new ExternalAccountMustBeUniqueRule(value, _externalAccounts, externalAccountUniquenessChecker));
        
        _externalAccounts.Add(new ExternalAccount(Id, value));
        
        AddDomainEvent(new UserAttachedExternalAccountDomainEvent(this));
    }
    
    public void DetachExternalAccount(ExternalAccountValue value)
    {
        CheckBlockedOrDeactivatedRule();

        var extAcc = ExternalAccounts.FirstOrDefault(x => x.Value.Equals(value));
        if (extAcc == null) return;
        
        _externalAccounts.Remove(extAcc);
        AddDomainEvent(new UserDetachedExternalAccountDomainEvent(this));
    }
    
    private void CheckBlockedOrDeactivatedRule()
    {
        CheckRule(new UserCanNotBeDeactivatedRule(Status));
        CheckRule(new UserCanNotBeBlockedRule(Status));
    }
}