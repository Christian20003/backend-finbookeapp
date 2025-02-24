using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class represents a user object sent to the client.
/// </summary>
public class UserClient : IUserClient
{
    [Required]
    public string Id { get; set; } = "";

    [Required]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    public string ImagePath { get; set; } = "";

    [Required]
    public Interfaces.ISession Session { get; set; } = new Session();
}
