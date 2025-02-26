using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class represents a user object sent to the client.
/// </summary>
public class UserClient : IUserClient
{
    [Required(ErrorMessage = "User id is required")]
    public string Id { get; set; } = "";

    [Required(ErrorMessage = "Username is required")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid")]
    public string Email { get; set; } = "";

    public string ImagePath { get; set; } = "";

    [Required(ErrorMessage = "Session object is required")]
    public Interfaces.ISession Session { get; set; } = new Session();
}
