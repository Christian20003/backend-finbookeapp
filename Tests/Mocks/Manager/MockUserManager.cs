using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Manager;

public static class MockUserManager
{
    public static Mock<UserManager<T>> GetMock<T>()
        where T : class
    {
        var obj = new Mock<UserManager<T>>(
            Mock.Of<IUserStore<T>>(),
            Mock.Of<IOptions<IdentityOptions>>(),
            Mock.Of<IPasswordHasher<T>>(),
            FakeItEasy.A.Fake<IEnumerable<IUserValidator<T>>>(),
            FakeItEasy.A.Fake<IEnumerable<IPasswordValidator<T>>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<T>>>()
        );
        return obj;
    }
}
