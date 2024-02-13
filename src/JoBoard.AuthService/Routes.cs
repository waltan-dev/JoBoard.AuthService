namespace JoBoard.AuthService;

// DO NOT CHANGE PUBLIC CONTRACT
public static class AuthTokenV1Routes
{
    private const string Base = "/api/v1/auth/token";
    
    public const string TokenByPassword = $"{Base}/by-password";
    public const string TokenByGoogle = $"{Base}/by-google";
    
    public const string RefreshToken = $"{Base}/refresh";
    public const string RevokeRefreshToken = $"{Base}/revoke";
}

// DO NOT CHANGE PUBLIC CONTRACT
public static class AccountV1Routes
{
    private const string Base = "/api/v1/account";
    
    public const string Register = $"{Base}/register";
    public const string RegisterByGoogle = $"{Base}/register-google";
    
    public const string ConfirmEmail = $"{Base}/confirm-email";
    
    public const string RequestPasswordReset = $"{Base}/request-password-reset";
    public const string ConfirmPasswordReset = $"{Base}/confirm-password-reset";
}

// DO NOT CHANGE PUBLIC CONTRACT
public static class ManageAccountV1Routes
{
    public const string Base = "/api/v1/account/manage";
    
    public const string ChangePassword = $"{Base}/change-password";
    
    public const string RequestEmailChange = $"{Base}/request-email-change";
    public const string ConfirmEmailChange = $"{Base}/confirm-email-change";
    
    public const string ChangeRole = $"{Base}/change-role";
    
    public const string RequestAccountDeactivation = $"{Base}/request-account-deactivation";
    public const string ConfirmAccountDeactivation = $"{Base}/confirm-account-deactivation";
}

// DO NOT CHANGE PUBLIC CONTRACT
public static class ManageExternalAccountV1Routes
{
    public const string Base = "/api/v1/account/manage/external";
    
    public const string AttachGoogleAccount = $"{Base}/attach-google";
    
    public const string DetachExternalAccount = $"{Base}/detach";
}