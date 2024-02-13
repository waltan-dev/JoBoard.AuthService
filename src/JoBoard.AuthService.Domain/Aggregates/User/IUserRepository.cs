﻿using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public interface IUserRepository : IRepository<User>
{
    Task AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
    
    Task<User?> FindByIdAsync(UserId userId, CancellationToken ct = default);
    Task<User?> FindByEmailAsync(Email email, CancellationToken ct = default);
    Task<User?> FindByExternalAccountValueAsync(ValueObjects.ExternalAccountValue externalAccount, CancellationToken ct = default);
    
    Task<bool> CheckEmailUniquenessAsync(Email email, CancellationToken ct = default);
}