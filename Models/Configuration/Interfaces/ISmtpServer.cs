namespace FinBookeAPI.Models.Configuration.Interfaces;

public interface ISmtpServer
{
    /// <summary>
    /// The host of the SMTP-Server
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// The port of the SMTP-Server
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// The username for authentication on the SMTP-Server
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The password for authentication on the SMTP-Server
    /// </summary>
    public string Password { get; set; }
}
