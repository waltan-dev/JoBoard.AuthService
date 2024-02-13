namespace JoBoard.AuthService.Domain.SeedWork;

public interface IDateTime
{
    DateTime UtcNow { get; }
}

public class DateTimeProvider : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}