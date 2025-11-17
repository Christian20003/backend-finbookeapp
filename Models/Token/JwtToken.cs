namespace FinBookeAPI.Models.Token;

/// <summary>
/// This class models a security token for authentication (JWT).
/// </summary>
public class JwtToken
{
    public string Value { get; set; } = "";
    public long Expires { get; set; }
}
