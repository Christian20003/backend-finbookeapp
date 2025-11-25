using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.DTO.Authentication.Output;

/// <summary>
/// This class models the transfer object of authentication data.
/// </summary>
public class SessionDTO(User user)
{
    public string AccessToken { get; set; } = user.AccessToken.Value;

    public long AccessTokenExpires { get; set; } = user.AccessToken.Expires;

    public string RefreshToken { get; set; } = user.RefreshToken.Value;

    public long RefreshTokenExpires { get; set; } = user.RefreshToken.Expires;
}
