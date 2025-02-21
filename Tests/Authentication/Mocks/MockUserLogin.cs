using FinBookeAPI.Models.Authentication;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockUserLogin
{
    public static Mock<UserLogin> GetMock()
    {
        var obj = new Mock<UserLogin>();
        obj.SetupProperty(o => o.Email, "max.mustermann@gmail.com");
        obj.SetupProperty(o => o.Password, "12345");
        return obj;
    }
}
