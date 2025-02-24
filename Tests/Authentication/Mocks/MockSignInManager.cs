using FinBookeAPI.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockSignInManager
{
    public static Mock<SignInManager<UserDatabase>> GetMock(Mock<UserManager<UserDatabase>> manager)
    {
        var obj = new Mock<SignInManager<UserDatabase>>(
            manager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<UserDatabase>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<ILogger<SignInManager<UserDatabase>>>(),
            Mock.Of<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<UserDatabase>>()
        );
        return obj;
    }
}
