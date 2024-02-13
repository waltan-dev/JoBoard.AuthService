namespace JoBoard.AuthService.Domain.Common.SeedWork;

public interface IChangeTracker
{
    IEnumerable<Entity> TrackedEntities { get; }
    public void Track(Entity entity);
}