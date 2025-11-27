using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Tests.CategoryType;

public partial class CategoryServiceUnitTests
{
    [Fact]
    public async Task Should_FailCreatingMainCategory_WhenNameIsEmpty()
    {
        _category.Name = "";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingMainCategory_WhenUserIdIsEmpty()
    {
        _category.UserId = Guid.Empty;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingMainCategory_WhenColorIsEmpty()
    {
        _category.Color = "";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingMainCategory_WhenColorHasInvalidFormat()
    {
        _category.Color = "abcde";

        await Assert.ThrowsAsync<FormatException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingMainCategory_WhenChildDoesNotExist()
    {
        _category.UserId = _database.Last().UserId;
        _category.Children = [_database.Last().Id, Guid.NewGuid()];

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingMainCategory_WhenChildIsNotOwned()
    {
        _category.Children = [_database.Last().Id];

        await Assert.ThrowsAsync<AuthorizationException>(
            () => _service.CreateMainCategory(_category)
        );
    }

    [Fact]
    public async Task Should_StoreMainCategory()
    {
        await _service.CreateMainCategory(_category);

        Assert.Contains(_database, elem => elem.Id == _category.Id);
    }

    [Fact]
    public async Task Should_ReturnCreatedMainCategory()
    {
        var result = await _service.CreateMainCategory(_category);

        Assert.Equal(_category.Id, result.Id);
        Assert.Equal(_category.Name, result.Name);
        Assert.Equal(_category.UserId, result.UserId);
        Assert.Equal(_category.Color, result.Color);
    }
}
