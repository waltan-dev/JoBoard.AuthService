﻿using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
}