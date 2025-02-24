namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserRegister
{
    // The email address of the user
    public string Email { get; set; }

    // The name of the user
    public string Name { get; set; }

    // The password of the user
    public string Password { get; set; }
}
