﻿using MediatR;

namespace JoBoard.AuthService.Application.Commands.DetachExternalAccount;

// immutable command
public class DetachExternalAccountCommand : IRequest<Unit>
{
    public string ExternalUserId { get; init; }
    public string Provider { get; init; }
}