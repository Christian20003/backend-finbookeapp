using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Services.Token;
using FinBookeAPI.Tests.Token.Records;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Token;

public class GenerateRefreshTokenUnitTest
{
    private readonly TokenService _service;
    private readonly Mock<IOptions<JwtSettings>> _settings;
    private readonly JwtSettings _options = JwtSettingsRecord.GetObject();
    private const string _userId = "6ccf3b8a-0886-4213-a998-4a0356cb68dd";

    public GenerateRefreshTokenUnitTest()
    {
        var logger = new Mock<ILogger<TokenService>>();
        _settings = new Mock<IOptions<JwtSettings>>();
        _settings.Setup(obj => obj.Value).Returns(_options);
        _service = new TokenService(_settings.Object, logger.Object);
    }

    [Fact]
    public void Should_FailToGenerateRefreshToken_WhenSecretIsNull()
    {
        _options.RefreshTokenSecret = null;

        Assert.Throws<ApplicationException>(() => _service.GenerateRefreshToken(_userId));
    }

    [Fact]
    public void Should_FailToGenerateRefreshToken_WhenIssuerIsNull()
    {
        _options.Issuer = null;

        Assert.Throws<ApplicationException>(() => _service.GenerateRefreshToken(_userId));
    }

    [Fact]
    public void Should_FailToGenerateRefreshToken_WhenAudienceIsNull()
    {
        _options.Audience = null;

        Assert.Throws<ApplicationException>(() => _service.GenerateRefreshToken(_userId));
    }

    [Fact]
    public void Should_FailToGenerateRefreshToken_WhenSecretIsTooShort()
    {
        _options.RefreshTokenSecret = "1234";

        Assert.Throws<ApplicationException>(() => _service.GenerateRefreshToken(_userId));
    }

    [Fact]
    public void Should_FailToGenerateRefreshToken_WhenExpirationTimeIsSmallerThanZero()
    {
        _options.RefreshTokenExpireD = 0;

        Assert.Throws<ApplicationException>(() => _service.GenerateRefreshToken(_userId));
    }

    [Fact]
    public void Should_GenerateNotEmptyRefreshToken()
    {
        var token = _service.GenerateRefreshToken(_userId);

        Assert.NotEqual("", token.Value);
    }

    [Fact]
    public void Should_GenerateRefreshTokenWhichExpires()
    {
        _options.RefreshTokenExpireD = 1;
        var token = _service.GenerateRefreshToken(_userId);
        var time = DateTime.UtcNow;

        Assert.True(token.Expires > time.Ticks);
    }
}
