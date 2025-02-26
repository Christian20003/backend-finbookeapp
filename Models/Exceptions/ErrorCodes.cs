namespace FinBookeAPI.Models.Exceptions;

/// <summary>
/// This enumeration includes all possible exception cases.
/// </summary>
public enum ErrorCodes
{
    ENTRY_NOT_FOUND,
    INVALID_ENTRY,
    UPDATE_FAILED,
    INSERT_FAILED,
    OPERATION_CANCELED,
    UNAUTHORIZED,
    SERVER_ERROR,
}
