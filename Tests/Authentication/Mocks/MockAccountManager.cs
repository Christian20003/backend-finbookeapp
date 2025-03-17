using FinBookeAPI.Models.Wrapper;
using Moq;

namespace FinBookeAPI.Tests.Authentication.Mocks;

public class MockAccountManager
{
    public static Mock<IAccountManager> GetMock()
    {
        var obj = new Mock<IAccountManager>();
        return obj;
    }
}
