using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.DTO.Authentication;

public class RefreshTokenDTO
{
    public string Id { get; set; } = "";

    public string Token { get; set; } = "";

    public long Expires { get; set; } = 0;

    public RefreshTokenDTO() { }

    public RefreshTokenDTO(RefreshToken token)
    {
        Id = token.Id;
        Token = token.Token;
        Expires = token.ExpiresAt.Ticks;
    }
}
