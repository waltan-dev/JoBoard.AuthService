namespace JoBoard.AuthService.Domain.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime TrimMilliseconds(this DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
    }
}