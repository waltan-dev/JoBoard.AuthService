using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Services;
using Microsoft.Extensions.Options;

namespace JoBoard.AuthService.Infrastructure.Authentication;

public class Tokenizer : ITokenizer
{
    private readonly ConfirmationTokenConfig _config;

    public Tokenizer(IOptions<ConfirmationTokenConfig> options)
    {
        _config = options.Value;
    }
    
    public ConfirmationToken Generate(int? expiresInHours = null)
    {
        int expires = expiresInHours ?? _config.ExpiresInHours;

        return new ConfirmationToken(Guid.NewGuid().ToString(), DateTime.UtcNow.AddHours(expires));
    }
}