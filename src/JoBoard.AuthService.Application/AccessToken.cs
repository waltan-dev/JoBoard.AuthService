using System.IdentityModel.Tokens.Jwt;
using JoBoard.AuthService.Application.Exceptions;

namespace JoBoard.AuthService.Application;

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