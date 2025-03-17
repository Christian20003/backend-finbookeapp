namespace FinBookeAPI.Models.Configuration;

/// <summary>
/// Class <c>LogEvents</c> models every possible log event id.
/// </summary>
public class LogEvents
{
    public const int OPERATION_SUCCESS = 2000;

    public const int OBJECT_INVALID = 4000;
    public const int PROPERTY_MISSING = 4001;
    public const int PROPERTY_INVALID = 4002;
    public const int PROPERTY_UNEQUAL = 4003;
    public const int PROPERTY_TOO_SMALL = 4004;
    public const int PROPERTY_TOO_LARGE = 4005;
    public const int OPERATION_FAILED = 4006;
    public const int UPDATE_FAILED = 4007;
    public const int INSERT_FAILED = 4008;
    public const int SEARCH_FAILED = 4009;
    public const int DELETE_FAILED = 4010;
}
