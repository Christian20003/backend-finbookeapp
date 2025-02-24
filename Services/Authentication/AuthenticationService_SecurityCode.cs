using FinBookeAPI.Models.Authentication.Interfaces;

namespace FinBookeAPI.Services.Authentication;

public partial class AuthenticationService : IAuthenticationService
{
    public Task SecurityCode(IUserResetRequest request)
    {
        throw new NotImplementedException();
    }
}
