using Microsoft.AspNetCore.Identity;

namespace FinBookeAPI.Models.Authentication;

/// <summary>
/// This class models a user of this application.
/// </summary>
public class UserDatabase : IdentityUser
{
    // Properties: Name, Email, Password are already implemented in base class
    // This string store the path to the profile image
    public string ImagePath { get; set; } = "";
}
