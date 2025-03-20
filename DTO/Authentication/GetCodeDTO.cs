using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.DTO.Authentication;

/// <summary>
/// The class <c>GetCodeDTO</c> represents a transfer object to get a security code to
/// be able of reseting a users password.
/// </summary>
public class GetCodeDTO
{
    /// <summary>
    /// The email address of the user account.
    /// </summary>
    [Required(ErrorMessage = "Email property is missing")]
    [EmailAddress(ErrorMessage = "Email property should be a valid email address")]
    public string Email { get; set; } = "";

    /// <summary>
    /// This method converts the current <c>GetCodeDTO</c> instance into an object that
    /// can be processed by the provided <c>AuthenticationService</c>.
    /// </summary>
    /// <returns>
    /// The object which can be used in the <c>AuthenticationService</c>.
    /// </returns>
    public UserResetRequest GetUserResetRequest()
    {
        return new UserResetRequest { Email = Email };
    }
}
