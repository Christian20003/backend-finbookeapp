using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class represents a user object sent to the client.
/// </summary>
public class UserClient
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    [Required(ErrorMessage = "User id is required")]
    public string Id { get; set; } = "";

    /// <summary>
    /// The name of the user.
    /// </summary>
    [Required(ErrorMessage = "Username is required")]
    public string Name { get; set; } = "";

    /// <summary>
    /// The email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid")]
    public string Email { get; set; } = "";

    /// <summary>
    /// The path to a profil image.
    /// </summary>
    public string ImagePath { get; set; } = "";

    /// <summary>
    /// A session object storing authentication tokens.
    /// </summary>
    [Required(ErrorMessage = "Session object is required")]
    public Session Session { get; set; } = new Session();
}
