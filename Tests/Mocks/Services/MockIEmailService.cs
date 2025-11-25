using FinBookeAPI.Services.Email;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Services;

public static class MockIEmailService
{
    public static Mock<IEmailService> GetMock()
    {
        var obj = new Mock<IEmailService>();
        return obj;
    }
}
