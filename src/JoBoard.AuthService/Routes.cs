﻿namespace JoBoard.AuthService;

// DO NOT CHANGE PUBLIC CONTRACT
public static class AuthV1Routes
{
    private const string Base = "/api/v1/auth";
    
    public const string Register = $"{Base}/register";
    public const string RegisterByGoogle = $"{Base}/register-google";
    
    public const string Login = $"{Base}/login";
    public const string LoginByGoogle = $"{Base}/login-google";
    
    public const string ConfirmEmail = $"{Base}/confirm-email";
    
    public const string RequestPasswordReset = $"{Base}/request-password-reset";
    public const string ConfirmPasswordReset = $"{Base}/confirm-password-reset";
}