﻿using JoBoard.AuthService.Application.UseCases.Account.Register.ByGoogle;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Application.UseCases.Account.Register;

public class RegisterByGoogleAccountCommandHandlerTests
{
    [Fact]
    public async Task HandleValid()
    {
        var command = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForNewUser,
            Role = "Worker"
        };
        
        var handler = CreateHandler();
        var result = await handler.Handle(command, default);
        
        Assert.Equal(result, MediatR.Unit.Value);
    }
    
    [Fact]
    public async Task HandleWithExistingAccount()
    {
        var command = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForExistingUser,
            Role = "Worker"
        };
        
        var handler = CreateHandler();
        var result = await handler.Handle(command, default);
        
        Assert.Equal(result, MediatR.Unit.Value);
    }
    
    [Fact]
    public async Task HandleWithExistingEmail()
    {
        var command = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForNewUserWithExistingEmail,
            Role = "Worker"
        };

        var handler = CreateHandler();
        await Assert.ThrowsAsync<DomainException>(async() =>
        {
            await handler.Handle(command, default);
        });
    }
    
    private static RegisterByGoogleAccountCommandHandler CreateHandler()
    {
        var googleAuthProvider = GoogleFixtures.GetGoogleAuthProviderStub();
        var eventDispatcher = EventFixtures.GetDomainEventDispatcherStub();
        var userRepository = DatabaseFixtures.CreateUserRepositoryStub();
        
        return new RegisterByGoogleAccountCommandHandler(
            eventDispatcher,
            googleAuthProvider,
            userRepository);
    }
}