namespace JoBoard.AuthService.Domain.Common.SeedWork;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
}