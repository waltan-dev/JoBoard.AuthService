namespace JoBoard.AuthService.Infrastructure.Data.Repositories;

public abstract class EfBaseRepository
{
    protected readonly AuthDbContext DbContext;

    protected EfBaseRepository(AuthDbContext dbContext)
    {
        dbContext.Database.AutoTransactionsEnabled = false;
        DbContext = dbContext;
    }

    protected Task ExecuteWithoutCommitAsync(CancellationToken cancellationToken = default)
    {
        // this isn't transaction commit
        return DbContext.SaveChangesAsync(cancellationToken); 
    }
}