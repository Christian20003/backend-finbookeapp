namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserClient
{
    // The id of the user
    public string Id { get; set; }

    // The name of the user
    public string Name { get; set; }

    // The email address of the user
    public string Email { get; set; }

    // The path to a profil image
    public string ImagePath { get; set; }

    // A session object storing authentication tokens
    public ISession Session { get; set; }
}
