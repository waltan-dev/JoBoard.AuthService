using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace JoBoard.AuthService.Infrastructure.Jwt;

public class AccessToken
{
    public string Value { get; }
    public DateTime Expiration { get; }
    public string UserId { get; }

    public AccessToken(string value, DateTime expiration, string userId)
    {
        Value = value;
        Expiration = expiration;
        UserId = userId;
    }

    public AccessToken(string accessToken)
    {
        var jwtToken = new JwtSecurityTokenHandler().ReadToken(accessToken) as JwtSecurityToken;
        if (jwtToken == null)
            throw new ValidationException("Invalid access token");
        
        Value = accessToken;
        Expiration = jwtToken.ValidTo;
        UserId = jwtToken.Claims.First(x => x.Type == nameof(UserId)).Value;
    }
}