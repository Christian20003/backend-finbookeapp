using FinBookeAPI.Models.Wrapper;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public static class MockDataProtection
{
    public static Mock<IDataProtection> GetMock()
    {
        var obj = new Mock<IDataProtection>();
        obj.Setup(o => o.Protect(It.IsAny<string>())).Returns((string input) => input);
        obj.Setup(o => o.Unprotect(It.IsAny<string>())).Returns((string input) => input);
        return obj;
    }
}
