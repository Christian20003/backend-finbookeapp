using FinBookeAPI.Models.Token;
using Moq;

namespace FinBookeAPI.Tests.Token;

public partial class TokenServiceUnitTests
{
    [Fact]
    public async Task Should_AddAccessTokenToCollection()
    {
        var token = _service.GenerateAccessToken(_userId);

        await _service.StoreAccessToken(token.Value);

        _collection.Verify(obj => obj.Add(It.IsAny<JwtToken>()), Times.Once);
    }

    [Fact]
    public async Task Should_FailToStoreAccessToken_WhenDatabaseOperationFails()
    {
        var token = _service.GenerateAccessToken(_userId);
        _collection
            .Setup(obj => obj.Add(It.IsAny<JwtToken>()))
            .ThrowsAsync(new OperationCanceledException());

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _service.StoreAccessToken(token.Value)
        );
    }
}
