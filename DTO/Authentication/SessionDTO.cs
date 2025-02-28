using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.DTO.Authentication;

public class SessionDTO
{
    public string JwtToken { get; set; } = "";

    public long JwtExpires { get; set; } = 0;

    public RefreshTokenDTO RefreshToken { get; set; } = new RefreshTokenDTO();

    public SessionDTO() { }

    public SessionDTO(Session session)
    {
        JwtToken = session.Token.Value;
        JwtExpires = session.Token.Expires;
        RefreshToken = new RefreshTokenDTO(session.RefreshToken);
    }
}
