using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Common.Builders;

public class ConfirmationTokenBuilder
{
    private readonly ISecureTokenizer _secureTokenizer = new SecureTokenizerStub();
    
    public ConfirmationToken BuildActive()
    {
        return ConfirmationToken.Create(_secureTokenizer, TimeSpan.FromHours(24));
    }
    
    public ConfirmationToken BuildExpired()
    {
        return ConfirmationToken.Create(_secureTokenizer, TimeSpan.FromHours(-1));
    }
}