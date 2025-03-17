namespace FinBookeAPI.Models.Exceptions;

/// <summary>
/// This class models an exception thrown during authentication procedures.
/// </summary>
public class AuthenticationException : Exception
{
    /// <summary>
    /// This attribute defines a short reason of this exception.
    /// </summary>
    public ErrorCodes Code { get; }

    public AuthenticationException(ErrorCodes code, string message, Exception e)
        : base(message, e)
    {
        Code = code;
    }

    public AuthenticationException(ErrorCodes code, string message)
        : base(message)
    {
        Code = code;
    }
}
