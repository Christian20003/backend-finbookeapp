using System.ComponentModel.DataAnnotations;

namespace FinBookeAPI.DTO.Authentication.Input;

/// <summary>
/// This class represents a transfer object for logout requests.
/// </summary>
public class LogoutDTO
{
    [Required(ErrorMessage = "Access token property is missing")]
    public string AccessToken { get; set; } = "";

    [Required(ErrorMessage = "Refresh token property is missing")]
    public string RefreshToken { get; set; } = "";
}
