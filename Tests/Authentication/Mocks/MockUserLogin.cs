using FinBookeAPI.Models.Authentication;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockUserLogin
{
    public static Mock<IUserLogin> GetMock()
    {
        var obj = new Mock<IUserLogin>();
        obj.SetupProperty(o => o.Email, TestData.Email);
        obj.SetupProperty(o => o.Password, TestData.Password);
        return obj;
    }
}
