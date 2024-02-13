using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Tests.Unit.Domain.SeedWork;

public partial class ValueObjectTests
{
    [Fact]
    public void EqualOperatorWithSameReferences()
    {
        var valueObject = new TestValueObject(null, "test-value");
        
        // Act
        var result = valueObject == valueObject;

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void EqualOperatorWithNull()
    {
        var valueObject = new TestValueObject(null, "test-value");
        
        // Act
        var result = valueObject == null;

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void NotEqualOperatorWithSameReferences()
    {
        var valueObject = new TestValueObject(null, "test-value");
        
        // Act
        var result = valueObject != valueObject;

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void NotEqualOperatorWithNull()
    {
        var valueObject = new TestValueObject(null, "test-value");
        
        // Act
        var result = valueObject != null;

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void CompareHashCodesOfEqualObjects()
    {
        var valueObject1 = new TestValueObject(null, "test-value");
        var valueObject2 = new TestValueObject(null, "test-value");
        
        // Act
        var result = valueObject1.GetHashCode() == valueObject2.GetHashCode();

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void CopyAndEquals()
    {
        var valueObject1 = new TestValueObject(null, "test-value");
        var valueObject2 = valueObject1.GetCopy();
        
        // Act
        var result = valueObject1.Equals(valueObject2);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void CopyAndCompareReferences()
    {
        var valueObject1 = new TestValueObject(null, "test-value");
        var valueObject2 = valueObject1.GetCopy();
        
        // Act
        var result = valueObject1 != valueObject2;

        // Assert
        Assert.True(result);
    }
    
    private class TestValueObject : ValueObject
    {
        public TestValueObject(string? value1, string value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public string? Value1 { get; }
        public string Value2 { get; }
        
        public static bool operator ==(TestValueObject left, TestValueObject right)
        {
            return EqualOperator(left, right);
        }
        
        public static bool operator !=(TestValueObject left, TestValueObject right)
        {
            return NotEqualOperator(left, right);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value1;
            yield return Value2;
        }
    }
}