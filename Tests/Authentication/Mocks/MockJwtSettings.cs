using FinBookeAPI.Models.Configuration;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockJwtSettings
{
    public static Mock<IJwtSettings> GetMock()
    {
        var obj = new Mock<IJwtSettings>();
        obj.SetupProperty(o => o.Secret, TestData.Secret);
        obj.SetupProperty(o => o.Audience, "");
        obj.SetupProperty(o => o.Issuer, "");
        return obj;
    }
}
