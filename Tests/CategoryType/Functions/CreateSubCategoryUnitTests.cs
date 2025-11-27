using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Tests.CategoryType;

public partial class CategoryServiceUnitTests
{
    [Fact]
    public async Task Should_FailCreatingSubCategory_WhenNameIsEmpty()
    {
        var first = _database.First();
        _category.Name = "";
        _category.UserId = first.UserId;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateSubCategory(first.Id, _category)
        );
    }

    [Fact]
    public async Task Should_FailCreatingSubCategory_WhenUserIdIsEmpty()
    {
        _category.UserId = Guid.Empty;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateMainCategory(_category));
    }

    [Fact]
    public async Task Should_FailCreatingSubCategory_WhenColorIsEmpty()
    {
        var first = _database.First();
        _category.Color = "";
        _category.UserId = first.UserId;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateSubCategory(first.Id, _category)
        );
    }

    [Fact]
    public async Task Should_FailCreatingSubCategory_WhenColorHasInvalidFormat()
    {
        var first = _database.First();
        _category.Color = "abcde";
        _category.UserId = first.UserId;

        await Assert.ThrowsAsync<FormatException>(
            () => _service.CreateSubCategory(first.Id, _category)
        );
    }

    [Fact]
    public async Task Should_FailCreatingSubCategory_WhenChildDoesNotExist()
    {
        var first = _database.First();
        _category.Children = [new Guid()];
        _category.UserId = first.UserId;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateSubCategory(first.Id, _category)
        );
    }

    [Fact]
    public async Task Should_FailCreatingSubCategory_WhenChildIsNotAccessible()
    {
        var first = _database.First();
        _category.Children = [_database.Last().Id];
        _category.UserId = first.UserId;

        await Assert.ThrowsAsync<AuthorizationException>(
            () => _service.CreateSubCategory(first.Id, _category)
        );
    }

    [Fact]
    public async Task Should_FailCreatingSubCategory_WhenParentDoesNotExist()
    {
        var first = _database.First();
        _category.UserId = first.UserId;

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateSubCategory(Guid.NewGuid(), _category)
        );
    }

    [Fact]
    public async Task Should_FailCreatingSubCategory_WhenParentIsNotAccessible()
    {
        var first = _database.First();

        await Assert.ThrowsAsync<AuthorizationException>(
            () => _service.CreateSubCategory(first.Id, _category)
        );
    }

    [Fact]
    public async Task Should_FailCreatingSubCategory_WhenCyclicDependeciesDetected()
    {
        _category.Children = [_database[1].Id];

        await Assert.ThrowsAsync<AuthorizationException>(
            () => _service.CreateSubCategory(_database[2].Id, _category)
        );
    }

    [Fact]
    public async Task Should_AddSubCategoryToDatabase()
    {
        var first = _database.First();
        _category.UserId = first.UserId;
        await _service.CreateSubCategory(first.Id, _category);

        Assert.Contains(_database, elem => elem.Id == _category.Id);
    }

    [Fact]
    public async Task Should_UpdateParentCategory_WhenNewSubcategoryIsAdded()
    {
        var first = _database.First();
        _category.UserId = first.UserId;
        await _service.CreateSubCategory(first.Id, _category);

        Assert.Contains(first.Children, id => id == _category.Id);
    }

    [Fact]
    public async Task Should_ReturnCreatedSubCategory()
    {
        var first = _database.First();
        _category.UserId = first.UserId;
        var result = await _service.CreateSubCategory(first.Id, _category);

        Assert.Equal(_category.Id, result.Id);
        Assert.Equal(_category.Name, result.Name);
        Assert.Equal(_category.UserId, result.UserId);
        Assert.Equal(_category.Color, result.Color);
    }
}
