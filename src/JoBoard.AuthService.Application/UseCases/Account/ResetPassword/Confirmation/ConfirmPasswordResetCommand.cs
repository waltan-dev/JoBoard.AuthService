﻿using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Account.ResetPassword.Confirmation;

// immutable command
public class ConfirmPasswordResetCommand : IRequest<Unit>
{
    public Guid UserId { get; init; }
    public string ConfirmationToken { get; init; }
    public string NewPassword { get; init; }
}