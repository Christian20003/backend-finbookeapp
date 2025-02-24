using FinBookeAPI.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockUserManager
{
    public static Mock<UserManager<UserDatabase>> GetMock()
    {
        var obj = new Mock<UserManager<UserDatabase>>(
            Mock.Of<IUserStore<UserDatabase>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<UserDatabase>>(),
            FakeItEasy.A.Fake<IEnumerable<IUserValidator<UserDatabase>>>(),
            FakeItEasy.A.Fake<IEnumerable<IPasswordValidator<UserDatabase>>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<UserDatabase>>>()
        );
        return obj;
    }
}
