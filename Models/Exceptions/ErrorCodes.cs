namespace FinBookeAPI.Models.Exceptions;

/// <summary>
/// This enumeration includes all possible exception cases.
/// </summary>
public enum ErrorCodes
{
    CONFIG_NOT_FOUND,
    ENTRY_NOT_FOUND,
    UNEXPECTED_STRUCTURE,
    UPDATE_FAILED,
    INSERT_FAILED,
    ACCESS_DENIED,
    ACCESS_EXPIRED,
    DATABASE_ERROR,
    EXTERNAL_SERVICE_ERROR,
}
