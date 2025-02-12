namespace FinBookeAPI.Models.Exceptions;

public class AuthenticationException : Exception
{
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
