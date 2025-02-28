namespace FinBookeAPI.Models.Configuration;

/// <summary>
/// Class <c>LogEvents</c> models every possible log event id.
/// </summary>
public class LogEvents
{
    public const int MISSING_OBJECT = 1000;
    public const int MISSING_PROPERTY = 1001;

    public const int SUCCESSFUL_OPERATION = 2000;
    public const int SUCCESSFUL_LOGIN = 2001;

    public const int INCOMING_REQUEST = 3001;

    public const int FAILED_OPERATION = 4000;
    public const int FAILED_UPDATE = 4001;
    public const int FAILED_INSERT = 4002;
    public const int FAILED_SEARCH = 4003;
    public const int FAILED_CHECK = 4004;

    public const int UNAUTHORIZED = 5000;
}
