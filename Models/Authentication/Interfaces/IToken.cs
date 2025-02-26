using FinBookeAPI.Models.Configuration.Interfaces;
using Microsoft.Extensions.Options;

namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IToken
{
    /// <summary>
    ///  The token value.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// The date after this token expires in milliseconds.
    /// </summary>
    public long Expires { get; set; }

    /// <summary>
    /// This method generates a new token value with the provided configuration
    /// in the settings object.
    /// </summary>
    /// <param name="settings">
    /// The options containing all necessary configuration data from
    /// <c>appsettings.json</c>.
    /// </param>
    /// <param name="identity">
    /// The main identity value for the subject property in a JWT.
    /// </param>
    public void GenerateTokenValue(IOptions<IJwtSettings> settings, string identity);
}
