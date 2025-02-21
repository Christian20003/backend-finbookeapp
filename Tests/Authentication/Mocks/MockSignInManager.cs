using FinBookeAPI.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockSignInManager
{
    public static Mock<SignInManager<IUserDatabase>> GetMock(
        Mock<UserManager<IUserDatabase>> manager
    )
    {
        var obj = new Mock<SignInManager<IUserDatabase>>(
            manager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IUserDatabase>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<ILogger<SignInManager<IUserDatabase>>>(),
            Mock.Of<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<IUserDatabase>>()
        );
        return obj;
    }
}
