using FinBookeAPI.Models.Token;

namespace FinBookeAPI.DTO.Authentication.Output;

/// <summary>
/// This class represents a transfer object with a refreshed access token.
/// </summary>
public class RefreshedTokenDTO(JwtToken token)
{
    public string AccessToken { get; set; } = token.Value;

    public long AccessTokenExpired { get; set; } = token.Expires;
}
