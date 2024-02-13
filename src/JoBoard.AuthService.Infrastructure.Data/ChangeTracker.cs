using JoBoard.AuthService.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.Infrastructure.Data;

public class ChangeTracker : IChangeTracker
{
    private readonly DbContext _dbContext;

    public ChangeTracker(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IEnumerable<Entity> TrackedEntities => _dbContext.ChangeTracker.Entries<Entity>().Select(x => x.Entity);
    public void Track(Entity entity) { } // dbContext track entities automatically
}