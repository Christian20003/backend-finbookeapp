using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Attributes;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    /// <summary>
    /// This method verifies if the provided category is valid.
    /// </summary>
    /// <param name="category">
    /// The category that should be verifed.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If the category name is null or empty.
    /// If the category color is null or empty.
    /// If the limit amount is smaller or equal to zero.
    /// If the limit amount is larger than that of its potential parent.
    /// If the limit amount is smaller than the sum of its potential children.
    /// If the limit period is smaller or equal to zero.
    /// If a category child does not exist in the database.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the category color is not a valid color format.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If a category child is not accessible by the user.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If a reading operation has been canceled.
    /// </exception>
    private async Task VerifyCategory(Category category)
    {
        _logger.LogDebug("Verify category {category}", category.ToString());
        var colorValidator = new ColorAttribute();
        if (category.UserId == Guid.Empty)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("UserId of category is not valid", nameof(category))
            );
        if (string.IsNullOrWhiteSpace(category.Name))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("Category name is null or empty", nameof(category))
            );
        if (string.IsNullOrWhiteSpace(category.Color))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("Category color is null or empty", nameof(category))
            );
        if (!colorValidator.IsValid(category.Color))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new FormatException("Category color is not a valid color encoding")
            );
        var parent = await _collection.HasParent(category.Id, category.UserId);
        var children = await _collection.GetCategories(category.Children);
        if (!category.Children.All(childId => children.Any(child => child.Id == childId)))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("Category children does not exist", nameof(category))
            );
        if (!children.All(child => child.UserId == category.UserId))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new AuthorizationException("Category children is not accessible")
            );
        if (category.Limit is not null)
        {
            var sum = children.Aggregate<Category, decimal>(
                0,
                (sum, cat) => sum += cat.Limit?.Amount ?? 0
            );
            if (category.Limit.Amount <= 0)
                Logging.ThrowAndLogWarning(
                    _logger,
                    LogEvents.CategoryOperationFailed,
                    new ArgumentException("Limit amount must be larger than zero", nameof(category))
                );
            if (parent is not null && parent.Limit!.Amount < category.Limit.Amount)
                Logging.ThrowAndLogWarning(
                    _logger,
                    LogEvents.CategoryOperationFailed,
                    new ArgumentException(
                        "Limit amount must be smaller than the amount of the parent",
                        nameof(category)
                    )
                );
            if (sum != 0 && category.Limit.Amount < sum)
                Logging.ThrowAndLogWarning(
                    _logger,
                    LogEvents.CategoryOperationFailed,
                    new ArgumentException(
                        "Limit amount must be larger than the amount of its children",
                        nameof(category)
                    )
                );
            if (category.Limit.PeriodDays <= 0)
                Logging.ThrowAndLogWarning(
                    _logger,
                    LogEvents.CategoryOperationFailed,
                    new ArgumentException("Limit period must be larger than zero", nameof(category))
                );
        }
    }
}
