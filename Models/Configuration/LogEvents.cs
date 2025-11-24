namespace FinBookeAPI.Models.Configuration;

/// <summary>
/// This enumeration includes all possible log events.
/// </summary>
public static class LogEvents
{
    public const int AuthenticationRequest = 4000;
    public const int OperationIgnored = 5000;
    public const int AuthenticationSuccess = 7000;
    public const int ResetCredentialsSuccess = 7010;
    public const int AuthenticationFailed = 8000;
    public const int AuthorizationFailed = 8005;
    public const int ResetCredentialsFailed = 8010;
    public const int ConfigurationError = 9000;
}
