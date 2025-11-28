namespace FinBookeAPI.Tests.CategoryType;

public partial class CategoryServiceUnitTests
{
    [Fact]
    public async Task Should_ThrowArgumentException_WhenUserIdIsEmpty()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.GetCategories(Guid.Empty));
    }

    [Fact]
    public async Task Should_ReturnNestedCategories()
    {
        var result = await _service.GetCategories(_database[1].UserId);

        Assert.Single(result);
        Assert.Contains(result.First().Children, child => child.Id == _database[2].Id);
        Assert.Contains(result.First().Children, child => child.Id == _database[3].Id);
    }
}
