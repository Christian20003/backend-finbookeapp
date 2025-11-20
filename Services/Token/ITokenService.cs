using FinBookeAPI.Models.Token;
using Microsoft.EntityFrameworkCore;
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
    /// This method verifies if a given refresh token is a valid JWT.
    /// </summary>
    /// <param name="token">
    /// The token that should be verified
    /// </param>
    /// <returns>
    /// The user id and the time where the token expires.
    /// </returns>
    /// <exception cref="ApplicationException">
    /// If required configuration data is null or the secret to generate a symmetric key
    /// has less than 16 bytes.
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
    public (string, long) VerifyRefreshToken(string token);

    /// <summary>
    /// This method verifies if a given access token is a valid JWT.
    /// </summary>
    /// <param name="token">
    /// The token that should be verified
    /// </param>
    /// <returns>
    /// The user id and the time where this token expires.
    /// </returns>
    /// <exception cref="ApplicationException">
    /// If required configuration data is null or the secret to generate a symmetric key
    /// has less than 16 bytes.
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
    /// If the user id is not inside the access token.
    /// </exception>
    public (string, long) VerifyAccessToken(string token);

    /// <summary>
    /// This method stores the access token in a database.
    /// </summary>
    /// <param name="token">
    /// The token that should be stored.
    /// </param>
    /// <exception cref="ApplicationException">
    /// If required configuration data is null or the secret to generate a symmetric key
    /// has less than 16 bytes.
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
    /// If the user id is not inside the access token.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// If the insertion operation in the database failed.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the database operation has been canceled.
    /// </exception>
    public Task StoreAccessToken(string token);

    /// <summary>
    /// This method stores the refresh token in a database.
    /// </summary>
    /// <param name="token">
    /// The token that should be stored.
    /// </param>
    /// <exception cref="ApplicationException">
    /// If required configuration data is null or the secret to generate a symmetric key
    /// has less than 16 bytes.
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
    /// <exception cref="DbUpdateException">
    /// If the insertion operation in the database failed.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the database operation has been canceled.
    /// </exception>
    public Task StoreRefreshToken(string token);

    /// <summary>
    /// This method proofs if the token exists in the database.
    /// </summary>
    /// <param name="token">
    /// The token value that should be checked.
    /// </param>
    /// <returns>
    /// <c>true</c> if the token exists in the database, otherwise <c>false</c>.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// If the database operation has been canceled.
    /// </exception>
    public Task<bool> TokenExists(string token);

    /// <summary>
    /// This method deletes all tokens in the database that have been expired.
    /// </summary>
    /// <exception cref="DbUpdateException">
    /// If the database update failed.
    /// </exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// If tokens to be deleted have been modified.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the database operation has been canceled.
    /// </exception>
    public Task CleanTokenDatabase();
}
