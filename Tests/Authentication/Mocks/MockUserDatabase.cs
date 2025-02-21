using FinBookeAPI.Models.Authentication;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockUserDatabase
{
    public static Mock<IUserDatabase> GetMock()
    {
        var obj = new Mock<IUserDatabase>();
        obj.SetupProperty(o => o.Id, TestData.UserId);
        obj.SetupProperty(o => o.UserName, TestData.Username);
        obj.SetupProperty(o => o.Email, TestData.Email);
        obj.SetupProperty(o => o.PasswordHash, TestData.PasswordHash);
        obj.SetupProperty(o => o.RefreshTokenId, TestData.TokenId);
        return obj;
    }
}
