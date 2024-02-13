using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Tests.Unit.Domain.SeedWork;

public class EntityTests
{
    [Fact]
    public void EqualsSameTypesWithEqualIds()
    {
        Entity testEntity1 = new TestEntityA(1);
        Entity testEntity2 = new TestEntityA(1);

        var result = testEntity1.Equals(testEntity2);
        
        Assert.True(result);
    }
    
    [Fact]
    public void EqualsSameTypesWithLeftDefaultId()
    {
        Entity testEntity1 = new TestEntityA();
        Entity testEntity2 = new TestEntityA(1);

        var result = testEntity1.Equals(testEntity2);
        
        Assert.False(result);
    }
    
    [Fact]
    public void EqualsSameTypesWithRightDefaultId()
    {
        Entity testEntity1 = new TestEntityA(1);
        Entity testEntity2 = new TestEntityA();

        var result = testEntity1.Equals(testEntity2);
        
        Assert.False(result);
    }
    
    [Fact]
    public void EqualsDifferentTypes()
    {
        Entity testEntityA = new TestEntityA();
        Entity testEntityB = new TestEntityB();

        var result = testEntityA.Equals(testEntityB);
        
        Assert.False(result);
    }
    
    [Fact]
    public void CompareHashCodesWithEqualIds()
    {
        Entity testEntity1 = new TestEntityA(1);
        Entity testEntity2 = new TestEntityA(1);

        var result = testEntity1.GetHashCode() == testEntity2.GetHashCode();
        
        Assert.True(result);
    }
    
    [Fact]
    public void CompareHashCodesWithNonEqualIds()
    {
        Entity testEntity1 = new TestEntityA(1);
        Entity testEntity2 = new TestEntityA(2);

        var result = testEntity1.GetHashCode() == testEntity2.GetHashCode();
        
        Assert.False(result);
    }
    
    [Fact]
    public void CompareHashCodesWithDefaultIds()
    {
        Entity testEntity1 = new TestEntityA();
        Entity testEntity2 = new TestEntityA();

        var result = testEntity1.GetHashCode() == testEntity2.GetHashCode();
        
        Assert.False(result);
    }
    
    private sealed class TestEntityA : Entity
    {
        public TestEntityA() {}
        public TestEntityA(long id)
        {
            Id = id;
        }
    }
    
    private sealed class TestEntityB : Entity
    {
        public TestEntityB() {}
        public TestEntityB(long id)
        {
            Id = id;
        }
    }
}