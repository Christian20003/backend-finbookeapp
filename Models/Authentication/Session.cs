namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a session object which store the authentication token and it's expiration time.
/// </summary>
public class Session()
{
    public Token Token { get; set; } = new Token();
    public IRefreshToken RefreshToken { get; set; } = new RefreshToken();
}
