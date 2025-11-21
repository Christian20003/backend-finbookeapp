using FinBookeAPI.Services.SecurityUtility;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Services;

public static class MockISecurityUtilityService
{
    public static Mock<ISecurityUtilityService> GetMock()
    {
        var obj = new Mock<ISecurityUtilityService>();
        obj.Setup(o => o.GenerateAccessCode(It.IsAny<int>())).Returns("H638IZ");
        obj.Setup(o => o.GeneratePassword(It.IsAny<int>())).Returns("RandomPassword");
        obj.Setup(o => o.Hash(It.IsAny<string>()))
            .Returns("bf1a94dd05271d2b7eee257d9f2248bcefdca0acd5708f343f8d791d6a6044b9");
        obj.Setup(o => o.IsHash(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        return obj;
    }
}
