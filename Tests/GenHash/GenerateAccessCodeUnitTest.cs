using FinBookeAPI.Services.GenHash;
using Moq;

namespace FinBookeAPI.Tests.GenHash;

public partial class GenerateAccessCodeUnitTest
{
    private readonly GenHashService _service;

    public GenerateAccessCodeUnitTest()
    {
        var logger = new Mock<ILogger<GenHashService>>();
        _service = new GenHashService(logger.Object);
    }

    [Fact]
    public void Should_FailToGenerateAccessCode_WhenInvalidLength()
    {
        Assert.Throws<ArgumentException>(() => _service.GenerateAccessCode(-5));
        Assert.Throws<ArgumentException>(() => _service.GenerateAccessCode(0));
    }

    [Fact]
    public void Should_ReturnNotEmptyAccessCode_WhenValidLength()
    {
        var result = _service.GenerateAccessCode(15);

        Assert.NotEqual("", result);
    }

    [Fact]
    public void Should_ReturnAccessCodeWithRequiredLength_WhenValidLength()
    {
        var result = _service.GenerateAccessCode(15);

        Assert.Equal(15, result.Length);
    }
}
