using System.Data;

namespace JoBoard.AuthService.Domain.Common.SeedWork;

// You can implement this interface with EF or Dapper
public interface IUnitOfWork
{
    Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
}