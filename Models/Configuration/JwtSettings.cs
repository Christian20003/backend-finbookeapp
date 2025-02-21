namespace FinBookeAPI.Models.Configuration;

/// <summary>
/// Class <c>JwTSettings</c> models the configurations for authentication data in <c>appsettings.json</c>
/// as well as secret section.
/// </summary>
public class JwTSettings : IJwtSettings
{
    /// <summary>
    /// The name of the section in the <c>appsettings.json</c> file.
    /// </summary>
    public const string SectionName = "JwtConfig";
    public string Issuer { get; set; } = "";
    public string Audience { get; set; } = "";
    public string Secret { get; set; } = "";
}
