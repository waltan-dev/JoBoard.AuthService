using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Infrastructure.Data.Repositories;

public abstract class EfBaseRepository
{
    protected readonly AuthDbContext DbContext;
    public IUnitOfWork UnitOfWork { get; }

    protected EfBaseRepository(AuthDbContext dbContext, IUnitOfWork unitOfWork)
    {
        if(dbContext.ChangeTracker != null) // only for tests
            dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        if(dbContext.Database != null) // only for tests
            dbContext.Database.AutoTransactionsEnabled = false;
        DbContext = dbContext;
        UnitOfWork = unitOfWork;
    }

    protected Task SaveChangesWithoutCommitAsync(CancellationToken ct = default)
    {
        // this isn't transaction commit
        // it's just a part of transaction
        return DbContext.SaveChangesAsync(ct); 
    }
}