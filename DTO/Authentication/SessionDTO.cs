using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.DTO.Authentication;

/// <summary>
/// The class <c>SessionDTO</c> models the transfer object of authentication data.
/// </summary>
public class SessionDTO
{
    /// <summary>
    /// The JWT to be authenticated of accessing restricted ressources.
    /// </summary>
    public string JwtToken { get; set; } = "";

    /// <summary>
    /// The time when the provided JWT expires.
    /// </summary>
    public long JwtExpires { get; set; } = 0;

    /// <summary>
    /// The refresh token to be able of updating the JWT.
    /// </summary>
    public string RefreshToken { get; set; } = "";

    /// <summary>
    /// The time when the provided refresh token expires.
    /// </summary>
    public long RefreshExpires { get; set; } = 0;

    public SessionDTO() { }

    public SessionDTO(Session session)
    {
        JwtToken = session.Token.Value;
        JwtExpires = session.Token.Expires;
        RefreshToken = session.RefreshToken.Token;
        RefreshExpires = session.RefreshToken.ExpiresAt.Ticks;
    }
}
