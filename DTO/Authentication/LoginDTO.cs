using System.ComponentModel.DataAnnotations;
using FinBookeAPI.Models.Authentication;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.DTO.Authentication;

public class LoginDTO
{
    [Required(ErrorMessage = "Email property is missing")]
    [EmailAddress(ErrorMessage = "Email property should be a valid email address")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Password property is missing")]
    public string Password { get; set; } = "";

    public IUserLogin GetUserLogin()
    {
        return new UserLogin { Email = Email, Password = Password };
    }
}
