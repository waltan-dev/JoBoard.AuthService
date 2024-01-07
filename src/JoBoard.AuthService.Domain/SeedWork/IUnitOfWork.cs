namespace JoBoard.AuthService.Domain.SeedWork;

// You can implement this interface with EF or Dapper
public interface IUnitOfWork
{
    Task StartTransactionAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
}