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

    public AuthenticationException(string message, ErrorCodes code, Exception e)
        : base(message, e)
    {
        Code = code;
    }

    public AuthenticationException(string message, ErrorCodes code)
        : base(message)
    {
        Code = code;
    }
}
