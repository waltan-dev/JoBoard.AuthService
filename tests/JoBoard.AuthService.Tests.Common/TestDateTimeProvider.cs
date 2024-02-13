using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Tests.Common;

public class TestDateTimeProvider : IDateTime
{
    public DateTime UtcNow { get; }

    public TestDateTimeProvider(DateTime dateTime)
    {
        UtcNow = dateTime;
    }
}