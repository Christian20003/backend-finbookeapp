using FinBookeAPI.Models.Wrapper;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Manager;

public static class MockDataProtection
{
    public static Mock<IDataProtection> GetMock()
    {
        var obj = new Mock<IDataProtection>();
        obj.Setup(o => o.Protect(It.IsAny<string>())).Returns((string input) => input);
        obj.Setup(o => o.ProtectEmail(It.IsAny<string>())).Returns((string input) => input);
        obj.Setup(o => o.Unprotect(It.IsAny<string>())).Returns((string input) => input);
        obj.Setup(o => o.UnprotectEmail(It.IsAny<string>())).Returns((string input) => input);
        return obj;
    }
}
