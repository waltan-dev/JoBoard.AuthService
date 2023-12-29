using System.Security.Claims;
using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Application;

public class AuthInfo
{
    public string UserId { get; set; }
    public string Role { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    
    
    public static AuthInfo Create(User user, ITokenGenerator tokenGenerator)
    {
        string role = user.AccountType switch
        {
            AccountType.Hirer => nameof(AccountType.Hirer),
            AccountType.Worker => nameof(AccountType.Worker),
            _ => throw new ArgumentOutOfRangeException(nameof(user.AccountType), user.AccountType, null)
        };
        
        var claims = new List<Claim>
        {
            new(nameof(UserId), user.Id.Value.ToString()),
            new(ClaimTypes.Email, user.Email.Value),
            new(ClaimsIdentity.DefaultNameClaimType, user.FullName.ToString()),
            new(ClaimsIdentity.DefaultRoleClaimType, role)
        };

        var accessToken = tokenGenerator.GenerateAccessToken(claims);
        var refreshToken = tokenGenerator.GenerateRefreshToken(claims);
        return new AuthInfo
        {
            UserId = user.Id.Value.ToString(),
            Role = role,
            AccessToken = accessToken.Value,
            RefreshToken = refreshToken.Value,
            AccessTokenExpiration = accessToken.Expiration
        };
    }
}