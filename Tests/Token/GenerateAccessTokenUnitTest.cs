using FinBookeAPI.Collections.TokenCollection;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Services.Token;
using FinBookeAPI.Tests.Token.Records;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Token;

public class GenerateAccessTokenUnitTest
{
    private readonly TokenService _service;
    private readonly Mock<IOptions<JwtSettings>> _settings;
    private readonly JwtSettings _options = JwtSettingsRecord.GetObject();
    private const string _userId = "6ccf3b8a-0886-4213-a998-4a0356cb68dd";

    public GenerateAccessTokenUnitTest()
    {
        var logger = new Mock<ILogger<TokenService>>();
        var collection = new Mock<ITokenCollection>();
        _settings = new Mock<IOptions<JwtSettings>>();
        _settings.Setup(obj => obj.Value).Returns(_options);
        _service = new TokenService(collection.Object, _settings.Object, logger.Object);
    }

    [Fact]
    public void Should_FailToGenerateAccessToken_WhenSecretIsNull()
    {
        _options.AccessTokenSecret = null;

        Assert.Throws<ApplicationException>(() => _service.GenerateAccessToken(_userId));
    }

    [Fact]
    public void Should_FailToGenerateAccessToken_WhenIssuerIsNull()
    {
        _options.Issuer = null;

        Assert.Throws<ApplicationException>(() => _service.GenerateAccessToken(_userId));
    }

    [Fact]
    public void Should_FailToGenerateAccessToken_WhenAudienceIsNull()
    {
        _options.Audience = null;

        Assert.Throws<ApplicationException>(() => _service.GenerateAccessToken(_userId));
    }

    [Fact]
    public void Should_FailToGenerateAccessToken_WhenSecretIsTooShort()
    {
        _options.AccessTokenSecret = "1234";

        Assert.Throws<ApplicationException>(() => _service.GenerateAccessToken(_userId));
    }

    [Fact]
    public void Should_FailToGenerateAccessToken_WhenExpirationTimeIsSmallerThanZero()
    {
        _options.AccessTokenExpireM = 0;

        Assert.Throws<ApplicationException>(() => _service.GenerateAccessToken(_userId));
    }

    [Fact]
    public void Should_GenerateNotEmptyAccessToken()
    {
        var token = _service.GenerateAccessToken(_userId);

        Assert.NotEqual("", token.Value);
    }

    [Fact]
    public void Should_GenerateAccessTokenWhichExpires()
    {
        _options.AccessTokenExpireM = 1;
        var token = _service.GenerateAccessToken(_userId);
        var time = DateTime.UtcNow;

        Assert.True(token.Expires > time.Ticks);
    }
}
