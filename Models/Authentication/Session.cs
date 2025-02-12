namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a session object which store the authentication token and it's expiration time.
/// </summary>
/// <param name="token">The value of the token</param>
/// <param name="expires">The time of expiration</param>
public class Session(string token, long expires)
{
    public string Token { get; set; } = token;
    public long Expires { get; set; } = expires;
}
