namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface ISession
{
    /// <summary>
    /// A JWT token for authentication.
    /// </summary>
    public IToken Token { get; set; }

    /// <summary>
    /// A refresh token to be able of refreshing the JWT.
    /// </summary>
    public IRefreshToken RefreshToken { get; set; }
}
