using FinBookeAPI.Collections.TokenCollection;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Collections;

public static class MockTokenCollection
{
    public static Mock<ITokenCollection> GetMock()
    {
        var result = new Mock<ITokenCollection>();
        result.Setup(obj => obj.Contains(It.IsAny<string>())).ReturnsAsync(true);
        return result;
    }
}
