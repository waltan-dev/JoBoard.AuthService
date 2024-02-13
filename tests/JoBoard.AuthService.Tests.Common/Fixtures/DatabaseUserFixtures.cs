﻿using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Tests.Common.Fixtures;

// Thread-safe Singleton for parallel tests
public static class DatabaseUserFixtures
{
    private static object _lock = new();
    private static User? _existingActiveUser;
    public static User ExistingActiveUser 
    {
        get
        {
            lock (_lock)
            {
                if (_existingActiveUser != null)
                    return _existingActiveUser;
                
                var user = User.RegisterByEmailAndPassword(userId: UserId.Generate(),
                    fullName: new FullName("Test", "Hirer"),
                    email: new Email("ExistingActiveUser@gmail.com"),
                    role: UserRole.Hirer,
                    password: PasswordFixtures.CreateDefault(),
                    registerConfirmToken: ConfirmationTokenFixtures.CreateNew());
                
                user.ConfirmEmail(user.RegisterConfirmToken!.Value);
                _existingActiveUser = user;
            
                return _existingActiveUser; 
            }
        }
    }
    
    public static readonly User ExistingUserRegisteredByEmail = User.RegisterByEmailAndPassword(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Hirer"),
        email: new Email("ExistingUserRegisteredByEmail@gmail.com"),
        role: UserRole.Hirer, 
        password: PasswordFixtures.CreateDefault(),
        registerConfirmToken: ConfirmationTokenFixtures.CreateNew()); 
    
    public static readonly User ExistingUserRegisteredByGoogleAccount = User.RegisterByGoogleAccount(
        userId: UserId.Generate(),
        fullName: new FullName(GoogleFixtures.UserProfileForExistingUser.FirstName, GoogleFixtures.UserProfileForExistingUser.LastName),
        email: new Email(GoogleFixtures.UserProfileForExistingUser.Email),
        role: UserRole.Worker, 
        googleUserId: GoogleFixtures.UserProfileForExistingUser.Id); 
    
    public static readonly User ExistingUserWithExpiredToken = User.RegisterByEmailAndPassword(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Hirer"),
        email: new Email("ExistingUserWithExpiredToken@gmail.com"),
        role: UserRole.Hirer, 
        password: PasswordFixtures.CreateDefault(),
        registerConfirmToken: ConfirmationTokenFixtures.CreateExpired());
}