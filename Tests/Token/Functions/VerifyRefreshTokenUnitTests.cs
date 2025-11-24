namespace FinBookeAPI.Tests.Token;

public partial class TokenServiceUnitTests
{
    [Fact]
    public void Should_FailRefreshTokenVerification_WhenSecretIsNull()
    {
        _options.RefreshTokenSecret = null;

        Assert.Throws<ApplicationException>(() => _service.VerifyRefreshToken(_token.Value));
    }

    [Fact]
    public void Should_FailRefreshTokenVerification_WhenIssuerIsNull()
    {
        _options.Issuer = null;

        Assert.Throws<ApplicationException>(() => _service.VerifyRefreshToken(_token.Value));
    }

    [Fact]
    public void Should_FailRefreshTokenVerification_WhenAudienceIsNull()
    {
        _options.Audience = null;

        Assert.Throws<ApplicationException>(() => _service.VerifyRefreshToken(_token.Value));
    }

    [Fact]
    public void Should_SucceedRefreshTokenVerification_WhenAllPropertiesAreValid()
    {
        var token = _service.GenerateRefreshToken(_userId);
        var (id, _) = _service.VerifyRefreshToken(token.Value);

        Assert.Equal(_userId, id);
    }
}
