namespace FinBookeAPI.Tests.CategoryType;

public partial class CategoryServiceUnitTests
{
    [Fact]
    public async Task Should_FailGeneratingCategory_WhenNameIsEmpty()
    {
        _category.Name = "";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_FailGeneratingCategory_WhenColorIsEmpty()
    {
        _category.Color = "";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_FailGeneratingCategory_WhenChildDoesNotExist()
    {
        _category.UserId = _database.Last().UserId;
        _category.Children = [_database.Last().Id, new Guid()];

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_FailGeneratingCategory_WhenChildIsNotOwned()
    {
        _category.Children = [_database.Last().Id];

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_StoreCategory()
    {
        await _service.CreateMainCategory(_category);

        Assert.Contains(_database, elem => elem.Id == _category.Id);
    }

    [Fact]
    public async Task Should_ReturnCreatedCategory()
    {
        var result = await _service.CreateMainCategory(_category);

        Assert.Equal(_category.Id, result.Id);
        Assert.Equal(_category.Name, result.Name);
        Assert.Equal(_category.UserId, result.UserId);
        Assert.Equal(_category.Color, result.Color);
    }
}
