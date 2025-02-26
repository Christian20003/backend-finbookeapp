using System.Net.Mail;
using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public async Task SecurityCode(IUserResetRequest request)
    {
        /* var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
        //var user = await CheckUserAccount(_protector.Protect(request.Email));
        Random res = new();
        var code = "";

        for (int i = 0; i < 6; i++)
        {
            code += chars[res.Next(36)];
        }
        new SmtpClient("smtp.gmail.com", 587).Send(
            "noreply@finbooke.com",
            request.Email,
            "Security code for password reset",
            code
        ); */
        throw new NotImplementedException();
    }
}
