namespace FinBookeAPI.Tests.Token;

public partial class TokenServiceUnitTests
{
    [Fact]
    public void Should_FailAccessTokenVerification_WhenSecretIsNull()
    {
        _options.AccessTokenSecret = null;

        Assert.Throws<ApplicationException>(() => _service.VerifyAccessToken(_token.Value));
    }

    [Fact]
    public void Should_FailAccessTokenVerification_WhenIssuerIsNull()
    {
        _options.Issuer = null;

        Assert.Throws<ApplicationException>(() => _service.VerifyAccessToken(_token.Value));
    }

    [Fact]
    public void Should_FailAccessTokenVerification_WhenAudienceIsNull()
    {
        _options.Audience = null;

        Assert.Throws<ApplicationException>(() => _service.VerifyAccessToken(_token.Value));
    }

    [Fact]
    public void Should_SucceedAccessTokenVerification_WhenAllPropertiesAreValid()
    {
        var token = _service.GenerateAccessToken(_userId);
        var (id, _) = _service.VerifyAccessToken(token.Value);

        Assert.Equal(_userId, id);
    }
}
