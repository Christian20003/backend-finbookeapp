namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserTokenRequest
{
    /// <summary>
    /// The email of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The refresh token to authenticate.
    /// </summary>
    public IRefreshToken Token { get; set; }
}
