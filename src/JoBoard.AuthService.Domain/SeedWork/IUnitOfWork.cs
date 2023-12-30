namespace JoBoard.AuthService.Domain.SeedWork;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct);
}