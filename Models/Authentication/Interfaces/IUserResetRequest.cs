namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserResetRequest
{
    /// <summary>
    /// The email of the reset password request.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The generated code to authenticate for password reset.
    /// </summary>
    public string? Code { get; set; }
}
