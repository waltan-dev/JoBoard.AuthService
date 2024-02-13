﻿using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Auth.Services;

namespace JoBoard.AuthService.Tests.Common.Stubs;

// Thread-safe Singleton
internal static class PasswordHasherStubFactory
{
    public static IPasswordHasher Create()
    {
        return new PasswordHasher();
    }
}