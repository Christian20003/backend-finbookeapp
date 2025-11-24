using FinBookeAPI.AppConfig.Redaction;
using FinBookeAPI.Models.Wrapper;
using Microsoft.Extensions.Compliance.Classification;
using Microsoft.Extensions.Compliance.Redaction;
using Moq;

namespace FinBookeAPI.Tests.Mocks.Manager;

public static class MockRedactorProvider
{
    public static Mock<IRedactorProvider> GetMock()
    {
        var obj = new Mock<IRedactorProvider>();
        obj.Setup(o => o.GetRedactor(It.IsAny<DataClassificationSet>()))
            .Returns(new StarRedactor());
        return obj;
    }
}
