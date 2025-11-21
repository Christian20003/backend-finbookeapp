using FinBookeAPI.Services.Token;
using FinBookeAPI.Tests.Records;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Services;

public static class MockITokenService
{
    public static Mock<ITokenService> GetMock(string userId)
    {
        var obj = new Mock<ITokenService>();
        var token = JwtTokenRecord.GetObject();
        obj.Setup(o => o.GenerateAccessToken(It.IsAny<string>())).Returns(token);
        obj.Setup(o => o.GenerateRefreshToken(It.IsAny<string>())).Returns(token);
        obj.Setup(o => o.VerifyRefreshToken(It.IsAny<string>())).Returns((userId, token.Expires));
        return obj;
    }
}
