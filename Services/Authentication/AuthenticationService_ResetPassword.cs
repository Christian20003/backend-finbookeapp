using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public Task ResetPassword(IUserResetRequest request)
    {
        throw new NotImplementedException();
    }
}
