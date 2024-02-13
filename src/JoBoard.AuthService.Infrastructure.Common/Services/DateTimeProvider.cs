using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Infrastructure.Common.Services;

public class DateTimeProvider : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}