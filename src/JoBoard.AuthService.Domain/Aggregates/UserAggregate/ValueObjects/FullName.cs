using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

public class FullName : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    
    public FullName(string firstName, string lastName)
    {
        DomainGuard.IsNotNullOrWhiteSpace(firstName);
        DomainGuard.IsNotNullOrWhiteSpace(lastName);
        
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}