namespace JoBoard.AuthService.Infrastructure.Jwt;

public class JwtStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return (app) =>
        {
            // TODO add jwt
            app.UseAuthorization();
            next(app);
        };
    }
}