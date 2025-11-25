namespace FinBookeAPI.Models.Exceptions;

/// <summary>
/// This class models an exception thrown if a specifc resource is locked and cannot
/// be accessed from the current enquirer.
/// </summary>
public class ResourceLockedException : Exception
{
    public ResourceLockedException(string? message, Exception e)
        : base(message, e) { }

    public ResourceLockedException(string? message)
        : base(message) { }

    public ResourceLockedException()
        : base() { }
}
