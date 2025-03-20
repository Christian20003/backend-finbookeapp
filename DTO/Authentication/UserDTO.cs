using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.DTO.Authentication;

/// <summary>
/// The class <c>UserDTO</c> models the transfer object of a user account including
/// all necessary data which must be stored on the client side.
/// </summary>
public class UserDTO
{
    /// <summary>
    /// The id of the user.
    /// </summary>
    public string Id { get; set; } = "";

    /// <summary>
    /// The name of the user.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// The email address of the user.
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// The path to the profile image.
    /// </summary>
    public string ImagePath { get; set; } = "";

    /// <summary>
    /// An object containing all necessary tokens for the current session.
    /// </summary>
    public SessionDTO Session { get; set; } = new SessionDTO();

    public UserDTO() { }

    public UserDTO(UserClient user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        ImagePath = user.ImagePath;
        Session = new SessionDTO(user.Session);
    }
}
