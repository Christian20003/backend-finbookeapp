using System.Net;
using System.Net.Mail;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Authentication.Interfaces;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    /// <summary>
    /// This method proofs if any user account exist in the authentication database with the provided email address. This method will
    /// throw an <c><see cref="AuthenticationException"/></c> if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided email does not have a user account (<see cref="ErrorCodes"/>: <c>ENTRY_NOT_FOUND</c>).</item>
    ///     <item>The found user account has an empty string as username property (<see cref="ErrorCodes"/>: <c>UNEXPECTED_STRUCTURE</c>).</item>
    ///     <item>The found user account has an empty string as email property (<see cref="ErrorCodes"/>: <c>UNEXPECTED_STRUCTURE</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="email">
    /// The email of the user account.
    /// </param>
    /// <returns>
    /// An instance of <c>UserDatabase</c> which represents a single user account.
    /// </returns>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    private async Task<UserDatabase> CheckUserAccount(string email)
    {
        _logger.LogDebug("Find user account of {user}", email);
        var user = await _userManager.FindByEmailAsync(email);
        // Proof if user exist
        if (user == null)
        {
            _logger.LogWarning(LogEvents.FAILED_SEARCH, "User account could not be found");
            throw new AuthenticationException("User not found", ErrorCodes.ENTRY_NOT_FOUND);
        }
        // Proof if username property is set
        if (user.UserName == null)
        {
            _logger.LogWarning(
                LogEvents.MISSING_PROPERTY,
                "Unexpected missing username property of user {id}",
                user.Id
            );
            throw new AuthenticationException("Empty username", ErrorCodes.UNEXPECTED_STRUCTURE);
        }
        // Proof if email property is set
        if (user.Email == null)
        {
            _logger.LogWarning(
                LogEvents.MISSING_PROPERTY,
                "Unexpected missing email property of user {id}",
                user.Id
            );
            throw new AuthenticationException("Empty email", ErrorCodes.UNEXPECTED_STRUCTURE);
        }
        return user;
    }

    /// <summary>
    /// This method proofs the validity of the provided <c>token</c>. This method will throw an <c><see cref="AuthenticationException"/></c>
    /// if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided user account does not have a refresh token (<see cref="ErrorCodes"/>: <c>ENTRY_NOT_FOUND</c>).</item>
    ///     <item>The provided token does not correspond to the stored token (<see cref="ErrorCodes"/>: <c>ACCESS_DENIED</c>).</item>
    ///     <item>The stored token has expired (<see cref="ErrorCodes"/>: <c>ACCESS_EXPIRED</c>).</item>
    ///     <item>Necessary database operations have been canceled (<see cref="ErrorCodes"/>: <c>DATABASE_ERROR</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="token">
    /// The refresh token received from a client.
    /// </param>
    /// <param name="user">
    /// The user account from the database.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    private async Task CheckRefreshToken(IRefreshToken token, UserDatabase user)
    {
        _logger.LogDebug("Proof existence of refresh token for {user}", user.Email);
        try
        {
            var storedToken = await _database.FindRefreshToken(obj =>
                obj.Id == user.RefreshTokenId
            );
            if (storedToken == null)
            {
                _logger.LogInformation(LogEvents.MISSING_OBJECT, "Refresh token not found");
                throw new AuthenticationException(
                    "Refresh token not found",
                    ErrorCodes.ENTRY_NOT_FOUND
                );
            }
            token.HashValue();
            if (storedToken.Token != token.Token)
            {
                _logger.LogWarning(
                    LogEvents.UNAUTHORIZED,
                    "Invalid refresh token provided for logout"
                );
                throw new AuthenticationException(
                    "Invalid refresh token",
                    ErrorCodes.ACCESS_DENIED
                );
            }
            if (storedToken.ExpiresAt.Ticks < DateTime.UtcNow.Ticks)
            {
                _logger.LogWarning(LogEvents.UNAUTHORIZED, "Refresh token has expired");
                throw new AuthenticationException(
                    "Refresh token has expired",
                    ErrorCodes.ACCESS_EXPIRED
                );
            }
        }
        catch (OperationCanceledException exception)
        {
            _logger.LogError(LogEvents.FAILED_OPERATION, "Database operation has been canceled");
            throw new AuthenticationException(
                "Database operation has been canceled",
                ErrorCodes.DATABASE_ERROR,
                exception
            );
        }
    }

    /// <summary>
    /// This method updates a user account in the authentication database. This method will throw an <c><see cref="AuthenticationException"/></c>
    /// if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided user account could not be updated (<see cref="ErrorCodes"/>: <c>UPDATE_FAILED</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="user">
    /// The user account with updated properties.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    private async Task UpdateUser(UserDatabase user)
    {
        _logger.LogDebug("Update user account of {user}", user.Email);
        var update = await _userManager.UpdateAsync(user);
        if (!update.Succeeded)
        {
            _logger.LogWarning(
                LogEvents.FAILED_UPDATE,
                "Refresh token could not be updated for user - {user}",
                user.Id
            );
            throw new AuthenticationException("Failed update of user", ErrorCodes.UPDATE_FAILED);
        }
    }

    /// <summary>
    /// This method sends the provided <c>message</c> to the defined address through an SMTP-Server. This method
    /// will throw an <c><see cref="AuthenticationException"/></c> if one of the following occurs:
    /// <list type="bullet">
    ///     <item>The provided message could not be sent due to an SMTP-Server error (<see cref="ErrorCodes"/>: <c>EXTERNAL_SERVICE_ERROR</c>).</item>
    /// </list>
    /// </summary>
    /// <param name="message">
    /// The message which should be sent.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// See method description.
    /// </exception>
    private void SendEmail(MailMessage message)
    {
        _logger.LogDebug("Send an email to {user}", message.To);
        try
        {
            var server = _mailServer.Value;
            var smtpClient = new SmtpClient
            {
                Host = server.Host,
                Port = server.Port,
                Credentials = new NetworkCredential(server.Username, server.Password),
                EnableSsl = true,
            };
            smtpClient.Send(message);
        }
        catch (Exception exception)
            when (exception is InvalidOperationException
                || exception is ObjectDisposedException
                || exception is SmtpException
                || exception is SmtpFailedRecipientException
                || exception is SmtpFailedRecipientsException
            )
        {
            _logger.LogError(LogEvents.FAILED_OPERATION, "SMTP-Server does not sent the email");
            throw new AuthenticationException(
                "Mail could not be sent",
                ErrorCodes.EXTERNAL_SERVICE_ERROR,
                exception
            );
        }
    }

    /// <summary>
    /// This method generates a random string based on the characters defined in <c>options</c>.
    /// </summary>
    /// <param name="options">
    /// A string of all possible characters.
    /// </param>
    /// <param name="length">
    /// The length of the resulting string.
    /// </param>
    /// <returns>
    /// The randomly generated string.
    /// </returns>
    private static string GenerateRandomString(string options, int length)
    {
        Random res = new();
        var result = "";

        for (int i = 0; i < length; i++)
        {
            result += options[res.Next(options.Length)];
        }
        return result;
    }
}
