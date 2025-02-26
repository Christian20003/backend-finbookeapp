namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserClient
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The name of the user.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The email address of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The path to a profil image.
    /// </summary>
    public string ImagePath { get; set; }

    /// <summary>
    /// A session object storing authentication tokens.
    /// </summary>
    public ISession Session { get; set; }
}
