using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public  class UserStatus : Enumeration
{
    private UserStatus(int id, string name) : base(id, name) { }

    /// <summary>
    /// waiting for email confirmation
    /// </summary>
    public static readonly UserStatus Pending = new(1, nameof(Pending)); 
    
    /// <summary>
    /// email confirmed and user is active
    /// </summary>
    public static readonly UserStatus Active = new(2, nameof(Active));
    
    /// <summary>
    /// user has been deactivated his account
    /// </summary>
    public static readonly UserStatus Deactivated = new(3, nameof(Deactivated));
    
    /// <summary>
    /// user blocked by admin
    /// </summary>
    public static readonly UserStatus Blocked = new(4, nameof(Blocked));
}