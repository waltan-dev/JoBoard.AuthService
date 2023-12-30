﻿using MediatR;

namespace JoBoard.AuthService.Application.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest
{
    public string Email { get; set; }
    public string ConfirmationToken { get; set; }
    public string NewPassword { get; set; }
}