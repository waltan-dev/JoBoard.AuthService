﻿using JoBoard.AuthService.Application.Models;
using MediatR;

namespace JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;

public class RegisterByEmailCommand : IRequest<UserResult>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}