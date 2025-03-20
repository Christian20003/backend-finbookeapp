using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.DTO.Authentication;

/// <summary>
/// The class <c>RegisterDTO</c> models a transfer object for a register request.
/// </summary>
public class RegisterDTO
{
    /// <summary>
    /// The email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email porperty is missing")]
    [EmailAddress(ErrorMessage = "Email property should be a valid email address")]
    public string Email { get; set; } = "";

    /// <summary>
    /// The user name.
    /// </summary>
    [Required(ErrorMessage = "Name property is missing")]
    public string Name { get; set; } = "";

    /// <summary>
    /// The password of the user account.
    /// </summary>
    [Required(ErrorMessage = "Password property is missing")]
    public string Password { get; set; } = "";

    /// <summary>
    /// This method converts the current <c>RegisterDTO</c> instance into an object that
    /// can be processed by the provided <c>AuthenticationService</c>.
    /// </summary>
    /// <returns>
    /// The object which can be used in the <c>AuthenticationService</c>.
    /// </returns>
    public UserRegister GetUserRegister()
    {
        return new UserRegister
        {
            Email = Email,
            Password = Password,
            Name = Name,
        };
    }
}
