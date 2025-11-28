using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<Category> CreateCategory(Category category)
    {
        _logger.LogDebug("Add new category {category}", category.ToString());
        var parent = await VerifyCategory(category);
        if (parent is not null)
        {
            if (await HasCycles(parent.Id, category.Children))
                Logging.ThrowAndLogWarning(
                    _logger,
                    LogEvents.CategoryOperationFailed,
                    new ArgumentException(
                        $"{category} produces a cycle in the category order",
                        nameof(category)
                    )
                );
            parent.Children = parent.Children.Append(category.Id);
            await _collection.UpdateCategory(parent);
        }
        await _collection.CreateCategory(category);

        _logger.LogInformation(
            LogEvents.CategoryOperationSuccess,
            "{category} is added",
            category.ToString()
        );
        return category;
    }
}
