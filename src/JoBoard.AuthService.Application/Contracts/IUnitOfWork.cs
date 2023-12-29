namespace JoBoard.AuthService.Application.Contracts;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct);
}