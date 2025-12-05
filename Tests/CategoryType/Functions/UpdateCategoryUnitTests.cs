using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Tests.CategoryType;

public partial class CategoryServiceUnitTests
{
    [Fact]
    public async Task Should_FailUpdateCategory_WhenCategoryDoesNotExist()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_Fail_UpdateCategory_WhenCategoryIsNotOwned()
    {
        _database.Add(_category);
        var input = new Category(_category) { UserId = Guid.NewGuid() };

        await Assert.ThrowsAsync<AuthorizationException>(() => _service.UpdateCategory(input));
    }

    [Fact]
    public async Task Should_FailUpdatingCategory_WhenNameIsEmpty()
    {
        _database.Add(_category);
        _category.Name = "";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenUserIdIsEmpty()
    {
        _database.Add(_category);
        _category.UserId = Guid.Empty;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenColorIsEmpty()
    {
        _database.Add(_category);
        _category.Color = "";

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenColorHasInvalidFormat()
    {
        _database.Add(_category);
        _category.Color = "abcde";

        await Assert.ThrowsAsync<FormatException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenLimitAmountIsNegativ()
    {
        _database.Add(_category);
        _category.Limit!.Amount = -30M;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenLimitAmountIsZero()
    {
        _database.Add(_category);
        _category.Limit!.Amount = 0M;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenLimitPeriodIsNegativ()
    {
        _database.Add(_category);
        _category.Limit!.PeriodDays = -3;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenLimitPeriodIsZero()
    {
        _database.Add(_category);
        _category.Limit!.PeriodDays = 0;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenLimitAmountIsLargerThanFromParent()
    {
        _database.Add(_category);
        var parent = _database.First();
        _category.Limit!.Amount = 500M;
        parent.Limit!.Amount = 44M;
        parent.Children = [_category.Id];
        parent.UserId = _category.UserId;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenLimitAmountIsSmallerThanFromChildren()
    {
        _database.Add(_category);
        var child = _database.First();
        child.Limit!.Amount = 500M;
        child.UserId = _category.UserId;
        _category.Limit!.Amount = 44M;
        _category.Children = [child.Id];

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenChildDoesNotExist()
    {
        _database.Add(_category);
        _category.Children = [Guid.NewGuid()];

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdateCategory_WhenChildIsNotOwned()
    {
        _database.Add(_category);
        var child = _database.First();
        _category.Children = [child.Id];

        await Assert.ThrowsAsync<AuthorizationException>(() => _service.UpdateCategory(_category));
    }

    [Fact]
    public async Task Should_FailUpdataCategory_WhenRelationshipCycleExist()
    {
        _database.Add(_category);
        var child = _database.Find(elem => elem.Children.Any());
        var parent = _database.Find(elem => elem.Id == child!.Children.First());
        var update = new Category(_category) { Children = [child!.Id] };
        parent!.Children = [update.Id];
        child.UserId = update.UserId;
        parent.UserId = update.UserId;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateCategory(update));
    }

    [Fact]
    public async Task Should_NotChangeDatabaseState_WhenUpdateIsEqualToOriginal()
    {
        _database.Add(_category);

        var result = await _service.UpdateCategory(_category);
        var update = result.First();

        Assert.Equal(_category.ModifiedAt, update.ModifiedAt);
    }

    [Fact]
    public async Task Should_ChangeDatabaseState_WhenUpdateIsNotEqualToOriginal()
    {
        _database.Add(_category);
        var input = new Category(_category) { Name = "New Name", Color = "rgb(88,88,88)" };

        var result = await _service.UpdateCategory(input);

        Assert.Equal(input.Name, _category.Name);
        Assert.Equal(input.Color, _category.Color);
        Assert.Equal(input.ModifiedAt, _category.ModifiedAt);
    }

    [Fact]
    public async Task Should_UpdateOldParentCategory_WhenChildrenAreMoved()
    {
        var parent = _database.Find(elem => elem.Children.Any());
        _category.UserId = parent!.UserId;
        _database.Add(_category);
        var input = new Category(_category)
        {
            Children = parent!.Children,
            Limit = new Limit { Amount = 600M, PeriodDays = 30 },
        };

        var result = await _service.UpdateCategory(input);

        Assert.Empty(parent.Children);
    }

    [Fact]
    public async Task Should_ReturnAllUpdatedCategories_WhenChildrenAreMoved()
    {
        var parent = _database.Find(elem => elem.Children.Any());
        _category.UserId = parent!.UserId;
        _database.Add(_category);
        var input = new Category(_category)
        {
            Children = parent!.Children,
            Limit = new Limit { Amount = 600M, PeriodDays = 30 },
        };

        var result = await _service.UpdateCategory(input);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task Should_ReturnUpdatedCategory()
    {
        _database.Add(_category);
        var input = new Category(_category) { Name = "New Name" };

        var result = await _service.UpdateCategory(input);

        Assert.Equal(input.Name, result.First().Name);
    }
}
