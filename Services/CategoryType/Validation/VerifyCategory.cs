using FinBookeAPI.Attributes;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    /// <summary>
    /// This method verifies if the provided category name and color is valid.
    /// </summary>
    /// <param name="category">
    /// The category that should be verifed.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If the category name or color is null or empty.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the category color is not a valid color format.
    /// </exception>
    private void VerifyCategory(Category category)
    {
        _logger.LogDebug("Verify category {name}", category.Name);
        var colorValidator = new ColorAttribute();
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
            throw new ArgumentException(
                $"Category color of {category.Name} is null or empty",
                nameof(category)
            );
        }
        if (!colorValidator.IsValid(category.Color))
        {
            _logger.LogWarning(
                LogEvents.CategoryOperationFailed,
                "Category color is not a valid color encoding"
            );
            throw new FormatException(
                $"Category color of {category.Name} is not a valid color encoding"
            );
        }
    }
}
