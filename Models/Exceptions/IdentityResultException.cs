using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Models.Exceptions;

public class IdentityResultException : Exception
{
    public IEnumerable<IdentityError> Errors { get; }

    public IdentityResultException(IEnumerable<IdentityError> errors, string? msg)
        : base(msg)
    {
        Errors = errors;
    }

    public IdentityResultException(
        IEnumerable<IdentityError> errors,
        string? msg,
        Exception exception
    )
        : base(msg, exception)
    {
        Errors = errors;
    }
}
