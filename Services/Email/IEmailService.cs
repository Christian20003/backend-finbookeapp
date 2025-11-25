using System.Net.Mail;

namespace FinBookeAPI.Services.Email;

public interface IEmailService
{
    /// <summary>
    /// This method sends a message to the provided email address.
    /// </summary>
    /// <param name="email">
    /// The recipient's email address.
    /// </param>
    /// <param name="subject">
    /// The subject of the email.
    /// </param>
    /// <param name="body">
    /// The body content of the email.
    /// </param>
    /// <param name="isHtml">
    /// If <c>true</c>, the body content is treated as HTML.
    /// If <c>false</c>, the body content is treated as plain text.
    /// </param>
    /// <exception cref="ApplicationException">
    /// If one of the following conditions is true:
    /// <list type="bullet">
    ///     <item>SMTP-Host is null.</item>
    ///     <item>SMTP-Port is zero, negativ or larger than 65.535.</item>
    ///     <item>Email-Address of sender is null or not a valid email address.</item>
    /// </list>
    /// </exception>
    /// <exception cref="SmtpException">
    /// If the connection with the SMTP-Server failed as well as authentication.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// If the message object has been deleted, before sending.
    /// </exception>
    /// <exception cref="SmtpFailedRecipientException">
    /// If the message could not be sent to the client.
    /// </exception>
    public void Send(string email, string subject, string body, bool isHtml = true);
}
