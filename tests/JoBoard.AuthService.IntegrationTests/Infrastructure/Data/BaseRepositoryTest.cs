using JoBoard.AuthService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JoBoard.AuthService.IntegrationTests.Infrastructure.Data;

public abstract class BaseRepositoryTest
{
    protected readonly AuthDbContext _dbContext;
    protected readonly UnitOfWork _unitOfWork;
    
    public BaseRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseNpgsql("Host=localhost;Port=54322;Database=auth-service-test-db;Username=test-user;Password=password")
            .Options;
        
        _dbContext = new AuthDbContext(options);
        _unitOfWork = new UnitOfWork(_dbContext);
        ClearDatabase(_dbContext);
    }
    
    private static void ClearDatabase(DbContext dbContext)
    {
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"public\".\"Users\" CASCADE ;");
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"public\".\"ExternalAccounts\" CASCADE;");
    }
}