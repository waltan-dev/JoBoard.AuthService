﻿using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Confirmation;

public class ConfirmPasswordResetCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string ConfirmationToken { get; set; }
    public string NewPassword { get; set; }
}