using FinBookeAPI.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockUserManager
{
    public static Mock<UserManager<IUserDatabase>> GetMock()
    {
        var obj = new Mock<UserManager<IUserDatabase>>(
            Mock.Of<IUserStore<IUserDatabase>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<IUserDatabase>>(),
            FakeItEasy.A.Fake<IEnumerable<IUserValidator<IUserDatabase>>>(),
            FakeItEasy.A.Fake<IEnumerable<IPasswordValidator<IUserDatabase>>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<IUserDatabase>>>()
        );
        return obj;
    }
}
