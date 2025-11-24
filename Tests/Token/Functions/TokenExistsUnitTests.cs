using Moq;

namespace FinBookeAPI.Tests.Token;

public partial class TokenServiceUnitTests
{
    [Fact]
    public async Task Should_ReturnTrue_WhenTokenIsInCollection()
    {
        var token = _service.GenerateAccessToken(_userId);
        _collection.Setup(obj => obj.Contains(It.IsAny<string>())).ReturnsAsync(true);

        var result = await _service.TokenExists(token.Value);
        Assert.True(result);
    }

    [Fact]
    public async Task Should_ReturnFalse_WhenTokenIsNotInCollection()
    {
        var token = _service.GenerateAccessToken(_userId);
        _collection.Setup(obj => obj.Contains(It.IsAny<string>())).ReturnsAsync(false);

        var result = await _service.TokenExists(token.Value);
        Assert.False(result);
    }

    [Fact]
    public async Task Should_FailToCheck_WhenDatabaseOperationFails()
    {
        var token = _service.GenerateAccessToken(_userId);
        _collection
            .Setup(obj => obj.Contains(It.IsAny<string>()))
            .ThrowsAsync(new OperationCanceledException());

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _service.TokenExists(token.Value)
        );
    }
}
