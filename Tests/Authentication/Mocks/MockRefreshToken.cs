using FinBookeAPI.Models.Authentication.Interfaces;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockRefreshToken
{
    public static Mock<IRefreshToken> GetMock()
    {
        var obj = new Mock<IRefreshToken>();
        obj.SetupProperty(o => o.Id, TestData.TokenId);
        obj.SetupProperty(o => o.UserId, TestData.UserId);
        obj.SetupProperty(o => o.Token, TestData.Token);
        return obj;
    }
}
