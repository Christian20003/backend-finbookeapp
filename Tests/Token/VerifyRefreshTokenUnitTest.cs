using FinBookeAPI.Collections.TokenCollection;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Token;
using FinBookeAPI.Services.Token;
using FinBookeAPI.Tests.Token.Records;
using Microsoft.Extensions.Options;
using Moq;

namespace FinBookeAPI.Tests.Token;

public class VerifyRefreshTokenUnitTest
{
    private readonly TokenService _service;
    private readonly Mock<IOptions<JwtSettings>> _settings;
    private readonly JwtSettings _options = JwtSettingsRecord.GetObject();
    private readonly JwtToken _token;
    private const string userId = "6ccf3b8a-0886-4213-a998-4a0356cb68dd";

    public VerifyRefreshTokenUnitTest()
    {
        var logger = new Mock<ILogger<TokenService>>();
        var collection = new Mock<ITokenCollection>();
        _settings = new Mock<IOptions<JwtSettings>>();
        _settings.Setup(obj => obj.Value).Returns(_options);
        _service = new TokenService(collection.Object, _settings.Object, logger.Object);
        _token = _service.GenerateRefreshToken(userId);
    }

    [Fact]
    public void Should_FailVerification_WhenSecretIsNull()
    {
        _options.RefreshTokenSecret = null;

        Assert.Throws<ApplicationException>(() => _service.VerifyRefreshToken(_token.Value));
    }

    [Fact]
    public void Should_FailVerification_WhenIssuerIsNull()
    {
        _options.Issuer = null;

        Assert.Throws<ApplicationException>(() => _service.VerifyRefreshToken(_token.Value));
    }

    [Fact]
    public void Should_FailVerification_WhenAudienceIsNull()
    {
        _options.Audience = null;

        Assert.Throws<ApplicationException>(() => _service.VerifyRefreshToken(_token.Value));
    }

    [Fact]
    public void Should_SucceedVerification_WhenAllPropertiesAreValid()
    {
        var (id, _) = _service.VerifyRefreshToken(_token.Value);

        Assert.Equal(userId, id);
    }
}
