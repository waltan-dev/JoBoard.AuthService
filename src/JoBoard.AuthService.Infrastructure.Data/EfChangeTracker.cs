using JoBoard.AuthService.Domain.Common.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Infrastructure.Data;

public class EfChangeTracker : IChangeTracker
{
    private readonly DbContext _dbContext;

    public EfChangeTracker(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Entity> TrackedEntities => _dbContext.ChangeTracker.Entries<Entity>().Select(x => x.Entity);
    public void Track(Entity entity) { } // dbContext track entities automatically
}