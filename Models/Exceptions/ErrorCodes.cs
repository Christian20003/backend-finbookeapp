namespace FinBookeAPI.Models.Exceptions;

/// <summary>
/// This enumeration includes all possible exception cases.
/// </summary>
public enum ErrorCodes
{
    // Authentication specific
    INVALID_CREDENTIALS,
    INVALID_TOKEN,
    EXPIRED_TOKEN,
    INVALID_CODE,
    EXPIRED_CODE,
    ACCESS_LOCKED,

    // Structure specifc
    UNEXPECTED_STRUCTURE,

    // CRUD-operation specifc
    ENTRY_NOT_FOUND,
    UPDATE_FAILED,
    INSERT_FAILED,
    DELETE_FAILED,

    // Dependency specifc
    CONFIG_NOT_FOUND,
    DATABASE_ERROR,
    EXTERNAL_SERVICE_ERROR,
}
