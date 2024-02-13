using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate;

public class UserRole : Enumeration
{
    private UserRole(int id, string name) : base(id, name) { }

    public static readonly UserRole Hirer = new(1, nameof(Hirer));
    public static readonly UserRole Worker = new(2, nameof(Worker));
    public static readonly UserRole Admin = new(3, nameof(Admin));
}