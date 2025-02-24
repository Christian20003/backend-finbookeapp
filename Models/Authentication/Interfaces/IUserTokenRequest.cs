namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserTokenRequest
{
    // The email of the user
    public string Email { get; set; }

    // The refresh token to authenticate
    public IRefreshToken Token { get; set; }
}
