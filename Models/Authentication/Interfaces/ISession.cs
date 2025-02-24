namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface ISession
{
    // A JWT token for authentication
    public IToken Token { get; set; }

    // A refresh token to be able of refreshing the JWT
    public IRefreshToken RefreshToken { get; set; }
}
