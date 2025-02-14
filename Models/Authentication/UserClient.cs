using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class represents a user object sent to the client.
/// </summary>
public class UserClient
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
    public Session Session { get; set; } = new Session();
}
