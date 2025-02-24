namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserResetRequest
{
    // The email of the user
    public string Email { get; set; }

    // The provided code to authenticate for password reset
    public string? Code { get; set; }
}
