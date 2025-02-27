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
    /// This method proofs if any user account exist in the authentication database from the provided email.
    /// </summary>
    /// <param name="email">
    /// The email of the user account.
    /// </param>
    /// <returns>
    /// An instance of <c>UserDatabase</c> which represents a single user account.
    /// </returns>
    /// <exception cref="AuthenticationException">
    /// If not any user account could be found or the found instance has an empty username or email property.
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
            throw new AuthenticationException("Empty username", ErrorCodes.INVALID_ENTRY);
        }
        // Proof if email property is set
        if (user.Email == null)
        {
            _logger.LogWarning(
                LogEvents.MISSING_PROPERTY,
                "Unexpected missing email property of user {id}",
                user.Id
            );
            throw new AuthenticationException("Empty email", ErrorCodes.INVALID_ENTRY);
        }
        return user;
    }

    /// <summary>
    /// This method proofs if the user has provided a valid refresh token which has been
    /// assigned to his account.
    /// </summary>
    /// <param name="token">
    /// The refresh token received from the user.
    /// </param>
    /// <param name="user">
    /// The user account from the database
    /// </param>
    /// <exception cref="AuthenticationException">
    /// If the user does not have any refresh tokens or the refresh token is invalid. It can
    /// also happen if the database cancel any of its operations.
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
                throw new AuthenticationException("Invalid refresh token", ErrorCodes.UNAUTHORIZED);
            }
            /* if (storedToken.ExpiresAt.Ticks < DateTime.UtcNow.Ticks)
            {
                _logger.LogWarning(LogEvents.UNAUTHORIZED, "Refresh token has expired");
                throw new AuthenticationException(
                    "Refresh token has expired",
                    ErrorCodes.UNAUTHORIZED
                );
            } */
        }
        catch (OperationCanceledException exception)
        {
            _logger.LogError(LogEvents.FAILED_OPERATION, "Database operation has been canceled");
            throw new AuthenticationException(
                "Database operation has been canceled",
                ErrorCodes.OPERATION_CANCELED,
                exception
            );
        }
    }

    /// <summary>
    /// This method updates a user account in the database.
    /// </summary>
    /// <param name="user">
    /// The user object with all updated properties.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// If the update fails.
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
    /// This method sends the provided message to the defined address through an SMTP-Server.
    /// </summary>
    /// <param name="message">
    /// The message which should be sent.
    /// </param>
    /// <exception cref="AuthenticationException">
    /// If the SMTP request fails and the message could not be transmitted.
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
                ErrorCodes.SERVER_ERROR,
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
