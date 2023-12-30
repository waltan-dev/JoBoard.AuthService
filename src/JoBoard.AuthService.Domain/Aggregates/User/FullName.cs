using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class FullName : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }
    
    public FullName(string firstName, string lastName)
    {
        Guard.IsNotNullOrWhiteSpace(firstName);
        Guard.IsNotNullOrWhiteSpace(lastName);
        
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}