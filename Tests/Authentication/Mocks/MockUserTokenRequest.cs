using FinBookeAPI.Models.Authentication.Interfaces;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockUserTokenRequest
{
    public static Mock<IUserTokenRequest> GetMock()
    {
        var obj = new Mock<IUserTokenRequest>();
        var token = MockRefreshToken.GetMock();
        obj.Setup(obj => obj.Token).Returns(token.Object);
        return obj;
    }
}
