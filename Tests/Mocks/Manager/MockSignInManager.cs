using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Manager;

public static class MockSignInManager
{
    public static Mock<SignInManager<T>> GetMock<T>(Mock<UserManager<T>> manager)
        where T : class
    {
        var result = new Mock<SignInManager<T>>(
            manager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<T>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<ILogger<SignInManager<T>>>(),
            Mock.Of<Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<T>>()
        );
        result
            .Setup(obj =>
                obj.CheckPasswordSignInAsync(It.IsAny<T>(), It.IsAny<string>(), It.IsAny<bool>())
            )
            .ReturnsAsync(SignInResult.Success);
        return result;
    }
}
