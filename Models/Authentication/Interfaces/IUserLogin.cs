namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserLogin
{
    /// <summary>
    /// The email of the login request.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The password of the login request.
    /// </summary>
    public string Password { get; set; }
}
