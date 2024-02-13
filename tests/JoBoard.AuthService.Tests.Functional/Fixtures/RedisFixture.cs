using Testcontainers.Redis;

namespace JoBoard.AuthService.Tests.Functional.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class RedisFixture : IAsyncLifetime
{
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithName(Guid.NewGuid().ToString())
        .WithCleanUp(true)
        .Build();
    
    public string ConnectionString { get; private set; }
    
    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
        ConnectionString = _redisContainer.GetConnectionString();
    }

    public async Task DisposeAsync()
    {
        await _redisContainer.DisposeAsync();
    }
}