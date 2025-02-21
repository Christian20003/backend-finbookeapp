namespace FinBookeAPI.Models.Authentication;

public interface IUserLogin
{
    public string Email { get; set; }
    public string Password { get; set; }
}
