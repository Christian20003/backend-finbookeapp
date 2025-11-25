using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    public async Task<Category> CreateMainCategory(Category category)
    {
        _logger.LogDebug("Add new category {name}", category.Name);
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            _logger.LogWarning(LogEvents.CategoryOperationFailed, "Category name is null or empty");
            throw new ArgumentException("Category name is null or empty", nameof(category));
        }
        if (string.IsNullOrWhiteSpace(category.Color))
        {
            _logger.LogWarning(
                LogEvents.CategoryOperationFailed,
                "Category color is null or empty"
            );
            throw new ArgumentException("Category color is null or empty", nameof(category));
        }
        if (
            category.Children.Any()
            && await _collection.ExistCategories(category.Children, category.UserId)
        )
        {
            _logger.LogWarning(
                LogEvents.CategoryOperationFailed,
                "At least one of these categories does not exist {ids}",
                category.Children.ToString()
            );
            throw new ArgumentException(
                "One or more of the provided child categories does not exist",
                nameof(category)
            );
        }

        var result = await _collection.CreateCategory(category);
        _logger.LogInformation(
            LogEvents.CategoryOperationSuccess,
            "A new category {Name} is added: {Id}",
            category.Name,
            category.Id
        );
        return result;
    }
}
