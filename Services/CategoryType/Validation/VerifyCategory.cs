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
    /// <returns>
    /// The parent category if it is set, otherwise <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the category name is null or empty.
    /// If the category color is null or empty.
    /// If a category child does not exist in the database.
    /// If the category parent does not exist in the database.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the category color is not a valid color format.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If a category child is not accessible by the user.
    /// If the category parent is not accessible by the user.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If a reading operation has been canceled.
    /// </exception>
    private async Task<Category?> VerifyCategory(Category category)
    {
        _logger.LogDebug("Verify category {category}", category.ToString());
        var colorValidator = new ColorAttribute();
        Category? parent = null;
        if (category.UserId == Guid.Empty)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException(
                    $"UserId {category.UserId} of category {category.Id} is not valid",
                    nameof(category)
                )
            );
        if (string.IsNullOrWhiteSpace(category.Name))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException(
                    $"Category name is null or empty from {category.Id}",
                    nameof(category)
                )
            );
        if (string.IsNullOrWhiteSpace(category.Color))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException(
                    $"Category color is null or empty from {category.Id}",
                    nameof(category)
                )
            );
        if (!colorValidator.IsValid(category.Color))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new FormatException(
                    $"Category color is not a valid color encoding from {category.Id}"
                )
            );
        if (category.Parent.HasValue)
        {
            parent = await _collection.GetCategory(category.Parent.Value);
            if (parent is null)
                Logging.ThrowAndLogWarning(
                    _logger,
                    LogEvents.CategoryOperationFailed,
                    new ArgumentException(
                        $"Upper category {parent} does not exist",
                        nameof(category)
                    )
                );
            if (parent.UserId != category.UserId)
                Logging.ThrowAndLogWarning(
                    _logger,
                    LogEvents.CategoryOperationFailed,
                    new AuthorizationException(
                        $"User {category.UserId} does not have access on parent: {parent.Id}"
                    )
                );
        }
        if (!await _collection.ExistCategories(category.Children))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException(
                    $"Some children of {category.Id} do not exist: [{string.Join(", ", category.Children)}]",
                    nameof(category)
                )
            );
        if (!await _collection.HasAccess(category.UserId, category.Children))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new AuthorizationException(
                    $"User {category.UserId} does not have access on children: {string.Join(", ", category.Children)}"
                )
            );

        return parent;
    }
}
