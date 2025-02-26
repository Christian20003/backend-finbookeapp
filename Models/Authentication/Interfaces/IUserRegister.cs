namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserRegister
{
    /// <summary>
    /// The email address of the register request.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// The user name of the register request.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The password of the register request.
    /// </summary>
    public string Password { get; set; }
}
