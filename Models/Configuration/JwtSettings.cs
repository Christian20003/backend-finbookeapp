namespace FinBookeAPI.Models.Configuration;

/// <summary>
/// Class <c>JwtSettings</c> models the configurations for authentication data in <c>appsettings.json</c>
/// as well as secret section.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// The name of the section in the <c>appsettings.json</c> file.
    /// </summary>
    public const string SectionName = "JwtConfig";

    /// <summary>
    /// The <c>URI</c> of the host which generates a JWT token.
    /// </summary>
    public string? Issuer { get; set; } = "";

    /// <summary>
    /// The <c>URI</c> of the host which receives the JWT token for validation.
    /// </summary>
    public string? Audience { get; set; } = "";

    /// <summary>
    /// The secret to verify and generate JWT tokens.
    /// </summary>
    public string? Secret { get; set; } = "";
}
