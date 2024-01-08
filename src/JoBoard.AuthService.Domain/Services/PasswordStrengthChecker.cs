namespace JoBoard.AuthService.Domain.Services;

public static class PasswordStrengthChecker
{
    public static int MinPasswordLength { get; } = 6;
    public static int MaxPasswordLength { get; } = 32;

    public static bool Check(string password)
    {
        bool containsMin = password.Length >= MinPasswordLength;
        bool lessThanMaxLimit = password.Length <= MaxPasswordLength;
        bool containsUpper = false;
        bool containsLower = false;
        bool containsDigits = false;
        bool containsNonAlphanumeric = false;
        
        foreach (var passwordChar in password)
        {
            if (char.IsLetter(passwordChar))
            {
                if (char.IsUpper(passwordChar))
                    containsUpper = true;
                if (char.IsLower(passwordChar))
                    containsLower = true;
                
                continue;
            }

            if (char.IsDigit(passwordChar))
            {
                containsDigits = true;
                continue;
            }

            if (char.IsLetterOrDigit(passwordChar) == false)
            {
                containsNonAlphanumeric = true;
                continue;
            }
        }

        return containsMin && lessThanMaxLimit 
                           && containsUpper && containsLower 
                           && containsDigits && containsNonAlphanumeric;
    }
}