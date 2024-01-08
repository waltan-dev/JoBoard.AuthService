﻿using MediatR;

namespace JoBoard.AuthService.Application.Auth.Manage.ChangeRole;

public class ChangeRoleCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string NewRole { get; set; }
}