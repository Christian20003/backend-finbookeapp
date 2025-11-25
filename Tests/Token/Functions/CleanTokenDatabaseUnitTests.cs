using Microsoft.EntityFrameworkCore;
using Moq;

namespace FinBookeAPI.Tests.Token;

public partial class TokenServiceUnitTests
{
    [Fact]
    public async Task Should_DeleteTokensFromDatabase()
    {
        await _service.CleanTokenDatabase();

        _collection.Verify(obj => obj.Delete(), Times.Once);
    }

    [Fact]
    public async Task Should_FailDeletion_WhenDatabaseOperationFails()
    {
        _collection.Setup(obj => obj.Delete()).ThrowsAsync(new DbUpdateException());

        await Assert.ThrowsAsync<DbUpdateException>(() => _service.CleanTokenDatabase());
    }

    [Fact]
    public async Task Should_FailDeletion_WhenEntitesHaveBeenModified()
    {
        _collection.Setup(obj => obj.Delete()).ThrowsAsync(new DbUpdateConcurrencyException());

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _service.CleanTokenDatabase());
    }
}
