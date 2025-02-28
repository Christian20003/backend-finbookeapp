using FinBookeAPI.Models.Authentication;

namespace FinBookeAPI.DTO.Authentication;

public class UserDTO
{
    public string Id { get; set; } = "";

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public string ImagePath { get; set; } = "";

    public SessionDTO Session { get; set; } = new SessionDTO();

    public UserDTO() { }

    public UserDTO(UserClient user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        ImagePath = user.ImagePath;
        Session = new SessionDTO(user.Session);
    }
}
