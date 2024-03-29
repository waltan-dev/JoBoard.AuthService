﻿using MediatR;

namespace JoBoard.AuthService.Application.Commands.ChangeEmail.Request;

// immutable command
public class RequestEmailChangeCommand : IRequest<Unit>
{
    public string NewEmail { get; init; }
}