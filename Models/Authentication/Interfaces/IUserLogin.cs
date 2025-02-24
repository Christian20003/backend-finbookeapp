namespace FinBookeAPI.Models.Authentication.Interfaces;

public interface IUserLogin
{
    public string Email { get; set; }
    public string Password { get; set; }
}
