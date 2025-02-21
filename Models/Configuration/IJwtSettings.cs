namespace FinBookeAPI.Models.Configuration;

public interface IJwtSettings
{
    /// <summary>
    /// The <c>URI</c> of the host which generates a JWT token.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// The <c>URI</c> of the host which receives the JWT token for validation.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// The secret to verify and generate JWT tokens.
    /// </summary>
    public string Secret { get; set; }
}
