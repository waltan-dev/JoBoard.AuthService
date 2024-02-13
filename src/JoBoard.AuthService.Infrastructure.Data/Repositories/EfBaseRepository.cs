namespace JoBoard.AuthService.Infrastructure.Data.Repositories;

public abstract class EfBaseRepository
{
    protected readonly AuthDbContext DbContext;

    protected EfBaseRepository(AuthDbContext dbContext)
    {
        dbContext.ChangeTracker.AutoDetectChangesEnabled = true;
        dbContext.Database.AutoTransactionsEnabled = false;
        DbContext = dbContext;
    }

    protected Task SaveChangesWithoutCommitAsync(CancellationToken cancellationToken = default)
    {
        // this isn't transaction commit
        // it's just a part of transaction
        return DbContext.SaveChangesAsync(cancellationToken); 
    }
}