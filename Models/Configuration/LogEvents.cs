namespace FinBookeAPI.Models.Configuration;

/// <summary>
/// Class <c>LogEvents</c> models every possible log event id.
/// </summary>
public class LogEvents
{
    public const int MISSING_PROPERTY = 1000;

    public const int SUCCESSFUL_LOGIN = 3000;

    public const int FAILED_UPDATE = 4001;
    public const int FAILED_INSERT = 4002;
}
