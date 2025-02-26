using FinBookeAPI.Models.Configuration.Interfaces;

namespace FinBookeAPI.Models.Configuration;

/// <summary>
/// This class models the SMTP-Server settings to be able of sending mails.
/// </summary>
public class SmtpServer : ISmtpServer
{
    /// <summary>
    /// The name of the section in the <c>appsettings.json</c> file.
    /// </summary>
    public const string SectionName = "Smtp";

    public string Host { get; set; } = "";

    public int Port { get; set; } = 0;

    public string Username { get; set; } = "";

    public string Password { get; set; } = "";
}
