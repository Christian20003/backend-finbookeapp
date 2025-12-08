using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Tests.CategoryType;

public partial class CategoryServiceUnitTests
{
    [Fact]
    public async Task Should_FailRemovingCategory_WhenCategoryDoesNotExist()
    {
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.DeleteCategory(_category));
    }

    [Fact]
    public async Task Should_Fail_RemovingCategory_WhenCategoryIsNotOwned()
    {
        _database.Add(_category);
        var input = new Category(_category) { UserId = Guid.NewGuid() };

        await Assert.ThrowsAsync<AuthorizationException>(() => _service.DeleteCategory(input));
    }

    [Fact]
    public async Task Should_RemoveCategoryFromDatabase()
    {
        _database.Add(_category);

        await _service.DeleteCategory(_category);

        Assert.DoesNotContain(_database, elem => elem.Id == _category.Id);
    }

    [Fact]
    public async Task Should_RemoveCategoryIdFromParents_WhenCategoryIsDeleted()
    {
        var parent = _database.First();
        parent.UserId = _category.UserId;
        parent.Children = [_category.Id];
        _database.Add(_category);

        await _service.DeleteCategory(_category);

        Assert.DoesNotContain(parent.Children, id => id == _category.Id);
    }

    [Fact]
    public async Task Should_ReturnRemovedCategory()
    {
        _database.Add(_category);

        var result = await _service.DeleteCategory(_category);

        Assert.Equal(_category.Name, result.Name);
        Assert.Equal(_category.Color, result.Color);
        Assert.Equal(_category.Limit!.Amount, result.Limit!.Amount);
        Assert.Equal(_category.Limit!.PeriodDays, result.Limit!.PeriodDays);
        Assert.Equal(_category.Children, result.Children);
    }
}
