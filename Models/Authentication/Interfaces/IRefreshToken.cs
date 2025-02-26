namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IRefreshToken
{
    /// <summary>
    /// The id of the token.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The refresh token as string.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// The id of the user which this token corresponds to.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// The date when this token expires.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// The date when this token was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// This method generates a random string with multiple characters which
    /// will be used as token value.
    /// </summary>
    public void GenerateTokenValue();

    /// <summary>
    /// This method hashes the value of this token.
    /// </summary>
    public void HashValue();
}
