using FinBookeAPI.Models.Token;
using Microsoft.IdentityModel.Tokens;

namespace FinBookeAPI.Services.Token;

public interface ITokenService
{
    /// <summary>
    /// This method generates a new access token.
    /// </summary>
    /// <param name="userId">
    /// The id of the user who is owner of this token.
    /// </param>
    /// <returns>
    /// The newly generates token.
    /// </returns>
    /// <exception cref="ApplicationException">
    /// If one of the following conditions is true:
    /// <list type="bullet">
    ///     <item>If required configuration data is null.</item>
    ///     <item>If the required secret to generate a symmetric key is too small (less than 16 bytes).</item>
    ///     <item>If the expiration time is smaller than zero.</item>
    /// </list>
    /// </exception>
    public JwtToken GenerateAccessToken(string userId);

    /// <summary>
    /// This method generates a new refresh token.
    /// </summary>
    /// <param name="userId">
    /// The if of the user who is owner of this token.
    /// </param>
    /// <returns>
    /// The newly generated refresh token.
    /// </returns>
    /// <exception cref="ApplicationException">
    /// If one of the following conditions is true:
    /// <list type="bullet">
    ///     <item>If required configuration data is null.</item>
    ///     <item>If the required secret to generate a symmetric key is too small (less than 16 bytes).</item>
    ///     <item>If the expiration time is smaller than zero.</item>
    /// </list>
    /// </exception>
    public JwtToken GenerateRefreshToken(string userId);

    /// <summary>
    /// This method verifies a refresh token
    /// </summary>
    /// <param name="token">
    /// The token that should be verified
    /// </param>
    /// <returns>
    /// The user id stored in the refresh token.
    /// </returns>
    /// <exception cref="ApplicationException">
    /// If required configuration data is null.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// If the provided token is null or empty.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If the provided token exceeds the maximum length.
    /// </exception>
    /// <exception cref="SecurityTokenMalformedException">
    /// If the token does not fulfill the required structure.
    /// </exception>
    /// <exception cref="SecurityTokenEncryptionKeyNotFoundException">
    /// If the 'kid' header claim is not null AND decryption fails.
    /// </exception>
    /// <exception cref="SecurityTokenException">
    /// If the 'enc' header claim is null or empty.
    /// </exception>
    /// <exception cref="SecurityTokenExpiredException">
    /// If the token has expired.
    /// </exception>
    /// <exception cref="SecurityTokenInvalidSignatureException">
    /// If the signature is not valid.
    /// </exception>
    /// <exception cref="SecurityTokenInvalidIssuerException">
    /// If the 'issuer' property in the token is invalid.
    /// </exception>
    /// <exception cref="SecurityTokenInvalidAudienceException">
    /// The 'audience' property in the token is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// If the user id is not inside the refresh token.
    /// </exception>
    public string VerifyRefreshToken(string token);
}
