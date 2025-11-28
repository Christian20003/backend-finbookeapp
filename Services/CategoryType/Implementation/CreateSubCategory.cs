using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<Category> CreateSubCategory(Guid parent, Category child)
    {
        _logger.LogDebug("Create new sub category {child} of {parent}", child.ToString(), parent);
        await VerifyCategory(child);
        var category = await VerifyCategory(parent, child.UserId);
        if (await HasCycles(parent, child.Children))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException(
                    $"{child.ToString} produces a cycle in the category order",
                    nameof(child)
                )
            );

        category.Children = category.Children.Append(child.Id);
        await _collection.CreateCategory(child);
        await _collection.UpdateCategory(category);
        return child;
    }
}
