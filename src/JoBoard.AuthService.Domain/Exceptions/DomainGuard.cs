namespace JoBoard.AuthService.Domain.Exceptions;

public static class DomainGuard
{
    public static void IsEmail(string email)
    {
        
    }
    
    public static void IsNotDefault(Guid value)
    {
        if(value == default)
            throw new DomainException("Value can not be default");
    }
    
    public static void IsNotDefault(TimeSpan value)
    {
        if(value == default)
            throw new DomainException("Value can not be default");
    }
    
    public static void IsGreaterThan(TimeSpan value, TimeSpan min) 
    {
        if(min > value)
            throw new DomainException("Value must be greater");
    }
    
    public static void IsNotNullOrWhiteSpace(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Value can not be empty");
    }
}