using FinBookeAPI.AppConfig.Documentation;
using FinBookeAPI.Attributes;
using FinBookeAPI.Models.CategoryType;
using FinBookeAPI.Models.Configuration;
using FinBookeAPI.Models.Exceptions;

namespace FinBookeAPI.Services.CategoryType;

public partial class CategoryService : ICategoryService
{
    /// <summary>
    /// This method verifies if a existing category is valid.
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
    /// <exception cref="EntityNotFoundException">
    /// If the database object could not be found.
    /// If the database object of a child could not be found.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If a category child is not accessible by the user.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If a reading operation has been canceled.
    /// </exception>
    private async Task<Category> VerifyExistingCategory(Category category)
    {
        _logger.LogDebug("Verify existing category {category}", category.ToString());
        var entity = await VerifyCategoryAccess(category.Id, category.UserId);
        VerifyCategoryName(category.Name);
        VerifyCategoryColor(category.Color);
        if (category.Limit is null)
            await VerifyCategoryChildren(category.Children, category.UserId);
        else
            await VerifyCategoryLimit(
                category.Id,
                category.UserId,
                category.Limit,
                category.Children
            );
        return entity;
    }

    /// <summary>
    /// This method verifies if a new category is valid.
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
    /// <exception cref="EntityNotFoundException">
    /// If the database object of a child could not be found.
    /// </exception>
    /// <exception cref="DuplicateEntityException">
    /// If the category id does already exist in the database.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If a category child is not accessible by the user.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If a reading operation has been canceled.
    /// </exception>
    private async Task VerifyNewCategory(Category category)
    {
        _logger.LogDebug("Verify new category {category}", category.ToString());
        var entity = await VerfiyCategoryId(category.Id);
        if (entity is not null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new DuplicateEntityException("Category id is does already exist")
            );
        VerifyCategoryName(category.Name);
        VerifyCategoryColor(category.Color);
        if (category.Limit is null)
            await VerifyCategoryChildren(category.Children, category.UserId);
        else
            await VerifyCategoryLimit(
                category.Id,
                category.UserId,
                category.Limit,
                category.Children
            );
    }

    /// <summary>
    /// This method verifies a category id.
    /// </summary>
    /// <param name="categoryId">
    /// The category id that should be verified.
    /// </param>
    /// <returns>
    /// The category object from the database that corresponds
    /// to the provided id. If the category does not exist, this
    /// method returns <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the provided category id is an empty GUID.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    private async Task<Category?> VerfiyCategoryId(Guid categoryId)
    {
        _logger.LogDebug("Verify category id {id}", categoryId);
        if (categoryId == Guid.Empty)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("Category id is not valid", nameof(categoryId))
            );
        return await _collection.GetCategory(category => category.Id == categoryId);
    }

    /// <summary>
    /// This method verifies if the a user has access on a
    /// category.
    /// </summary>
    /// <param name="categoryId">
    /// The id of the category to which the user wants acccess.
    /// </param>
    /// <param name="userId">
    /// The id of the user.
    /// </param>
    /// <returns>
    /// The category object from the database.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the provided category id is an empty GUID.
    /// If the provided user id is an empty GUID.
    /// </exception>
    /// <exception cref="EntityNotFoundException">
    /// If the database object could not be found.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the user does not have access to the category.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    private async Task<Category> VerifyCategoryAccess(Guid categoryId, Guid userId)
    {
        _logger.LogDebug("Verify category access {id} of user {user}", categoryId, userId);
        var entity = await VerfiyCategoryId(categoryId);
        if (userId == Guid.Empty)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("UserId of category is not valid", nameof(userId))
            );
        if (entity is null)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new EntityNotFoundException("Category does not exist")
            );
        if (entity.UserId != userId)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new AuthorizationException("Category is not accessible")
            );
        return entity;
    }

    /// <summary>
    /// This method verifies the name of a category.
    /// </summary>
    /// <param name="name">
    /// The name that should be verified.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If the name is an empty string.
    /// </exception>
    private void VerifyCategoryName(string name)
    {
        _logger.LogDebug("Verify category name {name}", name);
        if (string.IsNullOrWhiteSpace(name))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("Category name is null or empty", nameof(name))
            );
    }

    /// <summary>
    /// This method verifies the color of a category.
    /// </summary>
    /// <param name="color">
    /// The color that should be verified.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If the color is an empty string.
    /// </exception>
    /// <exception cref="FormatException">
    /// If the color is not a valid color format.
    /// </exception>
    private void VerifyCategoryColor(string color)
    {
        _logger.LogDebug("Verify category color {color}", color);
        var colorValidator = new ColorAttribute();
        if (string.IsNullOrWhiteSpace(color))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("Category color is null or empty", nameof(color))
            );
        if (!colorValidator.IsValid(color))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new FormatException("Category color is not a valid color encoding")
            );
    }

    /// <summary>
    /// This method verifies the category children list.
    /// </summary>
    /// <param name="childrenIds">
    /// A list containing all ids of each children.
    /// </param>
    /// <param name="userId">
    /// The id of the user.
    /// </param>
    /// <returns>
    /// A list of all category objects of each child from the
    /// database.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the provided user id is an empty GUID.
    /// If any of the provided child ids is an empty GUID.
    /// </exception>
    /// <exception cref="EntityNotFoundException">
    /// If the database object of a child could not be found.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the user does not have access to a child.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    private async Task<IEnumerable<Category>> VerifyCategoryChildren(
        IEnumerable<Guid> childrenIds,
        Guid userId
    )
    {
        _logger.LogDebug("Verify category children");
        if (userId == Guid.Empty)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("UserId of category is not valid", nameof(userId))
            );
        if (childrenIds.Any(elem => elem == Guid.Empty))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("Children id is not valid", nameof(childrenIds))
            );
        var children = await _collection.GetCategories(category =>
            childrenIds.Contains(category.Id)
        );
        if (!childrenIds.All(childId => children.Any(child => child.Id == childId)))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new EntityNotFoundException("Category children does not exist")
            );
        if (!children.All(child => child.UserId == userId))
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new AuthorizationException("Category children is not accessible")
            );
        return children;
    }

    /// <summary>
    /// This method verifies the limit of a category.
    /// </summary>
    /// <param name="categoryId">
    /// The id of the category.
    /// </param>
    /// <param name="userId">
    /// The if of the user.
    /// </param>
    /// <param name="limit">
    /// The limit object if that category.
    /// </param>
    /// <param name="childrenIds">
    /// A list containing all ids of each children.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If the provided user id is an empty GUID.
    /// If any of the provided child ids is an empty GUID.
    /// If the limit amount is smaller or equal to zero.
    /// If the limit amount is larger than that of its potential parent.
    /// If the limit amount is smaller than the sum of its potential children.
    /// If the limit period is smaller or equal to zero.
    /// </exception>
    /// <exception cref="EntityNotFoundException">
    /// If the database object of a child could not be found.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// If the user does not have access to a child.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// If the reading operation has been canceled.
    /// </exception>
    private async Task VerifyCategoryLimit(
        Guid categoryId,
        Guid userId,
        Limit limit,
        IEnumerable<Guid> childrenIds
    )
    {
        _logger.LogDebug("Verify category limit {limit}", limit.ToString());
        var children = await VerifyCategoryChildren(childrenIds, userId);
        var parent = await _collection.GetCategory(category =>
            category.Children.Contains(categoryId)
        );
        var sum = children.Aggregate<Category, decimal>(
            0,
            (sum, cat) => sum += cat.Limit?.Amount ?? 0
        );
        if (limit.Amount <= 0)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("Limit amount must be larger than zero", nameof(limit))
            );
        if (parent is not null && parent.Limit!.Amount < limit.Amount)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException(
                    "Limit amount must be smaller than the amount of the parent",
                    nameof(limit)
                )
            );
        if (sum != 0 && limit.Amount < sum)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException(
                    "Limit amount must be larger than the amount of its children",
                    nameof(limit)
                )
            );
        if (limit.PeriodDays <= 0)
            Logging.ThrowAndLogWarning(
                _logger,
                LogEvents.CategoryOperationFailed,
                new ArgumentException("Limit period must be larger than zero", nameof(limit))
            );
    }
}
