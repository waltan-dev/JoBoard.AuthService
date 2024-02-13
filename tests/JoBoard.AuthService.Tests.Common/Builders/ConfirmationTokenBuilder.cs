using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class ConfirmationTokenBuilder
{
    private readonly ISecureTokenizer _secureTokenizer;

    public ConfirmationTokenBuilder(ISecureTokenizer secureTokenizer)
    {
        _secureTokenizer = secureTokenizer;
    }
    
    public ConfirmationToken BuildActive()
    {
        return ConfirmationToken.Create(_secureTokenizer, TimeSpan.FromHours(24));
    }
}