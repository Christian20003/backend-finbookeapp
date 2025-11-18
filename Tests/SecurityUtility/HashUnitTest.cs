using FinBookeAPI.Services.SecurityUtility;
using Moq;

namespace FinBookeAPI.Tests.SecurityUtility;

public partial class HashUnitTest
{
    private readonly SecurityUtilityService _service;

    public HashUnitTest()
    {
        var logger = new Mock<ILogger<SecurityUtilityService>>();
        _service = new SecurityUtilityService(logger.Object);
    }

    [Fact]
    public void Should_GenerateNotEmptyHashValue()
    {
        var hash = _service.Hash("content");

        Assert.NotEqual("", hash);
    }

    [Fact]
    public void Should_ReturnHashValueWithSalt()
    {
        var hash = _service.Hash("content");

        Assert.Contains("-", hash);
    }

    [Fact]
    public void Should_Return16ByteHashValue()
    {
        var hash = _service.Hash("content");
        var parts = hash.Split("-");

        Assert.Equal(32, parts[0].Length);
    }

    [Fact]
    public void Shoud_Return16ByteSaltValue()
    {
        var hash = _service.Hash("content");
        var parts = hash.Split("-");

        Assert.Equal(32, parts[1].Length);
    }

    [Fact]
    public void Should_FailVerification_WhenHashValueIsInvalid()
    {
        Assert.Throws<ArgumentException>(() => _service.IsHash("content", "abcde"));
    }

    [Fact]
    public void Should_ReturnFalse_WhenHashValuesAreUnequal()
    {
        var hash = _service.Hash("content");
        var check = _service.IsHash("different content", hash);

        Assert.False(check);
    }

    [Fact]
    public void Should_ReturnTrue_WhenHashValuesAreEqual()
    {
        var hash = _service.Hash("content");
        var check = _service.IsHash("content", hash);

        Assert.True(check);
    }
}
