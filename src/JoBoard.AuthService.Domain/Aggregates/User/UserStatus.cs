namespace JoBoard.AuthService.Domain.Aggregates.User;

public enum UserStatus
{
    Pending, // waiting for email confirmation
    Active, // email confirmed and user is active
    Blocked, // user blocked by admin
    Deactivated // user has been deactivated his account
}