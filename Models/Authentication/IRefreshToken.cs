namespace FinBookeAPI.Models.Authentication;

public interface IRefreshToken
{
    // Id of the token
    public string Id { get; set; }

    // The refresh token as string
    public string Token { get; set; }

    // The id of the user which this token corresponds to
    public string UserId { get; set; }

    // Date when this token expires
    public DateTime ExpiresAt { get; set; }

    // Date when this token was created
    public DateTime CreatedAt { get; set; }
}
