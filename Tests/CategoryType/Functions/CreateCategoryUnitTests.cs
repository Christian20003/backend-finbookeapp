using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Tests.CategoryType;

public partial class CategoryServiceUnitTests
{
    [Fact]
    public async Task Should_FailCreatingCategory_WhenNameIsEmpty()
    {
        _category.Name = "";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingCategory_WhenUserIdIsEmpty()
    {
        _category.UserId = Guid.Empty;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingCategory_WhenColorIsEmpty()
    {
        _category.Color = "";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingCategory_WhenColorHasInvalidFormat()
    {
        _category.Color = "abcde";

        await Assert.ThrowsAsync<FormatException>(() => _service.CreateCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingCategory_WhenChildDoesNotExist()
    {
        _category.UserId = _database.Last().UserId;
        _category.Children = [_database.Last().Id, Guid.NewGuid()];

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingCategory_WhenChildIsNotOwned()
    {
        _category.Children = [_database.Last().Id];

        await Assert.ThrowsAsync<AuthorizationException>(() => _service.CreateCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingCategory_WhenParentDoesNotExist()
    {
        _category.Parent = Guid.NewGuid();

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingCategory_WhenParentIsNotAccessible()
    {
        _category.Parent = _database.First().Id;

        await Assert.ThrowsAsync<AuthorizationException>(() => _service.CreateCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingCategory_WhenCyclicDependeciesDetected()
    {
        _category.Children = [_database[1].Id];
        _category.Parent = _database[2].Id;

        await Assert.ThrowsAsync<AuthorizationException>(() => _service.CreateCategory(_category));
    }

    [Fact]
    public async Task Should_AddCategoryToDatabase_WhenSubCategory()
    {
        var first = _database.First();
        _category.UserId = first.UserId;
        _category.Parent = first.Id;
        await _service.CreateCategory(_category);

        Assert.Contains(_database, elem => elem.Id == _category.Id);
    }

    [Fact]
    public async Task Should_AddCategoryToDatabase_WhenMainCategory()
    {
        await _service.CreateCategory(_category);

        Assert.Contains(_database, elem => elem.Id == _category.Id);
    }

    [Fact]
    public async Task Should_UpdateParentCategory_WhenNewSubcategoryIsAdded()
    {
        var first = _database.First();
        _category.UserId = first.UserId;
        _category.Parent = first.Id;
        await _service.CreateCategory(_category);

        Assert.Contains(first.Children, id => id == _category.Id);
    }

    [Fact]
    public async Task Should_ReturnCreatedMainCategory()
    {
        var result = await _service.CreateCategory(_category);

        Assert.Equal(_category.Id, result.Id);
        Assert.Equal(_category.Name, result.Name);
        Assert.Equal(_category.UserId, result.UserId);
        Assert.Equal(_category.Color, result.Color);
    }
}
