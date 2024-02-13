using JoBoard.AuthService.Domain.Common.Exceptions;

namespace JoBoard.AuthService.Domain.Common.SeedWork;

public abstract class BusinessObject
{
    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
            throw new DomainException(rule.Message);
    }
}